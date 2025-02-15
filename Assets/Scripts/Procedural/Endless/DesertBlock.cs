using Game.Cars;
using UnityEngine;

namespace DefaultNamespace
{
    public class DesertBlock : Block
    {
        [SerializeField, ReorderableList] private GameObject[] obstacles;
        [SerializeField, ReorderableList] private Transform[] spawnpoints;
        [Title("Settings")]
        [SerializeField] private float obstacleSpawnChance = 0.5f;

        public override void Init()
        {
            SpawnItems();
        }
        
        private void SpawnItems()
        { 
            foreach (Transform spawnpoint in spawnpoints)
            {
                if (Random.value < obstacleSpawnChance)
                {
                    GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];
                    Instantiate(obstacle, spawnpoint.position, Quaternion.identity, transform);
                }
            }
        }
    }
}