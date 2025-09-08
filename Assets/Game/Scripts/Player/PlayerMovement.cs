using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private InputManager _input;
    private Rigidbody _rigidbody;

    private void Awake() 
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    //Jangan lakukan subscribe di dalam method Awake karena pada method tersebut belum tentu object InputManager telah diload,
    private void Start() 
    {
        _input.onMoveInput += Move;
    }

    private void OnDestroy() 
    {
        _input.onMoveInput -= Move;
        //kenapa harus unsubscribe? karena jika tidak unsubscribe, ketika object ini di destroy,
        //dia akan tetap mencoba untuk memanggil method Move, padahal object ini sudah tidak ada lagi, sehingga akan terjadi error.
        //btw ini digenerate otomatis oleh copilot.
    }

    private void Move(Vector2 axisDirection)
    {
        Vector3 movementDirection = new Vector3(axisDirection.x, 0, axisDirection.y);
        _rigidbody.AddForce(movementDirection * _walkSpeed * Time.deltaTime);
        //Debug.Log(movementDirection);
    }

}
