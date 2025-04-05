using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.RagdollAnimatorDemo
{
    /// <summary> hành động khi ragdoll bị tác động vật lý </summary>
    public class Demo_Ragd_Mover : MonoBehaviour
    {
        public Rigidbody Rigb;
        [Header( "Setting 'Grounded','Moving' and 'Speed' parameters for mecanim" )]
        public Animator Mecanim;
        [Space( 4 )]
        public float MovementSpeed = 2f;
        [Range( 0f, 1f )]
        public float RotateToSpeed = 0.8f;
        public bool AutoRotation = true;

        [Range( 0f, 1f )] public float DirectMovement = 0f;

        [Space( 4 )]
        public LayerMask GroundMask = 0 >> 1;
        [Space( 4 )]
        public float JumpPower = 3f;
        public float ExtraRaycastDistance = 0f;

        [Space( 4 )]
        public float HoldShiftForSpeed = 0f;
        public float HoldCtrlForSpeed = 0f;

        Quaternion targetRotation;
        Quaternion targetInstantRotation;
        bool isGrounded = true;

        bool wasInitialized = false;

        public void ResetTargetRotation()
        {
            targetRotation = transform.rotation;
            targetInstantRotation = transform.rotation;
            rotationAngle = transform.eulerAngles.y;

            currentWorldAccel = Vector3.zero;
            jumpRequest = 0f;
        }

        void Start()
        {

            if( !Rigb ) Rigb = GetComponent<Rigidbody>();
            if( Rigb )
            {
                Rigb.maxAngularVelocity = 30f;
                if( Rigb.interpolation == RigidbodyInterpolation.None ) Rigb.interpolation = RigidbodyInterpolation.Interpolate;
                Rigb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }

            targetRotation = transform.rotation;
            targetInstantRotation = transform.rotation;
            rotationAngle = transform.eulerAngles.y;

            if( Mecanim ) Mecanim.SetBool( "Grounded", true );

            wasInitialized = true;
        }

        private void OnEnable()
        {
            if( !wasInitialized ) return;
            ResetTargetRotation();
        }

        Vector2 moveDirectionLocal;
        Vector2 moveDirectionLocalNonZero;
        Vector3 moveDirectionWorld;
        float rotationAngle = 0f;
        float sd_rotationAngle = 0f;

        float toJump = 0f;

        void Update()
        {
            if( Rigb == null ) return;

            if( Input.GetKeyDown( KeyCode.Space ) )
            {
                if( toJump <= 0f )
                {
                    jumpRequest = JumpPower;
                    toJump = 0f;
                }
            }

            moveDirectionLocal = Vector2.zero;

            if( Input.GetKey( KeyCode.A ) ) moveDirectionLocal += Vector2.left;
            else if( Input.GetKey( KeyCode.D ) ) moveDirectionLocal += Vector2.right;

            if( Input.GetKey( KeyCode.W ) ) moveDirectionLocal += Vector2.up;
            else if( Input.GetKey( KeyCode.S ) ) moveDirectionLocal += Vector2.down;

            bool moving = false;
            Quaternion flatCamRot = Quaternion.Euler( 0f, Camera.main.transform.eulerAngles.y, 0f );

            if( moveDirectionLocal != Vector2.zero )
            {
                moveDirectionLocal.Normalize();
                moveDirectionWorld = flatCamRot * new Vector3( moveDirectionLocal.x, 0f, moveDirectionLocal.y );

                moving = true;
                moveDirectionLocalNonZero = moveDirectionLocal;
            }
            else
            {
                moveDirectionWorld = Vector3.zero;
            }

            if( Input.GetKey( KeyCode.R ) || moveDirectionLocal != Vector2.zero )
            {
                if( RotateToSpeed > 0f )
                    if( currentWorldAccel != Vector3.zero )
                    {
                        targetInstantRotation = Quaternion.LookRotation( currentWorldAccel );

                        rotationAngle = Mathf.SmoothDampAngle( rotationAngle, targetInstantRotation.eulerAngles.y, ref sd_rotationAngle, Mathf.Lerp( 0.5f, 0.01f, RotateToSpeed ) );
                        targetRotation = Quaternion.Euler( 0f, rotationAngle, 0f );// Quaternion.RotateTowards(targetRotation, targetInstantRotation, Time.deltaTime * 90f * RotateToSpeed);
                    }
            }

            if( Mecanim ) Mecanim.SetBool( "Moving", moving );

            float spd = MovementSpeed;
            if( HoldShiftForSpeed != 0f ) if( Input.GetKey( KeyCode.LeftShift ) ) spd = HoldShiftForSpeed;
            if( HoldCtrlForSpeed != 0f ) if( Input.GetKey( KeyCode.LeftControl ) ) spd = HoldCtrlForSpeed;

            float accel = 5f * MovementSpeed;
            if( !moving ) accel = 7f * MovementSpeed;

            currentWorldAccel = Vector3.MoveTowards( currentWorldAccel, moveDirectionWorld * spd, Time.deltaTime * accel );
            if( Mecanim ) if( moving ) Mecanim.SetFloat( "Speed", currentWorldAccel.magnitude );
        }


        Vector3 currentWorldAccel = Vector3.zero;

        float jumpRequest = 0f;
        private void FixedUpdate()
        {
            if( Rigb == null ) return;

            Vector3 targetVelo = currentWorldAccel;

            float yAngleDiff = Mathf.DeltaAngle( Rigb.rotation.eulerAngles.y, targetInstantRotation.eulerAngles.y );
            float directMovement = DirectMovement;
            directMovement *= Mathf.InverseLerp( 180f, 50f, Mathf.Abs( yAngleDiff ) );
            targetVelo = Vector3.Lerp( targetVelo, ( transform.forward ) * targetVelo.magnitude, directMovement );
            targetVelo.y = Rigb.velocity.y;

            toJump -= Time.fixedDeltaTime;

            if( jumpRequest != 0f && toJump <= 0f )
            {
                Rigb.position += transform.up * jumpRequest * 0.01f;
                targetVelo.y = jumpRequest;
                isGrounded = false;
                jumpRequest = 0f;
                jumpTime = Time.time;
                if( Mecanim ) Mecanim.SetBool( "Grounded", false );
            }
            else
            {
                if( isGrounded ) // Basic not recommended but working solution - snapping to the ground (this approach will push player down quick when loosing ground)
                {
                    targetVelo.y -= 2.5f * Time.fixedDeltaTime;
                }
            }

            Rigb.velocity = targetVelo;
            //Rigb.MovePosition( Rigb.position + targetVelo * Time.fixedDeltaTime );
            Rigb.angularVelocity = FEngineering.QToAngularVelocity( Rigb.rotation, targetRotation, true );

            if( Time.time - jumpTime > 0.2f )
            {
                //float radius = 0.3f;
                //if (Physics.SphereCast(new Ray(transform.position + transform.up, -transform.up), radius,   1.01f - radius, GroundMask, QueryTriggerInteraction.Ignore))
                if( Physics.Raycast( transform.position + transform.up, -transform.up, ( isGrounded ? 1.2f : 1.01f ) + ExtraRaycastDistance, GroundMask, QueryTriggerInteraction.Ignore ) )
                {
                    if( isGrounded == false )
                    {
                        isGrounded = true;
                        if( Mecanim ) Mecanim.SetBool( "Grounded", true );
                    }
                }
                else
                {
                    if( isGrounded == true )
                    {
                        isGrounded = false;
                        if( Mecanim ) Mecanim.SetBool( "Grounded", false );
                    }
                }
            }
            else
            {
                if( isGrounded == true )
                {
                    isGrounded = false;
                    if( Mecanim ) Mecanim.SetBool( "Grounded", false );
                }
            }

        }

        float jumpTime = -1f;

    }
}