using Game.Cars;
using UnityEngine;

namespace DefaultNamespace
{
    public class RoadBlock : Block
    {
        [SerializeField, ReorderableList] private Transform[] spawnpoints;
        [Title("Settings")]
        [SerializeField] private float obstacleSpawnChance = 0.5f;
        [SerializeField] private float coinSpawnChance = 0.5f;

        public override void Init()
        {
            SpawnItems();
        }
        
        private void SpawnItems()
        {
            if (CarGameManager.Instance == null)
            {
                Debug.LogError("CarGameManager is missing in the scene!");
                return;
            }
            
            foreach (Transform spawnpoint in spawnpoints)
            {
                // Select either coin or obstacle, or nothing
                float randomValue = UnityEngine.Random.value;
                if (randomValue < coinSpawnChance)
                {
                    CarGameManager.Instance.SpawnCoin(spawnpoint);
                }
                else if (randomValue < obstacleSpawnChance)
                {
                    // Spawn obstacle
                    CarGameManager.Instance.SpawnObstacle(spawnpoint);
                }
            }
        }
    }
}