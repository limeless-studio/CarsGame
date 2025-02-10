using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.UI
{
    public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnClicked;
        public event Action<bool> OnHoldClicked;

        private bool m_isHoldingButton;
        
        public bool IsHoldingButton => m_isHoldingButton;

        public void OnPointerDown(PointerEventData eventData) => ToggleHoldingButton(true);

        private void ToggleHoldingButton(bool isPointerDown)
        {
            m_isHoldingButton = isPointerDown;
            
            OnHoldClicked?.Invoke(isPointerDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ManageButtonInteraction(true);
            ToggleHoldingButton(false);
        }

        private void ManageButtonInteraction(bool isPointerUp = false)
        {
            if (!m_isHoldingButton)
                return;

            if (isPointerUp)
            {
                Click();
                return;
            }
        }

        private void Click()
        {
            OnClicked?.Invoke();
        }

        private void HoldClick()
        {
            ToggleHoldingButton(false);
        }

        private void Update() => ManageButtonInteraction();
    }
}