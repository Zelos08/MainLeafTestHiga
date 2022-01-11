using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;


    //Physics Variables
    public float f_deceleration = 2;

    //Variables of velocity
    public int i_maxVel = 2;
    public float f_acceleration = 0.03f;
    private bool b_accelerate;
    private float f_velocity;

    //Variables of rotation
    public int i_maxRotateVel = 1;
    public float f_rotateAcceleration = 0.05f;
    private float f_rotateVellocity;
    private float f_rotateDirection; //Say the direction of rotation 
    private Vector3 v3_eulerAngleVelocity;

    //Variables to jump
    public float f_distanceToCheckGround = 0.01f;
    public float f_jumpForce = 5f;
    public float f_jumpAnimationVelocity = 5;
    private bool b_onGround;
    private RaycastHit hit;

    //Variables to Crouch 
    public float f_crouchHeight;
    private BoxCollider _boxCollider;
    private Vector3 v3_BoxColliderNormalSize;
    private Vector3 v3_BoxColliderNormalCenter;
    private Vector3 v3_BoxColliderCrouchSize;
    private Vector3 v3_BoxColliderCrouchCenter;
    void Start()
    {
        //Get the Animator attached to the GameObject.
        _animator = gameObject.GetComponentInChildren<Animator>();
      
        if (_animator == null)
        {
            Debug.LogException(new UnityException("Add an animator to the player."));
            return;
        }

        //Get the rigidbody attached to the GameObject.
        _rigidbody = GetComponent<Rigidbody>();
        v3_eulerAngleVelocity = new Vector3(0, 100, 0);
        if (_rigidbody == null)
        {
            Debug.LogException(new UnityException("Add an rigidbody to the player."));
            return;
        }

        _boxCollider = GetComponent<BoxCollider>();
        v3_BoxColliderNormalSize = _boxCollider.size;
        v3_BoxColliderNormalCenter = _boxCollider.center;
        v3_BoxColliderCrouchSize = new Vector3(v3_BoxColliderNormalSize.x, f_crouchHeight, v3_BoxColliderNormalSize.z);
        v3_BoxColliderCrouchCenter = new Vector3(v3_BoxColliderNormalCenter.x, f_crouchHeight/2, v3_BoxColliderNormalCenter.z);
        if (_boxCollider == null)
        {
            Debug.LogException(new UnityException("Add an boxCollider to the player."));
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        NormalMovimentInput();      
    }


    private void FixedUpdate()
    {
        CaculatePhysics();
        SetAnimatorVariables();
    }

    //Moviment Functions

    private void NormalMovimentInput()
    {
        //Press the up button to reset the trigger and set another one
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetRotateY(-1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            StopRotateY(-1);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Accelerate(true);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Accelerate(false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetRotateY(1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            StopRotateY(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateJumpForce();
            StartJumpAnimation();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchDown();
            _animator.SetBool("Crouch", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            crouchUp();
            _animator.SetBool("Crouch", false);
        }
    }

    private void Accelerate(bool valor)
    {
        b_accelerate = valor;
    }

    private void SetRotateY(float direction)
    {
        f_rotateDirection = direction;
    }

    private void StopRotateY(float direction)
    {
        if(f_rotateDirection == direction)
        {
            f_rotateDirection = 0;
        }
    }

    private void crouchDown()
    {
        if (b_onGround)
        {
            _boxCollider.size = v3_BoxColliderCrouchSize;
            _boxCollider.center = v3_BoxColliderCrouchCenter;
        }
    }

    private void crouchUp()
    {
        _boxCollider.size = v3_BoxColliderNormalSize;
        _boxCollider.center = v3_BoxColliderNormalCenter;
    }

    //Apply Physics functions
    private void CaculatePhysics()
    {
        CalculateRotation();
        CalculateVelocity();
        CheckIfIsOnGround();
    }

    private void CalculateRotation()
    {
        //Accelerate rotation.
        if (f_rotateDirection > 0) f_rotateVellocity += f_rotateAcceleration;
        else if (f_rotateDirection < 0) f_rotateVellocity -= f_rotateAcceleration;
        //Desacelerate rotation.
        else
        {
            if ((f_rotateVellocity <= f_rotateAcceleration * f_deceleration * 1.5 && f_rotateVellocity > 0) || (f_rotateVellocity >= -f_rotateAcceleration * f_deceleration * 1.5 && f_rotateVellocity < 0))
                f_rotateVellocity = 0;
            else
            {
                if (f_rotateVellocity > 0) f_rotateVellocity -= f_rotateAcceleration * f_deceleration;
                else if (f_rotateVellocity < 0) f_rotateVellocity += f_rotateAcceleration * f_deceleration;
            }
            
        }
        f_rotateVellocity = Mathf.Clamp(f_rotateVellocity, -i_maxRotateVel, i_maxRotateVel);

        Quaternion deltaRotation = Quaternion.Euler(v3_eulerAngleVelocity * Time.fixedDeltaTime * f_rotateVellocity);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    private void CalculateVelocity()
    {
        if (b_accelerate) f_velocity += f_acceleration;
        else f_velocity -= f_acceleration * f_deceleration;

        f_velocity = Mathf.Clamp(f_velocity, 0, i_maxVel);

        Vector3 tempVect = transform.forward;
        tempVect = tempVect * f_velocity * Time.deltaTime;
        _rigidbody.MovePosition(transform.position + tempVect);
    }

    private void CalculateJumpForce()
    {
        if (b_onGround)
        {
            _rigidbody.AddRelativeForce(Vector3.up * f_jumpForce, ForceMode.Impulse);
            b_onGround = false;
        }
    }

    private void CheckIfIsOnGround()
    {
        //Use the "Jump" to check if player jumped
        if (!b_onGround && _animator.GetFloat("Jump") > -8)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            b_onGround = Physics.Raycast(ray, out hit, f_distanceToCheckGround);
            Debug.Log("veio ak");
        }
        Debug.Log(b_onGround);
    }
    //Animator Functions
    private void SetAnimatorVariables()
    {
        _animator.SetFloat("Turn", f_rotateVellocity);
        _animator.SetFloat("Forward", f_velocity);
        _animator.SetBool("OnGround", b_onGround);
        PlayJumpAnimation();
    }

    private void StartJumpAnimation()
    {
        //put it at -9 because of the animation
        _animator.SetFloat("Jump", -9);
    }

    private void PlayJumpAnimation()
    {
        if (!b_onGround)
        {
            _animator.SetFloat("Jump", _animator.GetFloat("Jump") + Time.deltaTime * f_jumpAnimationVelocity);
        }
    }
}
