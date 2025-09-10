using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputManager _input;

    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _walkSprintTransition;

    private float _speed;

    [SerializeField] private float _rotationSmoothTime = 0.1f;

    private float _rotationSmoothVelocity;

    [SerializeField] private float _jumpForce;
    
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private float _detectorRadius;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;

    [SerializeField] private Vector3 _upperStepOffset;
    [SerializeField] private float _stepCheckerDistance;
    [SerializeField] private float _stepForce;

    [SerializeField] private Transform _climbDetector;
    [SerializeField] private float _climbCheckDistance;
    [SerializeField] private LayerMask _climbableLayer;
    [SerializeField] private Vector3 _climbOffset;

    [SerializeField] private float _climbSpeed;

    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private CameraManager _cameraManager;

    private PlayerStance _playerStance;

    private Rigidbody _rigidbody;
    private Animator _animator;


    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        _speed = _walkSpeed;
        _playerStance = PlayerStance.Stand;
        HideAndLockCursor();
        _animator = GetComponent<Animator>();
    }

    //Jangan lakukan subscribe di dalam method Awake karena pada method tersebut belum tentu object InputManager telah diload,
    private void Start() 
    {
        _input.onMoveInput += Move;
        _input.onSprintInput += Sprint;
        _input.onJumpInput += Jump;
        _input.onClimbInput += StartClimb;
        _input.onCancelInput += CancelClimb;
    }

    private void Update()
    {
        CheckIsGrounded();
        CheckStep();
    }


    private void OnDestroy() 
    {
        _input.onMoveInput -= Move;
        _input.onSprintInput -= Sprint;
        _input.onJumpInput -= Jump;
        _input.onClimbInput -= StartClimb;
        _input.onCancelInput -= CancelClimb;
        //kenapa harus unsubscribe? karena jika tidak unsubscribe, ketika object ini di destroy,
        //dia akan tetap mencoba untuk memanggil method Move, padahal object ini sudah tidak ada lagi, sehingga akan terjadi error.
        //btw ini digenerate otomatis oleh copilot.
    }

    private void Move(Vector2 axisDirection)
    {
        Vector3 movementDirection = Vector3.zero;
        bool isPlayerStanding = _playerStance == PlayerStance.Stand;
        bool isPlayerClimbing = _playerStance == PlayerStance.Climb;


        if (isPlayerClimbing)
        {
            Vector3 horizontal = axisDirection.x * transform.right;
            Vector3 vertical = axisDirection.y * transform.up;
            movementDirection = horizontal + vertical;
            _rigidbody.AddForce(movementDirection * Time.deltaTime * _climbSpeed);
        }
        else if (isPlayerStanding)
        {
            switch (_cameraManager.cameraState)
            {
                case CameraState.ThirdPerson:
                    if (axisDirection.magnitude >= 0.1)
                    {
                        float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
                        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
                        movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;
                        //Debug.Log("speed: " + _speed);
                        _rigidbody.AddForce(movementDirection * Time.deltaTime * _speed);
                        //Debug.Log(movementDirection);

                        Vector3 velocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);
                        _animator.SetFloat("Velocity", velocity.magnitude * axisDirection.magnitude);

                    }
                    break;
                case CameraState.FirstPerson:
                    transform.rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);
                    Vector3 verticalDirection = axisDirection.y * transform.forward;
                    Vector3 horizontalDirection = axisDirection.x * transform.right;
                    movementDirection = verticalDirection + horizontalDirection;
                    _rigidbody.AddForce(movementDirection * Time.deltaTime * _speed);
                    break;
                default:
                    break;
            }


            
        }

    }

    private void Sprint(bool isSprint)
    {
        if (isSprint)
        {
            //Debug.Log("Sprinting");
            if (_speed < _sprintSpeed)
            {
                _speed = _speed + _walkSprintTransition * Time.deltaTime;
            }
        }
        else
        {
            if (_speed > _walkSpeed)
            {
                _speed = _speed - _walkSprintTransition * Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            Vector3 jumpDirection = Vector3.up;
            //_rigidbody.AddForce(jumpDirection * _jumpForce * Time.deltaTime, ForceMode.Impulse);
            _rigidbody.AddForce(jumpDirection * _jumpForce * Time.deltaTime);
        }

    }

    private void CheckIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundDetector.position, _detectorRadius, _groundLayer);
    }

    private void CheckStep()
    {
        bool isHitLowerStep = Physics.Raycast(_groundDetector.position,
                                                transform.forward,
                                                _stepCheckerDistance);

        bool isHitUpperStep = Physics.Raycast(_groundDetector.position +
                                                _upperStepOffset,
                                                transform.forward,
                                                _stepCheckerDistance);

        if (isHitLowerStep && !isHitUpperStep)
        {
            _rigidbody.AddForce(0, _stepForce, 0);
        }
    }

    private void StartClimb()
    {
        bool isInFrontOfClimbingWall = Physics.Raycast(_climbDetector.position,
                                                        transform.forward,
                                                        out RaycastHit hit,
                                                        _climbCheckDistance,
                                                        _climbableLayer);

        bool isNotClimbing = _playerStance != PlayerStance.Climb;

        if (isInFrontOfClimbingWall && _isGrounded && isNotClimbing)
        {
            Vector3 offset = (transform.forward * _climbOffset.z) + (Vector3.up * _climbOffset.y);
            transform.position = hit.point - offset;
            transform.forward = -hit.normal;
            _playerStance = PlayerStance.Climb;
            _rigidbody.useGravity = false;
            _cameraManager.SetFPSClampedCamera(true, transform.rotation.eulerAngles);
            _cameraManager.SetTPSFieldOfView(70);
        }
    }

    private void CancelClimb()
    {
        if (_playerStance == PlayerStance.Climb)
        {
            _playerStance = PlayerStance.Stand;
            _rigidbody.useGravity = true;
            transform.position -= transform.forward * 0.5f;
            _cameraManager.SetFPSClampedCamera(false, transform.rotation.eulerAngles);
            _cameraManager.SetTPSFieldOfView(40);
        }
    }

    private void HideAndLockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
