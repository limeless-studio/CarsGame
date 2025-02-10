using System;
using Game;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoadBlock : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Transform connectionPoint;
        [SerializeField] private float obstacleSpawnChance = 0.5f;
        [SerializeField] private float coinSpawnChance = 0.5f;
        [SerializeField, ReorderableList] private Transform[] spawnpoints;
        
        public Transform ConnectionPoint => connectionPoint;

        public void Init()
        {
            SpawnItems();
        }
        

        private void SpawnItems()
        {
            foreach (Transform spawnpoint in spawnpoints)
            {
                // Select either coin or obstacle, or nothing
                float randomValue = UnityEngine.Random.value;
                if (randomValue < coinSpawnChance)
                {
                    GameManager.Instance.SpawnCoin(spawnpoint);
                }
                else if (randomValue < obstacleSpawnChance)
                {
                    // Spawn obstacle
                    GameManager.Instance.SpawnObstacle(spawnpoint);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            RoadManager.Instance.OnEnterRoadBlock(this, other);
        }
    }
}