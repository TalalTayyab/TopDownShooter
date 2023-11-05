using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody2;
    private Vector2 _movementInput;
    [SerializeField]
    private float _speed = 5f;
    private Vector2 _smoothMovement;
    private Vector2 _currentVelocity;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private float _screenBorder;
    private Camera _camera;
    private Animator _animator;

    private void Awake()
    {
        _rigidbody2 = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        SetVelocity();
        //RotationInDirectionOfInput();
        RotateInDirectionOfMouse();
        SetAnimation();
    }

    private void SetVelocity()
    {
       
        _smoothMovement = Vector2.SmoothDamp(_smoothMovement, _movementInput, ref _currentVelocity, 0.1f);
        _rigidbody2.velocity = _smoothMovement * _speed;
        PreventPlayerGoingOffScreen();
    }

    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _rigidbody2.velocity.x <0) || 
                (screenPosition.x > _camera.pixelWidth - _screenBorder && _rigidbody2.velocity.x >0))
        {
            _rigidbody2.velocity = new Vector2(0, _rigidbody2.velocity.y);
        }
        if ((screenPosition.y < _screenBorder && _rigidbody2.velocity.y < 0) ||
                (screenPosition.y > _camera.pixelHeight - _screenBorder && _rigidbody2.velocity.y > 0))
        {
            _rigidbody2.velocity = new Vector2(_rigidbody2.velocity.x, 0);
        }
    }

    private void SetAnimation()
    {
        bool isMoving = _movementInput != Vector2.zero;
        _animator.SetBool("IsMoving", isMoving);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void RotationInDirectionOfInput()
    {
        if (_movementInput != Vector2.zero)
        {
            var targetRotation = Quaternion.LookRotation(transform.forward, _smoothMovement);
            var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _rigidbody2.MoveRotation(rotation);
            
        }
    }

    void RotateInDirectionOfMouse()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        //transform.up = direction;
        var direction = mousePosition - _rigidbody2.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _rigidbody2.rotation = angle;
    }

    

}
