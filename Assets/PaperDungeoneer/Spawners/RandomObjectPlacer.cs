using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.Spawners
{
    public class RandomObjectPlacer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> prefabs;

        public UnityEvent<GameObject> OnObjectPlaced;

        private GameObject instance;

        private void Awake()
        {
            PlaceRandomObject();
        }

        [ContextMenu("Place Random Object")]
        private void PlaceRandomObject()
        {
            if (prefabs == null || prefabs.Count == 0)
            {
                Debug.LogWarning("No prefabs assigned to the list!");
                return;
            }

            if (instance != null)
            {
                Destroy(instance);
                instance = null;
            }

            GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Count)];
            instance = Instantiate(selectedPrefab, transform);
            OnObjectPlaced.Invoke(instance);
        }

        
        #region GizmoVariables
        private int currentPrefabIndex = 0;
        private float timer = 0f;
        private const float previewInterval = 5f;
        #endregion
        
        private void OnDrawGizmos()
        {
            if (prefabs == null || prefabs.Count == 0)
                return;
            if (instance != null) return;

            timer += Time.deltaTime;
            if (timer >= previewInterval)
            {
                timer = 0f;
                currentPrefabIndex = (currentPrefabIndex + 1) % prefabs.Count;
            }

            GameObject prefab = prefabs[currentPrefabIndex];
            if (prefab == null)
                return;

            MeshFilter[] meshFilters = prefab.GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    Transform meshTransform = meshFilter.transform;

                    // Compute the correct world-space transform
                    Matrix4x4 matrix = transform.localToWorldMatrix * meshTransform.localToWorldMatrix;

                    Gizmos.color = new Color(1, 1, 1, 0.5f); // Semi-transparent white
                    Gizmos.matrix = matrix;
                    Gizmos.DrawMesh(meshFilter.sharedMesh, Vector3.zero, Quaternion.identity, Vector3.one);
                }
            }
        }
    }
}