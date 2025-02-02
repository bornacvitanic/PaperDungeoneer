using System.Collections.Generic;
using UnityEngine;

namespace PaperDungeoneer.Spawners.Data
{
    [CreateAssetMenu(fileName = "WeightedPrefabRepository", menuName = "Spawners/WeightedPrefabRepository", order = 0)]
    public class WeightedPrefabRepository : ScriptableObject
    {
        [System.Serializable]
        public struct WeightedPrefab
        {
            public GameObject prefab;
            public float weight;
        }
        
        public List<WeightedPrefab> prefabs;
    }
}