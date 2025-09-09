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

    private Rigidbody _rigidbody;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        _speed = _walkSpeed;
    }

    //Jangan lakukan subscribe di dalam method Awake karena pada method tersebut belum tentu object InputManager telah diload,
    private void Start() 
    {
        _input.onMoveInput += Move;
        _input.onSprintInput += Sprint;
        _input.onJumpInput += Jump;
    }

    private void Update()
    {
        CheckIsGrounded();
    }


    private void OnDestroy() 
    {
        _input.onMoveInput -= Move;
        _input.onSprintInput -= Sprint;
        _input.onJumpInput -= Jump;
        //kenapa harus unsubscribe? karena jika tidak unsubscribe, ketika object ini di destroy,
        //dia akan tetap mencoba untuk memanggil method Move, padahal object ini sudah tidak ada lagi, sehingga akan terjadi error.
        //btw ini digenerate otomatis oleh copilot.
    }

    private void Move(Vector2 axisDirection)
    {
        if (axisDirection.magnitude >= 0.1)
        {
            float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            Vector3 movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;

            _rigidbody.AddForce(movementDirection * Time.deltaTime * _speed);
            //Debug.Log(movementDirection);

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

}
