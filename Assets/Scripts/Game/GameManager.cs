using System;
using System.Collections;
using Snowy.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vehicle;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Title("References")]
        [SerializeField] private GameObject coinPrefab;
        [SerializeField, ReorderableList] private GameObject[] obstaclesPrefabs;
        [SerializeField] private HumanInput player;
        
        [Title("Settings")]
        [SerializeField] private float coinSpawnChance = 0.5f;
        [SerializeField] private float obstacleSpawnChance = 0.5f;
        
        [Title("UI")]
        [SerializeField] private Text countdownText;
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private CanvasGroup gameOverCanvasGroup;
        [SerializeField] private Text scoreText;
        [SerializeField] private TMP_Text coinsCollectedText;
        
        [Title("Debug")]
        [SerializeField, Disable] private int coinsCollected = 0;

        private void Start()
        {
            StartCountdown();
        }
        
        private void StartCountdown()
        {
            StartCoroutine(CountdownRoutine());
        }
        
        private System.Collections.IEnumerator CountdownRoutine()
        {
            // fade in
            while (fadeCanvasGroup.alpha > 0)
            {
                fadeCanvasGroup.alpha -= Time.deltaTime;
                yield return null;
            }
            
            yield return new WaitForSeconds(1f);
            countdownText.text = "3";
            yield return new WaitForSeconds(1f);
            countdownText.text = "2";
            yield return new WaitForSeconds(1f);
            countdownText.text = "1";
            yield return new WaitForSeconds(1f);
            countdownText.text = "GO!";
            yield return new WaitForSeconds(1f);
            countdownText.gameObject.SetActive(false);
            
            player.StartDriving();
        }

        public void SpawnCoin(Transform spawnPoint)
        {
            if (Random.value < coinSpawnChance)
            {
                Instantiate(coinPrefab, spawnPoint);
            }
        }
        
        public void SpawnObstacle(Transform spawnPoint)
        {
            if (Random.value < obstacleSpawnChance)
            {
                Instantiate(obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)], spawnPoint);
            }
        }

        public void CollectCoin()
        {
            coinsCollected++;
            coinsCollectedText.text = coinsCollected.ToString();
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }
        
        private IEnumerator GameOverRoutine()
        {
            // disable player input
            player.StopDriving();
            
            // smooth fade out
            while (gameOverCanvasGroup.alpha < 1)
            {
                gameOverCanvasGroup.alpha += Time.deltaTime;
                yield return null;
            }
            
            // show game over screen
            gameOverCanvasGroup.alpha = 1;
            scoreText.text = coinsCollected.ToString();
        }
    }
}