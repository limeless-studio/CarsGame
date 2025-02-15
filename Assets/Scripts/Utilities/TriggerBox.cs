using System.Collections.Generic;
using UnityEngine;

namespace Snowy.Utils
{
    public class TriggerBox : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private LayerMask layerMask;
        
        [Title("Debug")]
        [SerializeField, Disable] private bool isTriggered;
        [SerializeField, ReorderableList, Disable] private List<Collider> colliders = new List<Collider>();
        
        public bool IsTriggered => isTriggered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                colliders.Add(other);
                isTriggered = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                colliders.Remove(other);
                isTriggered = colliders.Count > 0;
            }
        }
    }
}