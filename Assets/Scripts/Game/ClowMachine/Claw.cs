using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Claw : Player
    {
        [Title("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform firstClaw;
        [SerializeField] private Transform secondClaw;
        [SerializeField] private Transform thirdClaw;
        
        [SerializeField] private Vector3 firstClawClosedRot;
        [SerializeField] private Vector3 secondClawClosedRot;
        [SerializeField] private Vector3 thirdClawClosedRot;
        
        [SerializeField] private Vector3 firstClawOpenRot;
        [SerializeField] private Vector3 secondClawOpenRot;
        [SerializeField] private Vector3 thirdClawOpenRot;
        
        [Title("Settings")]
        [SerializeField] private float speed = 1f;

        void Start()
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            
            // Hide cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            // Open claw
            OpenClaw();
        }
        
        void Update()
        {
            if (!GameManager.Instance.IsGameRunning) return;
            
            
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            Move();
            
            if (Input.GetMouseButtonDown(0))
                CloseClaw();
            else if (Input.GetMouseButtonUp(0))
                OpenClaw();
        }
        
        void Move()
        {
            // Get mouse movement
            float moveX = Input.GetAxis("Mouse X");
            float moveY = Input.GetAxis("Mouse Y");
            
            // Move claw accourding to mouse movement, accounting to the collisions
            rb.MovePosition(rb.position + new Vector3(moveX, moveY, 0) * (speed * Time.deltaTime));
        }
        
        void CloseClaw()
        {
            firstClaw.DOLocalRotate(firstClawClosedRot, 0.2f);
            secondClaw.DOLocalRotate(secondClawClosedRot, 0.2f);
            thirdClaw.DOLocalRotate(thirdClawClosedRot, 0.2f);
        }
        
        void OpenClaw()
        {
            firstClaw.DOLocalRotate(firstClawOpenRot, 0.2f);
            secondClaw.DOLocalRotate(secondClawOpenRot, 0.2f);
            thirdClaw.DOLocalRotate(thirdClawOpenRot, 0.2f);
        }

        public override void StartGame()
        {
            base.StartGame();
            
            rb.isKinematic = false;
            rb.useGravity = false;
        }
        
        public override void EndGame()
        {
            base.EndGame();
            
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        protected override void OnCollisionEnter(Collision other)
        {
            // IGNORE
        }
        
        protected override void OnTriggerEnter(Collider other)
        {
            // IGNORE
        }
    }
}