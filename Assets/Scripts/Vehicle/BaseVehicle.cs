using System;
using System.Linq;
using UnityEngine;

namespace Vehicle
{
    public enum GroundCheck
    {
        RayCast,
        SphereCaste
    };

    public enum MovementMode
    {
        Velocity,
        AngularVelocity
    };

    [Serializable]
    public struct VisualWheel
    {
        public Transform wheel;
        public MeshRenderer mesh;
        public bool canSteer;
    }

    public class BaseVehicle : MonoBehaviour
    {
        #region Serialized Fields

        [Title("References")]
        public Rigidbody rb;
        public Rigidbody carBody;

        [Title("Settings")] 
        [SerializeField] protected MovementMode movementMode;
        [SerializeField] protected GroundCheck groundCheck;
        [SerializeField] protected LayerMask drivableSurface;
        [SerializeField] protected float maxSpeed;
        [SerializeField] protected float accelaration;
        [SerializeField] protected float turn;
        [SerializeField] protected float downforce = 5f;
        [SerializeField] protected float jumpForce = 1000f;

        [Tooltip("if true : can turn vehicle in air")] 
        [SerializeField] protected bool airControl = false;

        [Tooltip("if true : vehicle will drift instead of brake while holding space")]
        [SerializeField] protected bool kartLike = false;

        [Tooltip("turn more while drifting (while holding space) only if kart Like is true")] 
        [SerializeField] protected float driftMultiplier = 1.5f;

        [SerializeField] protected AnimationCurve frictionCurve;
        [SerializeField] protected AnimationCurve turnCurve;
        [SerializeField] protected PhysicsMaterial frictionMaterial;

        [Title("Visuals")] 
        [SerializeField] protected Transform bodyMesh;
        [SerializeField, ReorderableList] protected VisualWheel[] wheels;
        [Range(0, 10)] [SerializeField] protected float bodyTilt;
        
        [Title("Audio")]
        [SerializeField] protected AudioSource engineSound;
        [SerializeField, Range(0, 1)] protected float minPitch;
        [SerializeField, Range(1, 3)] protected float maxPitch;
        [SerializeField] protected AudioSource skidSound;
        
        #endregion

        #region Protected Fields
        // Private Fields

        [Title("Debug")]
        [Disable, SerializeField]
        public Vector3 carVelocity;
        [Disable, SerializeField] public float skidWidth;
        [Disable, SerializeField] protected float radius;
        [Disable, SerializeField] protected float horizontalInput;
        [Disable, SerializeField] protected float gasInput;
        [Disable, SerializeField] protected float brakeInput;
        [Disable, SerializeField] protected float driftInput;
        [Disable, SerializeField] protected bool isGrounded;
        [Disable, SerializeField] protected Vector3 origin;
        [Disable, SerializeField] protected SphereCollider sphereCollider;

        [Disable, SerializeField] Transform[] frontWheels;
        [Disable, SerializeField] Transform[] rearWheels;
        [Disable, SerializeField] protected RaycastHit hit;
        
        #endregion
        
        protected virtual void Start()
        {
            sphereCollider = rb.GetComponent<SphereCollider>();
            radius = sphereCollider.radius;

            if (movementMode == MovementMode.AngularVelocity)
            {
                Physics.defaultMaxAngularSpeed = 100;
            }
            
            frontWheels = wheels.Where(wheel => wheel.canSteer).Select(wheel => wheel.wheel).ToArray();
            rearWheels = wheels.Where(wheel => !wheel.canSteer).Select(wheel => wheel.wheel).ToArray();
        }

        protected virtual void Update()
        {
            isGrounded = IsGrounded();
            Visuals();
            AudioManager();
        }

        protected virtual void FixedUpdate()
        {
            float verticalInput = gasInput - brakeInput;
            Vector3 vel = rb.linearVelocity;
            carVelocity = carBody.transform.InverseTransformDirection(carBody.linearVelocity);

            // Adjust friction according to sideways speed of car
            float carVelocityXAbs = Mathf.Abs(carVelocity.x);
            if (carVelocityXAbs > 0)
            {
                frictionMaterial.dynamicFriction = frictionCurve.Evaluate(carVelocityXAbs / 100f);
            }

            if (isGrounded)
            {
                // Turn logic
                float sign = Mathf.Sign(carVelocity.z);
                float turnMultiplier = turnCurve.Evaluate(carVelocity.magnitude / maxSpeed);
                if (kartLike && driftInput > 0.1f)
                {
                    // Turn more if drifting
                    turnMultiplier *= driftMultiplier;
                }

                if (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(carVelocity.z) > 1)
                {
                    carBody.AddTorque(Vector3.up * horizontalInput * sign * turn * 100f * turnMultiplier);
                }


                // Normal brake logic
                if (!kartLike)
                {
                    rb.constraints = (driftInput > 0.1f) ? RigidbodyConstraints.FreezeRotationX : RigidbodyConstraints.None;
                }

                // Acceleration logic
                if (movementMode == MovementMode.AngularVelocity)
                {
                    if (Mathf.Abs(verticalInput) > 0.1f && (!kartLike && driftInput < 0.1f || kartLike))
                    {
                        Vector3 targetAngularVelocity = carBody.transform.right * verticalInput * maxSpeed / radius;
                        rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, targetAngularVelocity, accelaration * Time.deltaTime);
                    }
                }
                else if (movementMode == MovementMode.Velocity)
                {
                    Vector3 projectedForward = Vector3.ProjectOnPlane(carBody.transform.forward, hit.normal);
                    Debug.DrawRay(transform.position, projectedForward, Color.red);

                    if (Mathf.Abs(verticalInput) > 0.1f && (!kartLike && driftInput < 0.1f || kartLike))
                    {
                        Vector3 targetVelocity = projectedForward * verticalInput * maxSpeed;
                        targetVelocity.y = rb.linearVelocity.y;
                        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, (accelaration / 10f) * Time.deltaTime);
                    }
                }

                // Downforce
                rb.AddForce(-transform.up * downforce * rb.mass);

                // Body tilt
                Quaternion targetRotation = Quaternion.FromToRotation(carBody.transform.up, hit.normal) * carBody.transform.rotation;
                carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, targetRotation, 0.12f));
            }
            else
            {
                //if (airControl)
                //{
                //    // Turn logic in air
                //    float turnMultiplier = turnCurve.Evaluate(carVelocity.magnitude / maxSpeed);
                //    carBody.AddTorque(Vector3.up * horizontalInput * turn * 100f * turnMultiplier);
                //}
//
                //// Stabilize car body in air
                //Quaternion targetRotation = Quaternion.FromToRotation(carBody.transform.up, Vector3.up) * carBody.transform.rotation;
                //carBody.MoveRotation(Quaternion.Slerp(carBody.rotation, targetRotation, 0.02f));
            }
        }
        
        
        protected virtual void AudioManager()
        {
            engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(carVelocity.z) / maxSpeed);
            skidSound.mute = (Mathf.Abs(carVelocity.x) <= 10 || !isGrounded);
        }

        protected virtual void Visuals()
        {
            // Tires
            Quaternion frontWheelRotation = Quaternion.Euler(frontWheels[0].localRotation.eulerAngles.x, 30f * horizontalInput, frontWheels[0].localRotation.eulerAngles.z);
            foreach (Transform FW in frontWheels)
            {
                FW.localRotation = Quaternion.Slerp(FW.localRotation, frontWheelRotation, 0.7f * Time.deltaTime / Time.fixedDeltaTime);
                FW.GetChild(0).localRotation = rb.transform.localRotation;
            }

            foreach (Transform RW in rearWheels)
            {
                RW.localRotation = rb.transform.localRotation;
            }

            // Body
            if (carVelocity.z > 1)
            {
                float tiltAngle = Mathf.Lerp(0, -5, carVelocity.z / maxSpeed);
                Quaternion bodyRotation = Quaternion.Euler(tiltAngle, bodyMesh.localRotation.eulerAngles.y, bodyTilt * horizontalInput);
                bodyMesh.localRotation = Quaternion.Slerp(bodyMesh.localRotation, bodyRotation, 0.1f * Time.deltaTime / Time.fixedDeltaTime);
            }
            else
            {
                bodyMesh.localRotation = Quaternion.Slerp(bodyMesh.localRotation, Quaternion.identity, 0.1f * Time.deltaTime / Time.fixedDeltaTime);
            }

            if (kartLike)
            {
                Quaternion parentRotation = (driftInput > 0.1f) ?
                    Quaternion.Euler(0, 45f * horizontalInput * Mathf.Sign(carVelocity.z), 0) :
                    Quaternion.identity;

                bodyMesh.parent.localRotation = Quaternion.Slerp(bodyMesh.parent.localRotation, parentRotation, 0.1f * Time.deltaTime / Time.fixedDeltaTime);
            }
        }
        
        // Checks if vehicle is grounded
        public virtual bool IsGrounded()
        {
            origin = rb.position + sphereCollider.radius * transform.up;
            var direction = -transform.up;
            var maxdistance = sphereCollider.radius + 0.2f;

            if (groundCheck == GroundCheck.RayCast)
            {
                return Physics.Raycast(rb.position, Vector3.down, out hit, maxdistance, drivableSurface);
            }

            if (groundCheck == GroundCheck.SphereCaste)
            {
                return Physics.SphereCast(origin, radius + 0.1f, direction, out hit, maxdistance, drivableSurface);
            }

            return false;
        }
        
        public virtual void Jump(float force = -1)
        {
            if (isGrounded)
            {
                if (Mathf.Approximately(force, -1)) force = jumpForce;
                AddForce(Vector3.up * force);
            }
        }
        
        public virtual void AddForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
        {
            rb.AddForce(force, mode);
        }
        
        public virtual void AddExternalForce(Vector3 force, Vector3 position, ForceMode mode = ForceMode.Impulse)
        {
            carBody.AddForceAtPosition(force, position, mode);
        }
        
        public virtual void AddExternalForce(Vector3 force, ForceMode mode = ForceMode.Impulse)
        {
            carBody.AddForce(force, mode);
        }
        
        [ContextMenu("Calculate Skid Width")]
        public void CalculateSkidWidth()
        {
            skidWidth = wheels.Max(wheel => (wheel.mesh.bounds.size.x / 2) * wheel.mesh.transform.lossyScale.x);
        }

        private void OnCollisionEnter(Collision other)
        {
            
        }

        # region Setters
        
        public void SetDriveInput(float input)
        {
            gasInput = Mathf.Clamp(input, -1f, 1f);
        }
        
        public void SetBackwardInput(float input)
        {
            brakeInput = Mathf.Clamp(input, -1f, 1f);
        }
        
        public void SetSteerInput(float input)
        {
            horizontalInput = Mathf.Clamp(input, -1f, 1f);
        }
        
        public void SetHandbrake(bool value)
        {
            driftInput = value ? 1f : 0f;
        }
        
        # endregion
    }
}