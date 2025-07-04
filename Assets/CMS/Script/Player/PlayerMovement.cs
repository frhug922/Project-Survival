using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats _playerStats;
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private Animator _anim;
    private SpriteRenderer _playerRenderer;

    private bool _canRun = true;
    private bool _isRunning = false;

    private float _lastMoveX = 0f;
    private float _lastMoveY = -1f;

    public Vector2 MoveInput => _moveInput;
    public bool IsRunning => _isRunning;

    public Vector2 LastMoveDirection => new Vector2(_lastMoveX, _lastMoveY);

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStats = GetComponent<PlayerStats>();
        _anim = GetComponent<Animator>();
        _playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");
        _moveInput.Normalize();

        if (_moveInput.sqrMagnitude > 0f)
        {
            _lastMoveX = _moveInput.x;
            _lastMoveY = _moveInput.y;
        }

        _anim.SetFloat("Horizontal", 1f);
        _anim.SetFloat("Vertical", _moveInput.y);
        _anim.SetFloat("Speed", _moveInput.sqrMagnitude);
    }

    private void LateUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mouseWorldPos - transform.position;

        if (_playerRenderer != null)
            _playerRenderer.flipX = dir.x < 0f;
    }

    private void FixedUpdate()
    {
        if (_moveInput.sqrMagnitude == 0f)
        {
            _isRunning = false;
            RecoverStamina();
            return;
        }

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = ((Vector2)mouseWorldPos - _rb.position).normalized;
        float dot = Vector2.Dot(_moveInput, mouseDir);
        bool isForward = dot > 0.5f;

        float speed = _playerStats.MoveSpeed;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);

        if (_playerStats.CurrentStamina <= 0f)
        {
            _canRun = false;
            _isRunning = false;
        }

        if (!_isRunning && shiftHeld && _canRun && _playerStats.CurrentStamina >= 10f)
        {
            _isRunning = true;
        }

        if (_isRunning && shiftHeld)
        {
            speed = _playerStats.RunSpeed;
            _playerStats.DecreaseStamina(_playerStats.StaminaDecreaseRate * Time.fixedDeltaTime);
        }
        else
        {
            _isRunning = false;
            RecoverStamina();
        }

        if (!isForward)
        {
            speed *= 0.8f;
        }

        _rb.MovePosition(_rb.position + _moveInput * speed * Time.fixedDeltaTime);
    }

    private void RecoverStamina()
    {
        _playerStats.RecoverStamina(_playerStats.StaminaRecoveryRate * Time.fixedDeltaTime);

        if (_playerStats.CurrentStamina >= 50f)
        {
            _canRun = true;
        }
    }
}
