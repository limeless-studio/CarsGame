using UnityEngine;

namespace Vehicle
{
    public class AICar : MonoBehaviour
    {
        [SerializeField] private BaseVehicle baseVehicle;
        
        void Start()
        {
            if (!baseVehicle) baseVehicle = GetComponent<BaseVehicle>();
        }
        
        void Update()
        {
            baseVehicle.SetDriveInput(1f);
        }
    }
}