using UnityEngine;

namespace Game.Cars
{
    public class CarGameManager : GameManager
    {
        public static CarGameManager Instance => (CarGameManager) instance;
        
        [Title("References")]
        [SerializeField] private GameObject coinPrefab;
        [SerializeField, ReorderableList] private GameObject[] obstaclesPrefabs;

        public override void CollectCoin(GameObject coin)
        {
            base.CollectCoin(coin);
            
            Destroy(coin);
        }

        public void SpawnCoin(Transform spawnPoint)
        {
            GameObject obj = Instantiate(coinPrefab, spawnPoint);
            obj.transform.localPosition = coinPrefab.transform.position;
        }
        
        public void SpawnObstacle(Transform spawnPoint)
        {
            GameObject prefab = obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];
            GameObject obj = Instantiate(prefab, spawnPoint);
            obj.transform.localPosition = prefab.transform.position;
        }

        protected override void OnGameStart()
        {
            base.OnGameStart();
        }
    }
}