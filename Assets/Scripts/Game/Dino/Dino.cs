using System;
using Snowy.Utils;
using UnityEngine;

namespace Game.Dino
{
    public class Dino : Player
    {
        [Title("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TriggerBox groundTrigger;
        [SerializeField] private Animator animator;
        
        [Title("Settings")]
        [SerializeField] private float jumpForce = 1f;
        [SerializeField] private float downForce = 1f;

        private void FixedUpdate()
        {
            if (rb.isKinematic) return;
            
            if (!groundTrigger.IsTriggered)
                rb.linearVelocity += new Vector3(0, -downForce, 0);
        }

        private void Update()
        {
            Animating();
        }

        public void AttemptJump()
        {
            if (rb.isKinematic) return;
            
            // Check if the player is grounded
            if (groundTrigger.IsTriggered)
                ApplyJump();
        }

        private void ApplyJump()
        {
            rb.linearVelocity = new Vector3(0, jumpForce, 0);
            animator.SetTrigger("Jump");
        }

        public override void StartGame()
        {
            base.StartGame();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        private void Animating()
        {
            animator.SetBool("IsGrounded", groundTrigger.IsTriggered);
        }
    }
}