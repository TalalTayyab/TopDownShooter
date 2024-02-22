using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _damagePrefab;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _screenBorder;
    [SerializeField] private Slider _dashCDUI;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private AudioSource _audioDamage;

    private Rigidbody2D _rigidbody2;
    private Vector2 _movementInput;
    private Vector2 _smoothMovement;
    private Vector2 _currentVelocity;
    private Camera _camera;
    private Animator _animator;
    private bool isDashing;
    private HealthController _hc;
    private float _dashCd;
    private bool _dashedOnCD;
    private float _healthRechargeCD;
    private TrailRenderer _trailRenderer;
    

    private void Awake()
    {
        _rigidbody2 = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
        _hc = GetComponent<HealthController>();
        _hc.isPlayerHealth = true;
        _hc.SetMaxHealth(PowerUpManagerFactory.PowerUpManager.PlayerHealth);
        _dashCd = PowerUpManagerFactory.PowerUpManager.PlayerDashCD;
        _healthRechargeCD = PowerUpManagerFactory.PowerUpManager.PlayerHealthRechargeCD;
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void HealthRecharge()
    {
        _healthRechargeCD -= Time.deltaTime;

        if (_healthRechargeCD <= 0)
        {
            _healthRechargeCD = PowerUpManagerFactory.PowerUpManager.PlayerHealthRechargeCD;
            _hc.AddHealth(PowerUpManagerFactory.PowerUpManager.PlayerHealthRecharge);
        }
    }

    public void OnDied()
    {
        Time.timeScale = 0;
        _gameOver.SetActive(true);
       
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    private void Update()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");


        if (_dashedOnCD)
        {
            _dashCd -= Time.deltaTime;
            _dashCDUI.value = 1 - (_dashCd / PowerUpManagerFactory.PowerUpManager.PlayerDashCD);
        }

        if (_dashCd <=0)
        {
            _dashedOnCD = false;
            
        }

        if (_dashedOnCD == false && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)))
        {
            _dashedOnCD = true;
            _dashCd = PowerUpManagerFactory.PowerUpManager.PlayerDashCD;
            StartCoroutine(Dash());
        }

        
    }

    

    private void FixedUpdate()
    {
        _hc.SetMaxHealth(PowerUpManagerFactory.PowerUpManager.PlayerHealth);

        if (!isDashing)
        {
            _smoothMovement = Vector2.SmoothDamp(_smoothMovement, _movementInput, ref _currentVelocity, 0.1f);
            _rigidbody2.velocity = _smoothMovement * PowerUpManagerFactory.PowerUpManager.PlayerSpeed;
        }
        else
        {
            /*_rigidbody2.velocity = new Vector2(
            _movementInput.x * PowerUpManagerFactory.PowerUpManager.PlayerDashSpeed,
            _movementInput.y * PowerUpManagerFactory.PowerUpManager.PlayerDashSpeed);*/

            _smoothMovement = Vector2.SmoothDamp(_smoothMovement, _movementInput, ref _currentVelocity, 0.1f);
            _rigidbody2.velocity = _smoothMovement * PowerUpManagerFactory.PowerUpManager.PlayerDashSpeed;
        }
        
        //RotationInDirectionOfInput();
       // RotateInDirectionOfMouse();
        SetAnimation();

        HealthRecharge();

    }

    private IEnumerator Dash()
    {
        isDashing = true;
        _trailRenderer.emitting = true;

        /*var tp = new Vector3(
            transform.position.x + (_movementInput.x * PowerUpManagerFactory.PowerUpManager.PlayerDashSpeed),
            transform.position.y + (_movementInput.y * PowerUpManagerFactory.PowerUpManager.PlayerDashSpeed));*/

        /*while (transform.position != tp)
        {
            transform.position = Vector2.MoveTowards(transform.position, tp, Time.deltaTime * PowerUpManagerFactory.PowerUpManager.PlayerDashSpeed * 10);
            yield return null;
        }
        //yield return new WaitForSeconds(PowerUpManagerFactory.PowerUpManager.PlayerDashCD);*/

        yield return new WaitForSeconds(0.2f);

        isDashing = false;
        _trailRenderer.emitting = false;
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

    public void TakeDamage()
    {
        var damage = Instantiate(_damagePrefab, transform);
        Destroy(damage, 0.5f);
        if (!_audioDamage.isPlaying) _audioDamage.Play();
    }

    //private void OnMove(InputValue inputValue)
    //{
    //    _movementInput = inputValue.Get<Vector2>();
    //}

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
