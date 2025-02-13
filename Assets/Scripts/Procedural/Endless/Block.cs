using UnityEngine;

namespace DefaultNamespace
{
    public class Block : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private Transform connectionPoint;
        
        public Transform ConnectionPoint => connectionPoint;
        
        public virtual void Init()
        {
        }
        
        private void OnTriggerEnter(Collider other)
        {
            EndlessManager.Instance.OnEnterBlock(this, other);
        }
    }
}