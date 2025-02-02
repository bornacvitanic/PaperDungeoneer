using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeshFolderMultipleFolds : MonoBehaviour
{
    [SerializeField] private float flattenDuration = 2f;
    [SerializeField] private int foldCount = 3;
    [SerializeField] private float foldDuration = 2f;

    public UnityEvent OnFold;
    private Coroutine flatteningRoutine;

    // Holds a baked/duplicated mesh along with its working vertex arrays.
    private class FolderMesh
    {
        public Mesh mesh;
        public Transform owner;
        public Vector3[] originalVertices;   // as originally baked/duplicated
        public Vector3[] currentVertices;    // working state; initially flattened
    }

    private readonly List<FolderMesh> folderMeshes = new List<FolderMesh>();

    [ContextMenu("Fold")]
    public void Fold(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target not set.", this);
            return;
        }
        
        CollectMeshes(target);
        if (folderMeshes.Count == 0)
        {
            Debug.LogWarning($"No MeshFilters or SkinnedMeshRenderers found under target '{target.name}'. Aborting folding.");
            return;
        }

        if (flatteningRoutine != null) StopCoroutine(flatteningRoutine);
        flatteningRoutine = StartCoroutine(FlattenThenFold(target));
    }

    private void CollectMeshes(GameObject target)
    {
        folderMeshes.Clear();
        // Process MeshFilter-based meshes.
        var meshFilters = target.GetComponentsInChildren<MeshFilter>();
        foreach (var mf in meshFilters)
        {
            if (mf.sharedMesh == null)
            {
                Debug.LogWarning($"MeshFilter on '{mf.gameObject.name}' has no mesh; skipping.");
                continue;
            }

            // Duplicate the mesh to avoid modifying the original asset.
            var meshCopy = Instantiate(mf.sharedMesh);
            meshCopy.name = mf.sharedMesh.name + " Folded";
            mf.mesh = meshCopy;
            AddFolderMesh(meshCopy, mf.transform);
        }

        // Process SkinnedMeshRenderer-based meshes.
        var skinnedRenderers = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var smr in skinnedRenderers)
        {
            if (smr.sharedMesh == null)
            {
                Debug.LogWarning($"SkinnedMeshRenderer on '{smr.gameObject.name}' has no mesh; skipping.");
                continue;
            }

            // Bake the current state of the skinned mesh into a new mesh.
            var meshCopy = new Mesh();
            smr.BakeMesh(meshCopy);
            meshCopy.name = smr.sharedMesh.name + " Folded";
            smr.sharedMesh = meshCopy;
            AddFolderMesh(meshCopy, smr.transform);
        }
    }

    // Prepares the FolderMesh by storing the original vertices and computing the flattened state.
    private void AddFolderMesh(Mesh mesh, Transform owner)
    {
        var originalVerts = mesh.vertices;
        var flattenedVerts = new Vector3[originalVerts.Length];
        for (var i = 0; i < originalVerts.Length; i++)
        {
            var v = originalVerts[i];
            flattenedVerts[i] = new Vector3(v.x, v.y, 0f); // flatten z to 0
        }

        // Set the mesh to its flattened state initially.
        mesh.vertices = flattenedVerts;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var fm = new FolderMesh
        {
            mesh = mesh,
            owner = owner,
            originalVertices = originalVerts,
            currentVertices = flattenedVerts
        };

        folderMeshes.Add(fm);
    }

    private IEnumerator FlattenThenFold(GameObject target)
    {
        // --- FLATTEN PHASE ---
        Debug.Log("Starting flattening phase...");
        var flattenElapsed = 0f;
        while (flattenElapsed < flattenDuration)
        {
            var t = flattenElapsed / (flattenDuration*1.3f);
            foreach (var fm in folderMeshes)
            {
                var verts = new Vector3[fm.originalVertices.Length];
                for (var i = 0; i < verts.Length; i++)
                {
                    // Transform vertex to target's local space.
                    var localVertex = fm.owner.TransformPoint(fm.originalVertices[i]);
                    var targetLocalVertex = target.transform.InverseTransformPoint(localVertex);

                    // Lerp only the Z-axis to flatten the mesh, keeping X and Y unchanged.
                    targetLocalVertex.z = Mathf.Lerp(targetLocalVertex.z, 0f, t);

                    // Transform back to mesh's local space.
                    var worldVertex = target.transform.TransformPoint(targetLocalVertex);
                    verts[i] = fm.owner.InverseTransformPoint(worldVertex);
                }
                fm.mesh.vertices = verts;
                fm.mesh.RecalculateNormals();
                fm.mesh.RecalculateBounds();
                fm.currentVertices = verts;
            }
            flattenElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Flattening complete.");

        // --- FOLD PHASES ---
        // Calculate the collective center of all meshes in the target's local space.
        Vector3 collectiveCenter = Vector3.zero;
        int totalVertices = 0;
        foreach (var fm in folderMeshes)
        {
            foreach (var v in fm.currentVertices)
            {
                // Transform vertex to target's local space.
                var localVertex = fm.owner.TransformPoint(v);
                var targetLocalVertex = target.transform.InverseTransformPoint(localVertex);

                collectiveCenter += targetLocalVertex;
                totalVertices++;
            }
        }
        collectiveCenter /= totalVertices;

        // Alternate fold axes: even folds rotate around x (folding along y) and odd folds rotate around y (folding along x).
        for (var foldIndex = 0; foldIndex < foldCount; foldIndex++)
        {
            // Determine fold axis and crease direction.
            var isXFold = (foldIndex % 2 == 0);

            Debug.Log($"Starting fold phase {foldIndex + 1} using {(isXFold ? "x-axis" : "y-axis")} rotation.");
            OnFold?.Invoke();

            // Animate the fold for this phase over foldDuration.
            var foldElapsed = 0f;
            while (foldElapsed < foldDuration)
            {
                var t = foldElapsed / foldDuration;
                foreach (var fm in folderMeshes)
                {
                    // We'll work on a new vertices array.
                    var verts = new Vector3[fm.currentVertices.Length];
                    // Determine the rotation axis vector in local space.
                    var rotationAxis = isXFold ? Vector3.right : Vector3.up;
                    for (var i = 0; i < verts.Length; i++)
                    {
                        // Transform vertex to target's local space.
                        var localVertex = fm.owner.TransformPoint(fm.currentVertices[i]);
                        var targetLocalVertex = target.transform.InverseTransformPoint(localVertex);

                        // Check if this vertex is above the crease (if folding around x-axis, use y; if y-axis, use x).
                        var coord = isXFold ? targetLocalVertex.y : targetLocalVertex.x;
                        if (coord > (isXFold ? collectiveCenter.y : collectiveCenter.x))
                        {
                            // The crease line for this vertex: preserve the coordinate along the folding direction.
                            // For an x-axis rotation, the crease point for vertex v is (v.x, collectiveCenter.y, v.z).
                            // For a y-axis rotation, it's (collectiveCenter.x, v.y, v.z).
                            var pivot = isXFold ? new Vector3(targetLocalVertex.x, collectiveCenter.y, targetLocalVertex.z)
                                                : new Vector3(collectiveCenter.x, targetLocalVertex.y, targetLocalVertex.z);
                            // Relative position from pivot.
                            var relPos = targetLocalVertex - pivot;
                            // Rotate from 0 to 180 degrees over time.
                            var angle = 180f * t;
                            var rot = Quaternion.AngleAxis(angle, rotationAxis);
                            targetLocalVertex = pivot + rot * relPos;
                        }

                        // Transform back to mesh's local space.
                        var worldVertex = target.transform.TransformPoint(targetLocalVertex);
                        verts[i] = fm.owner.InverseTransformPoint(worldVertex);
                    }
                    fm.mesh.vertices = verts;
                    fm.mesh.RecalculateNormals();
                    fm.mesh.RecalculateBounds();
                }
                foldElapsed += Time.deltaTime;
                yield return null;
            }

            // End of fold phase: update currentVertices for each FolderMesh.
            foreach (var fm in folderMeshes)
            {
                var verts = new Vector3[fm.currentVertices.Length];
                var rotationAxis = isXFold ? Vector3.right : Vector3.up;
                for (var i = 0; i < verts.Length; i++)
                {
                    // Transform vertex to target's local space.
                    var localVertex = fm.owner.TransformPoint(fm.currentVertices[i]);
                    var targetLocalVertex = target.transform.InverseTransformPoint(localVertex);

                    // Check if this vertex is above the crease (if folding around x-axis, use y; if y-axis, use x).
                    var coord = isXFold ? targetLocalVertex.y : targetLocalVertex.x;
                    if (coord > (isXFold ? collectiveCenter.y : collectiveCenter.x))
                    {
                        var pivot = isXFold ? new Vector3(targetLocalVertex.x, collectiveCenter.y, targetLocalVertex.z)
                                            : new Vector3(collectiveCenter.x, targetLocalVertex.y, targetLocalVertex.z);
                        // A full 180° rotation; note that a 180° rotation simply mirrors the vertex across the crease.
                        targetLocalVertex = pivot + Quaternion.AngleAxis(180f, rotationAxis) * (targetLocalVertex - pivot);
                    }

                    // Transform back to mesh's local space.
                    var worldVertex = target.transform.TransformPoint(targetLocalVertex);
                    verts[i] = fm.owner.InverseTransformPoint(worldVertex);
                }
                fm.currentVertices = verts;
                fm.mesh.vertices = verts;
                fm.mesh.RecalculateNormals();
                fm.mesh.RecalculateBounds();
            }
            Debug.Log($"Fold phase {foldIndex + 1} complete.");
        }

        Debug.Log("All folds complete.");
    }
}