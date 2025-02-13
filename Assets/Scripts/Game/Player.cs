using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public virtual void StartGame()
        {
            
        }
        
        public virtual void EndGame()
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                GameManager.Instance.CollectCoin(other.gameObject);
            }
            
            if (other.gameObject.CompareTag("Obstacle"))
            {
                GameManager.Instance.HitObstacle(other.gameObject);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                GameManager.Instance.CollectCoin(other.gameObject);
            }
            
            if (other.gameObject.CompareTag("Obstacle"))
            {
                GameManager.Instance.HitObstacle(other.gameObject);
            }
        }
    }
}