using System.Collections.Generic;
using PaperDungeoneer.Spawners.Data;
using UnityEngine;

namespace PaperDungeoneer.Spawners.Room
{
    public class RoomSpawner : MonoBehaviour
    {
        [Header("Room Settings")] 
        [SerializeField] private Vector2Int roomWidthRange = new(1, 5);
        [SerializeField] private Vector2Int roomLengthRange = new(1, 5);

        [Header("Wall and Corner Prefabs")]
        [SerializeField] private WeightedPrefabRepository wallPrefabs; // List of weighted wall prefabs
        [SerializeField] private WeightedPrefabRepository cornerPrefabs; // List of weighted corner prefabs
        [SerializeField] private WeightedPrefabRepository floorPrefabs; // List of weighted floor prefabs

        [Header("Center Floor Prefab")]
        [SerializeField] private GameObject centerFloorPrefab; // Center floor piece (2x2 size)

        [Header("Wall and Corner Dimensions")]
        [SerializeField] private float wallWidth = 1.0f; // Width of the wall prefabs
        
        [Header("Instances")]
        [SerializeField] private List<GameObject> instances;

        private int RoomWidth { get; set; }
        private int RoomLength { get; set; }


        private void Start()
        {
            if (Application.isPlaying || instances == null || instances.Count == 0) GenerateRoom();
        }

        [ContextMenu("Generate Room")]
        public void GenerateRoom()
        {
            // Clear existing room instances
            foreach (var instance in instances)
            {
                if (Application.isPlaying) Destroy(instance);
                else DestroyImmediate(instance);
            }
            instances.Clear();

            RoomWidth = GetRandomOddNumber(roomWidthRange.x, roomWidthRange.y) + 1;
            RoomLength = GetRandomOddNumber(roomLengthRange.x, roomLengthRange.y) + 1;

            // Calculate the total size of the room
            float totalWidth = RoomWidth * wallWidth;
            float totalLength = RoomLength * wallWidth;

            // Spawn corners
            instances.Add(SpawnCorner(new Vector3(-totalWidth / 2, 0, -totalLength / 2), 0)); // Bottom-left corner
            instances.Add(SpawnCorner(new Vector3(totalWidth / 2, 0, -totalLength / 2), 270)); // Bottom-right corner
            instances.Add(SpawnCorner(new Vector3(totalWidth / 2, 0, totalLength / 2), 180)); // Top-right corner
            instances.Add(SpawnCorner(new Vector3(-totalWidth / 2, 0, totalLength / 2), 90)); // Top-left corner

            // Spawn walls
            for (int i = 1; i < RoomWidth; i++)
            {
                float xPos = -totalWidth / 2 + i * wallWidth;
                instances.Add(SpawnWall(new Vector3(xPos, 0, -totalLength / 2), 0)); // Bottom wall
                instances.Add(SpawnWall(new Vector3(xPos, 0, totalLength / 2), 180)); // Top wall
            }

            for (int i = 1; i < RoomLength; i++)
            {
                float zPos = -totalLength / 2 + i * wallWidth;
                instances.Add(SpawnWall(new Vector3(-totalWidth / 2, 0, zPos), 90)); // Left wall
                instances.Add(SpawnWall(new Vector3(totalWidth / 2, 0, zPos), 270)); // Right wall
            }

            // Spawn floors
            bool hasCenterFloor = RoomWidth >= 2 && RoomLength >= 2; // Check if room is big enough for center floor
            Vector2Int centerFloorSize = new Vector2Int(2, 2); // Center floor is 2x2 tiles

            for (int x = 0; x < RoomWidth; x++)
            {
                for (int z = 0; z < RoomLength; z++)
                {
                    // Check if this tile is within the center floor area
                    bool isCenterFloorTile = hasCenterFloor &&
                                            x >= (RoomWidth - centerFloorSize.x) / 2 &&
                                            x < (RoomWidth + centerFloorSize.x) / 2 &&
                                            z >= (RoomLength - centerFloorSize.y) / 2 &&
                                            z < (RoomLength + centerFloorSize.y) / 2;

                    float xPos = -totalWidth / 2 + (x + 0.5f) * wallWidth;
                    float zPos = -totalLength / 2 + (z + 0.5f) * wallWidth;

                    if (isCenterFloorTile && x == (RoomWidth / 2) - 1 && z == (RoomLength / 2) - 1)
                    {
                        // Spawn the center floor piece
                        instances.Add(SpawnCenterFloor(new Vector3(xPos + wallWidth/2f, 0, zPos + wallWidth/2f)));
                    }
                    else if (!isCenterFloorTile)
                    {
                        // Spawn regular floor tiles
                        instances.Add(SpawnFloor(new Vector3(xPos, 0, zPos)));
                    }
                }
            }
        }
        
        private int GetRandomOddNumber(int min, int max)
        {
            // Ensure the range is valid
            if (min > max)
            {
                Debug.LogError("Invalid range: min must be less than or equal to max.");
                return min;
            }

            // Generate a random number within the range
            int randomNumber = Random.Range(min, max + 1);

            // If the number is even, add 1 to make it odd
            if (randomNumber % 2 == 0)
            {
                randomNumber += 1;

                // Ensure the number stays within the range
                if (randomNumber > max)
                {
                    randomNumber -= 2; // Subtract 2 to stay within the range
                }
            }

            return randomNumber;
        }

        private GameObject SpawnWall(Vector3 position, float rotationY)
        {
            if (wallPrefabs.prefabs.Count == 0) return null;

            // Select a weighted wall prefab
            GameObject wallPrefab = GetWeightedPrefab(wallPrefabs);

            // Instantiate the wall and set its position and rotation
            GameObject wall = Instantiate(wallPrefab, position, Quaternion.Euler(0, rotationY, 0));
            wall.transform.parent = this.transform;
            return wall;
        }

        private GameObject SpawnCorner(Vector3 position, float rotationY)
        {
            if (cornerPrefabs.prefabs.Count == 0) return null;

            // Select a weighted corner prefab
            GameObject cornerPrefab = GetWeightedPrefab(cornerPrefabs);

            // Instantiate the corner and set its position and rotation
            GameObject corner = Instantiate(cornerPrefab, position, Quaternion.Euler(0, rotationY, 0));
            corner.transform.parent = this.transform;
            return corner;
        }

        private GameObject SpawnFloor(Vector3 position)
        {
            if (floorPrefabs.prefabs.Count == 0) return null;

            // Select a weighted floor prefab
            GameObject floorPrefab = GetWeightedPrefab(floorPrefabs);

            // Instantiate the floor and set its position
            GameObject floor = Instantiate(floorPrefab, position, Quaternion.identity);
            floor.transform.parent = this.transform;
            return floor;
        }

        private GameObject SpawnCenterFloor(Vector3 position)
        {
            if (centerFloorPrefab == null) return null;

            // Instantiate the center floor piece and set its position
            GameObject centerFloor = Instantiate(centerFloorPrefab, position, Quaternion.identity);
            centerFloor.transform.parent = this.transform;
            return centerFloor;
        }

        public static GameObject GetWeightedPrefab(WeightedPrefabRepository weightedPrefabs)
        {
            // Calculate the total weight
            float totalWeight = 0;
            foreach (var weightedPrefab in weightedPrefabs.prefabs)
            {
                totalWeight += weightedPrefab.weight;
            }

            // Randomly select a value within the total weight
            float randomValue = Random.Range(0, totalWeight);

            // Find the prefab corresponding to the random value
            foreach (var weightedPrefab in weightedPrefabs.prefabs)
            {
                if (randomValue < weightedPrefab.weight)
                {
                    return weightedPrefab.prefab;
                }
                randomValue -= weightedPrefab.weight;
            }

            // Fallback to the first prefab if something goes wrong
            return weightedPrefabs.prefabs[0].prefab;
        }
    }
}