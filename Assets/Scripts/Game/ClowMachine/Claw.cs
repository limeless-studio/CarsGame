using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Claw : Player
    {
        [Title("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform rightClaw;
        [SerializeField] private Transform leftClaw;
        
        [Title("Settings")]
        [SerializeField] private float speed = 1f;
        [SerializeField] private float clawCloseAngle = 0f;
        [SerializeField] private float clawOpenAngle = 90f;

        void Start()
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            
            // Hide cursor
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        void Update()
        {
            if (!GameManager.Instance.IsGameRunning) return;
            
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
            
            rb.linearVelocity = Vector3.zero;
            
            // Move claw accourding to mouse movement, accounting to the collisions
            rb.MovePosition(rb.position + new Vector3(moveX, moveY, 0) * (speed * Time.deltaTime));
        }
        
        void CloseClaw()
        {
            rightClaw.DOLocalRotate(new Vector3(0, 0, clawCloseAngle), 0.2f);
            leftClaw.DOLocalRotate(new Vector3(0, 0, -clawCloseAngle), 0.2f);
        }
        
        void OpenClaw()
        {
            rightClaw.DOLocalRotate(new Vector3(0, 0, clawOpenAngle), 0.2f);
            leftClaw.DOLocalRotate(new Vector3(0, 0, -clawOpenAngle), 0.2f);
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
    }
}