using Snowy.Utils;
using UnityEngine;

namespace Game.Dino
{
    public class DinoGameManager: GameManager
    {
        public new static DinoGameManager Instance => (DinoGameManager) instance;
        
        [Title("References")]
        [SerializeField] private Moving movingGround;

        protected override void OnGameStart()
        {
            base.OnGameStart();
            
            InvokeRepeating(nameof(AddPoint), 1, 1);
        }
        
        protected override void GameOver()
        {
            base.GameOver();
            movingGround.SetCanMove(false);
            
            CancelInvoke(nameof(AddPoint));
        }

        private void AddPoint()
        {
            CollectCoin(player.gameObject);
        }
    }
}