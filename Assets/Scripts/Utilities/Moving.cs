using UnityEngine;

namespace Snowy.Utils
{
    public class Moving : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private float speed = 1f;
        [SerializeField] private Vector3 direction = Vector3.forward;
        [SerializeField] private bool canMove = true;

        private void Update()
        {
            if (canMove)
                transform.position += direction * (speed * Time.deltaTime);
        }

        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        public void SetDirection(Vector3 newDirection)
        {
            direction = newDirection;
        }

        public void SetCanMove(bool newCanMove)
        {
            canMove = newCanMove;
        }
    }
}