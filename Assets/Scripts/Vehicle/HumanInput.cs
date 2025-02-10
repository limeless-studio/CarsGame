using System;
using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Vehicle
{
    public class HumanInput : MonoBehaviour
    {
        [SerializeField] BaseVehicle vehicle;
        [SerializeField] private HoldableButton leftButton;
        [SerializeField] private HoldableButton rightButton;
        
        private bool started = false;

        private void Update()
        {
            if (!started) return;
            
            vehicle.SetDriveInput(1f);
            
            if (leftButton.IsHoldingButton)
            {
                vehicle.SetSteerInput(-1);
            }
            else if (rightButton.IsHoldingButton)
            {
                vehicle.SetSteerInput(1);
            }
            else
            {
                vehicle.SetSteerInput(0);
            }
        }
        
        public void StartDriving()
        {
            started = true;
        }

        public void StopDriving()
        {
            started = false;
            vehicle.SetDriveInput(0);
            vehicle.SetSteerInput(0);
        }
    }
}