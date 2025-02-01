using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MeshWireframeUpdater : MonoBehaviour
{
    private static readonly Color[] _COLORS = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
    };

    private void Awake()
    {
        UpdateChildMeshes();
    }

    [ContextMenu("Update Child Meshes")]
    public void UpdateChildMeshes()
    {
        // Process children with MeshFilter + MeshRenderer
        var meshFilters = GetComponentsInChildren<MeshFilter>(includeInactive: true);
        foreach (var meshFilter in meshFilters)
        {
            // Make sure there's a MeshRenderer and that it is enabled
            if (!meshFilter.gameObject.TryGetComponent<MeshRenderer>(out var meshRenderer) ||
                !meshRenderer.enabled ||
                !meshFilter.gameObject.activeSelf)
            {
                continue;
            }

            var mesh = meshFilter.sharedMesh;
            if (mesh == null)
                continue;

            var colors = SortedColoring(mesh);
            if (colors != null)
            {
                mesh.SetColors(colors);
                Debug.Log($"Updated mesh colors for MeshFilter on {meshFilter.gameObject.name}", meshFilter.gameObject);
            }
        }

        // Process children with SkinnedMeshRenderer
        var skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
        foreach (var smr in skinnedMeshRenderers)
        {
            if (!smr.enabled || !smr.gameObject.activeSelf)
                continue;

            var mesh = smr.sharedMesh;
            if (mesh == null)
                continue;

            var colors = SortedColoring(mesh);
            if (colors != null)
            {
                mesh.SetColors(colors);
                Debug.Log($"Updated mesh colors for SkinnedMeshRenderer on {smr.gameObject.name}", smr.gameObject);
            }
        }
    }

    private Color[] SortedColoring(Mesh mesh)
    {
        int n = mesh.vertexCount;
        int[] labels = new int[n];

        List<int[]> triangles = GetSortedTriangles(mesh.triangles);
        triangles.Sort((int[] t1, int[] t2) =>
        {
            int i = 0;
            while (i < t1.Length && i < t2.Length)
            {
                if (t1[i] < t2[i]) return -1;
                if (t1[i] > t2[i]) return 1;
                i += 1;
            }
            if (t1.Length < t2.Length) return -1;
            if (t1.Length > t2.Length) return 1;
            return 0;
        });

        foreach (int[] triangle in triangles)
        {
            List<int> availableLabels = new List<int>() { 1, 2, 3 };
            foreach (int vertexIndex in triangle)
            {
                if (availableLabels.Contains(labels[vertexIndex]))
                    availableLabels.Remove(labels[vertexIndex]);
            }
            foreach (int vertexIndex in triangle)
            {
                if (labels[vertexIndex] == 0)
                {
                    if (availableLabels.Count == 0)
                    {
                        Debug.LogError("Could not find color");
                        return null;
                    }
                    labels[vertexIndex] = availableLabels[0];
                    availableLabels.RemoveAt(0);
                }
            }
        }

        Color[] colors = new Color[n];
        for (int i = 0; i < n; i++)
            colors[i] = labels[i] > 0 ? _COLORS[labels[i] - 1] : _COLORS[0];

        return colors;
    }

    private List<int[]> GetSortedTriangles(int[] triangles)
    {
        var result = new List<int[]>();
        for (var i = 0; i < triangles.Length; i += 3)
        {
            var t = new List<int> { triangles[i], triangles[i + 1], triangles[i + 2] };
            t.Sort();
            result.Add(t.ToArray());
        }
        return result;
    }
}