using UnityEngine;

namespace Game.Bird
{
    public class Bird : Player
    {
        [Title("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        
        [Title("Settings")]
        [SerializeField] private float flapForce = 1f;
        [SerializeField] private float downForce = 1f;
        
        private void FixedUpdate()
        {
            if (rb.isKinematic) return;
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y - downForce, 0);
            
        }
        
        public void Flap()
        {
            if (rb.isKinematic) return;
            animator.Play("Flap");
            Invoke(nameof(FlapForce), .1f);
        }

        private void FlapForce()
        {
            rb.linearVelocity = new Vector3(0, flapForce, 0);
        }

        public override void StartGame()
        {
            base.StartGame();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        
        public override void EndGame()
        {
            base.EndGame();
            animator.CrossFade("Death", .1f);
        }
    }
}