using UnityEngine;

namespace Snowy.Utils
{
    public class Spawner : MonoBehaviour
    {
        [Title("References")]
        [SerializeField, ReorderableList] private Transform[] spawnpoints;
        [SerializeField, ReorderableList] private GameObject[] prefabs;
        
        [Title("Settings")]
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private float spawnChance = 0.5f;
        [SerializeField, MinMaxSlider(0, 100)] private Vector2Int spawnCount = new Vector2Int(1, 3);
        
        [Title("Debug")]
        [SerializeField, Disable] private int maxSpawnCount = 0;
        [SerializeField, Disable] private int spawnedCount = 0;

        void Start()
        {
            maxSpawnCount = Random.Range(spawnCount.x, spawnCount.y);
            InvokeRepeating(nameof(Spawn), 0, spawnInterval);
        }
       
        
        private void Spawn()
        {
            if (spawnedCount >= maxSpawnCount)
            {
                CancelInvoke(nameof(Spawn));
                return;
            }

            foreach (Transform spawnpoint in spawnpoints)
            {
                if (Random.value < spawnChance)
                {
                    GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                    Instantiate(prefab, spawnpoint.position, Quaternion.identity, transform);
                    spawnedCount++;
                }
            }
        }
    }
}