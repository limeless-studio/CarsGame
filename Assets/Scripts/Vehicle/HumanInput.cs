using System;
using DefaultNamespace.UI;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace Vehicle
{
    public class HumanInput : Player
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
        
        public override void StartGame()
        {
            started = true;
        }

        public override void EndGame()
        {
            started = false;
            vehicle.SetDriveInput(0);
            vehicle.SetSteerInput(0);
        }
    }
}