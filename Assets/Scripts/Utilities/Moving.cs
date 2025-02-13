using UnityEngine;

namespace Snowy.Utils
{
    public class Moving : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private float speed = 1f;
        [SerializeField] private Vector3 direction = Vector3.forward;
        
        private void Update()
        {
            transform.position += direction * (speed * Time.deltaTime);
        }
    }
}