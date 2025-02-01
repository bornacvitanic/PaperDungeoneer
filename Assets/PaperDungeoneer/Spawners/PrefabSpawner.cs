using UnityEngine;
using UnityEngine.Events;

namespace PaperDungoneer.Spawners
{
    public class PrefabSpawner : MonoBehaviour
    {
        public UnityEvent<GameObject> OnPrefabSpawned;

        public void SpawnPrefab(GameObject prefab)
        {
            var spawnedPrefab = Instantiate(prefab, transform);
            OnPrefabSpawned?.Invoke(spawnedPrefab);
        }
    }
}