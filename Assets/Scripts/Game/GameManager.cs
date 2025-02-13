using System;
using System.Collections;
using System.Globalization;
using Game.Cars;
using Snowy.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vehicle;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Title("References")]
        [SerializeField] protected Player player;
        
        [Title("Settings")]
        [SerializeField] protected float countdownDuration = 3f;
        
        [Title("UI")]
        [SerializeField] protected Text countdownText;
        [SerializeField] protected GameObject controls;
        [SerializeField] protected CanvasGroup fadeCanvasGroup;
        [SerializeField] protected CanvasGroup gameOverCanvasGroup;
        [SerializeField] protected Text scoreText;
        [SerializeField] protected TMP_Text coinsCollectedText;
        
        [Title("Debug")]
        [SerializeField, Disable] protected int coinsCollected = 0;
        
        protected void Start()
        {
            StartCoroutine(StartGame());
        }
        
        public void GameOver()
        {
            controls.SetActive(false);
            player.EndGame();
            StartCoroutine(GameOverRoutine());
        }
        
        protected IEnumerator StartGame()
        {
            yield return new WaitForSeconds(.5f);
            
            for (int i = 0; i < countdownDuration; i++)
            {
                countdownText.text = (countdownDuration - i).ToString(CultureInfo.InvariantCulture);
                yield return new WaitForSeconds(1);
            }
            
            countdownText.text = "GO!";
            
            controls.SetActive(true);
            
            OnGameStart();
        }
        
        protected IEnumerator GameOverRoutine()
        {
            scoreText.text = coinsCollected.ToString();
            
            // smooth fade out
            while (gameOverCanvasGroup.alpha < 1)
            {
                gameOverCanvasGroup.alpha += Time.deltaTime;
                yield return null;
            }
            
            // show game over screen
            gameOverCanvasGroup.alpha = 1;
        }
        
        public virtual void CollectCoin(GameObject coin)
        {
            coinsCollected++;
            coinsCollectedText.text = coinsCollected.ToString();
        }
        
        public virtual void HitObstacle(GameObject obstacle)
        {
            GameOver();
        }
        
        protected virtual void OnGameStart()
        {
            player.StartGame();
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}