using System;
using System.Collections;
using System.Globalization;
using Game.Cars;
using Snowy.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] protected int gameID = 0;
        [SerializeField] protected int countdownDuration = 3;

        [Title("Events")]
        public UnityEvent<int> OnCountdownEvent;
        public UnityEvent<int> OnCoinCollected;
        public UnityEvent OnObstacleHit;
        public UnityEvent OnGameStartEvent;
        public UnityEvent OnGameOverEvent;

        
        [Title("Debug")]
        [SerializeField, Disable] protected int score = 0;

        public int GameID => gameID;
        public int Score => score;
        
        protected void Start()
        {
            StartCoroutine(StartGame());
        }
        
        public void GameOver()
        {
            player.EndGame();
            OnGameOverEvent?.Invoke();
        }
        
        protected IEnumerator StartGame()
        {
            yield return new WaitForSeconds(.5f);
            
            for (int i = 0; i < countdownDuration; i++)
            {
                OnCountdownEvent?.Invoke(countdownDuration - i);
                yield return new WaitForSeconds(1);
            }

            OnCountdownEvent?.Invoke(0);
            
            OnGameStart();
        }
        
        public virtual void CollectCoin(GameObject coin)
        {
            score++;
            OnCoinCollected?.Invoke(score);
        }
        
        public virtual void HitObstacle(GameObject obstacle)
        {
            OnObstacleHit?.Invoke();
            GameOver();
        }
        
        protected virtual void OnGameStart()
        {
            player.StartGame();
            OnGameStartEvent?.Invoke();
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