using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class ClawGameManager : GameManager
    {
        [Title("Claw Game", Order = 0)]
        [SerializeField] private int timer = 60;
        [SerializeField] private UnityEvent<int> onTimerUpdate;
        
        private float m_timer;
        
        protected override void OnGameStart()
        {
            base.OnGameStart();
            
            m_timer = timer;
            InvokeRepeating(nameof(UpdateTimer), 1, 1);
        }
        
        protected override void GameOver()
        {
            base.GameOver();
            
            CancelInvoke(nameof(UpdateTimer));
        }
        
        private void UpdateTimer()
        {
            m_timer--;
            
            if (m_timer <= 0)
            {
                HitObstacle(gameObject);
            }
            
            onTimerUpdate?.Invoke((int) m_timer);
        }
    }
}