using System.Collections;
using Snowy.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
        [SerializeField, Disable] protected bool isDead = false;
        [SerializeField, Disable] protected bool isGameRunning = false;

        public int GameID => gameID;
        public int Score => score;
        public bool IsGameRunning => isGameRunning;

        protected void Start()
        {
            StartCoroutine(StartGame());
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
            if (isDead) return;
            
            score++;
            OnCoinCollected?.Invoke(score);
        }
        
        public virtual void HitObstacle(GameObject obstacle)
        {
            if (isDead) return;
            
            OnObstacleHit?.Invoke();
            GameOver();
        }
        
        protected virtual void OnGameStart()
        {
            isGameRunning = true;
            player.StartGame();
            OnGameStartEvent?.Invoke();
        }
        
        protected virtual void GameOver()
        {
            if (isDead) return;
            player.EndGame();
            isDead = true;
            isGameRunning = false;
            OnGameOverEvent?.Invoke();
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