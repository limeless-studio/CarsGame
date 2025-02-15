using UnityEngine;

namespace DefaultNamespace
{
    /// <summary>
    /// Flappy bird-like tube spawner
    /// </summary>
    public class TubeBlock : Block
    {
        [SerializeField, ReorderableList] private Transform[] spawnpoints;
        [Title("Settings")]
        [SerializeField] private GameObject tubePrefab;
        [SerializeField] private GameObject pointArea;
        [SerializeField] private float tubeDistance = 2f;
        [SerializeField] private Vector2 tubeHeightRange = new Vector2(-1f, 1f);
        
        public override void Init()
        {
            SpawnTubes();
        }
        
        private void SpawnTubes()
        {
            foreach (Transform spawnpoint in spawnpoints)
            {
                // Spawn two tubes one above and one below the spawnpoint with a distance of tubeDistance
                Vector3 spawnpointPosition = spawnpoint.position + Vector3.up * Random.Range(tubeHeightRange.x, tubeHeightRange.y);

                // Point Area
                var area = Instantiate(pointArea, spawnpointPosition, Quaternion.identity, transform);
                area.transform.localScale = new Vector3(1f, tubeDistance * 2, 1f);

                // Tubes
                Vector3 tubePosition = spawnpointPosition + Vector3.up * tubeDistance;
                Instantiate(tubePrefab, tubePosition, Quaternion.Euler(0, 0, 180), transform);
                tubePosition = spawnpointPosition + Vector3.down * tubeDistance;
                Instantiate(tubePrefab, tubePosition, Quaternion.identity, transform);
            }
        }
    }
}