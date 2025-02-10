using UnityEngine;

namespace Vehicle
{
    public class WheelVisuals : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private BaseVehicle carController;
        [SerializeField] TrailRenderer skidMark;
        [SerializeField] ParticleSystem smoke;
        
        private float fadeOutSpeed;
        
        private void Awake()
        {
            if (!carController) carController = GetComponentInParent<BaseVehicle>();
            smoke = GetComponent<ParticleSystem>();
            skidMark = GetComponent<TrailRenderer>();
            skidMark.emitting = false;
            skidMark.startWidth = carController.skidWidth;
        }


        private void OnEnable()
        {
            skidMark.enabled = true;
        }
        private void OnDisable()
        {
            skidMark.enabled = false;
        }

        void FixedUpdate()
        {
            if (carController.IsGrounded())
            {

                if (Mathf.Abs(carController.carVelocity.x) > 5)
                {
                    fadeOutSpeed = 0f;
                    skidMark.materials[0].color = Color.black;
                    skidMark.emitting = true;
                }
                else
                {
                    skidMark.emitting = false;
                }
            }
            else
            {
                skidMark.emitting = false;

            }
            if (!skidMark.emitting)
            {
                fadeOutSpeed += Time.deltaTime / 2;
                Color m_color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), fadeOutSpeed);
                skidMark.materials[0].color = m_color;
                if (fadeOutSpeed > 1)
                {
                    skidMark.Clear();
                }
            }

            // smoke
            if (skidMark.emitting == true)
            {
                smoke.Play();
            }
            else { smoke.Stop(); }

        }
    }
}