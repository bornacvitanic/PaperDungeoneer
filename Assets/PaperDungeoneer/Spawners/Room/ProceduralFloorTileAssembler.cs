using PaperDungeoneer.Spawners.Data;
using UnityEngine;

namespace PaperDungeoneer.Spawners.Room
{
    public class ProceduralFloorTileAssembler : MonoBehaviour
    {
        [Header("Half-Sized Floor Tiles")]
        public WeightedPrefabRepository halfFloorPrefabs; // Repository of half-sized floor tiles

        [Header("Tile Dimensions")]
        public float tileSize = 1.0f; // Size of the full floor tile (width and length)

        private void Awake()
        {
            AssembleProceduralFloorTile();
        }

        private void AssembleProceduralFloorTile()
        {
            if (halfFloorPrefabs == null || halfFloorPrefabs.prefabs.Count == 0)
            {
                Debug.LogError("Half-sized floor prefabs repository is not assigned or is empty.");
                return;
            }

            // Calculate the offset for half-sized tiles
            float halfTileSize = tileSize / 2f;

            // Spawn 4 half-sized tiles in a 2x2 grid
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    // Calculate the position for each half-sized tile
                    Vector3 position = new Vector3(
                        transform.position.x - halfTileSize / 2f + x * halfTileSize,
                        transform.position.y,
                        transform.position.z - halfTileSize / 2f + y * halfTileSize
                    );

                    // Select a random half-sized floor tile from the repository
                    GameObject halfFloorPrefab = RoomSpawner.GetWeightedPrefab(halfFloorPrefabs);

                    // Instantiate the half-sized tile and set its position and parent
                    GameObject halfFloorTile = Instantiate(halfFloorPrefab, position, Quaternion.Euler(0,Random.Range(0,4)*90f,0));
                    halfFloorTile.transform.parent = this.transform;
                }
            }
        }
    }
}