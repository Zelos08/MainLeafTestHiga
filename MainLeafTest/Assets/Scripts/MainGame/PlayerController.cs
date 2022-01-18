using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private int i_coins;
    enum State { Normal, Pushing, Pause};
    private State e_state;
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

    //Variable to push
    private bool b_canPush;
    private Vector3 v3_pushDirection;
    private Rigidbody _PushObject;

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

        i_coins = PlayerPrefs.GetInt("coins");
        ChangeCoins(0);
        e_state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.GetPaused())
        {
            switch (e_state)
            {
                case State.Normal:
                    NormalMovimentInput();
                    break;
                case State.Pushing:
                    PushingInput();
                    break;
                case State.Pause:
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.ChangePouse();
        }
    }


    private void FixedUpdate()
    {
        if (!GameController.GetPaused())
        {
            switch (e_state)
            {
                case State.Normal:
                    CaculatePhysics();
                    SetAnimatorVariables();
                    break;
                case State.Pushing:
                    PushMoviment();
                    AnimatePushMovimente();
                    break;
                case State.Pause:
                    break;
                default:
                    break;
            }
        }
        else
        {
            _rigidbody.Sleep();
        }
        
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

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (b_canPush)
            {
                StartPush();
            }
        }
    }

    private void PushingInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            v3_pushDirection = -transform.right;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            v3_pushDirection = transform.forward;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            v3_pushDirection = -transform.forward;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            v3_pushDirection = transform.right;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            if (v3_pushDirection == -transform.right)
                v3_pushDirection = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (v3_pushDirection == transform.forward)
                v3_pushDirection = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            if (v3_pushDirection == -transform.forward)
                v3_pushDirection = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            if (v3_pushDirection == transform.right)
                v3_pushDirection = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (b_canPush)
            {
                StopPush();
            }
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

    private void resetVariables()
    {
        f_rotateVellocity = 0;
        f_velocity = 0;
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
        }
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

    //Box push Functions
    public void CanPushABox(bool valor, Rigidbody box = null)
    {
        b_canPush = valor;
        _PushObject = box;
        GameController.ChangeTip("Use o botão esquerdo do mouse para agarrar");
        if (!b_canPush)
        {
            StopPush();
            GameController.HideTip();
        }
    }

    private void StartPush()
    {
        ChangeState(State.Pushing);
        //set push Animation
        _animator.SetBool("Crouch", true);
        CorrectRotationToPush();
    }

    private void StopPush()
    {
        ChangeState(State.Normal);
        _animator.SetBool("Crouch", false);
        _PushObject = null;
    }

    private void CorrectRotationToPush()
    {
        transform.LookAt(_PushObject.transform);

        float x = _rigidbody.rotation.x > 45? 90:0;
        float z = _rigidbody.rotation.z > 45 ? 90 : 0;

        _rigidbody.transform.eulerAngles = new Vector3(x, _rigidbody.transform.eulerAngles.y, z);
    }
    private void PushMoviment()
    {
        _rigidbody.MovePosition(transform.position + v3_pushDirection * Time.deltaTime);
        _PushObject.MovePosition(_PushObject.position + v3_pushDirection * Time.deltaTime);
    }

    private void AnimatePushMovimente()
    {
        _animator.SetFloat("Forward", v3_pushDirection.magnitude);
    }
    //Controll Function
    private void ChangeState(State newState)
    {
        e_state = newState;
        resetVariables();
    }

    public void ChangeCoins(int valor)
    {
        i_coins += valor;
        GameController.ChangeCoins(i_coins);
        PlayerPrefs.SetInt("coins", i_coins);
    }
}
