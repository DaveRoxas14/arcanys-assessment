using System;
using Arcanys.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ 
    // References
    [Header("References")]
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private Transform _cameraTransform;
    
    // Input Controller
    [Header("Input Reference")]
    [SerializeField] private InputReader _inputReader;
    
    // Movement Controller
    [Header("Movement Handler")]
    [SerializeField] private CharacterController _movementController;
    
    [Header("Settings")]
    [SerializeField] private bool _enableMove;
    [SerializeField] private bool _enableJump;
    
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration = 12f;
    [SerializeField] private float _rotationSpeed = 0.1f;
    [SerializeField] private float _airControlMultiplier = 0.5f;
    [SerializeField] private float rotationSpeed = 12f;
    
    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _fallMultiplier = 2.0f;
    [SerializeField] private float _coyoteTime = 0.15f;
    [SerializeField] private float _jumpBufferTime = 0.1f;
    [SerializeField] private float _variableJumpMultiplier = 0.5f;
    
    
    
    private Vector2 _moveInput;
    private float _turnSmoothVelocity;
    private bool _isJumping;
    private bool _isGrounded;
    private float _lastGroundedTime;
    private float _lastJumpPressedTime;
    private Vector3 _velocity;
    

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.JumpCanceledEvent += OnJumpReleased;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.JumpCanceledEvent -= OnJumpReleased;
    }

    private void OnMove(Vector2 value)
    {
        _moveInput = value;
    }

    private void OnJump()
    {
        _lastJumpPressedTime = _jumpBufferTime;
    }

    private void OnJumpReleased()
    {
        _isJumping = false;
    }
    
    private void Update()
    {
        HandleGroundCheck();
        HandleTimers();
        ApplyJump();
        ApplyMovement();
    }

    private void HandleTimers()
    {
        _lastGroundedTime -= Time.deltaTime;
        _lastJumpPressedTime -= Time.deltaTime;
    }

    private void HandleGroundCheck()
    {
        _isGrounded = _movementController.isGrounded;

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (_isGrounded)
            _lastGroundedTime = _coyoteTime;
    }
    
    private void ApplyMovement()
    {
        /*if (!_enableMove) return;
        
        var direction = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        if (direction.magnitude >= 0.1)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg 
                              + _cameraTransform.eulerAngles.y;
            
            var angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y, 
                targetAngle, 
                ref _turnSmoothVelocity, 
                _rotationSpeed);
            
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            var moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            
            _movementController.Move(moveDirection.normalized * _moveSpeed * Time.deltaTime);
        }*/
        
        var direction = Vector3.zero;
        if (_cameraTransform)
        {
            var forward = _cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();

            var right = _cameraTransform.right;
            right.y = 0;
            right.Normalize();

            direction = forward * _moveInput.y + right * _moveInput.x;
        }
        else
        {
            direction = new Vector3(_moveInput.x, 0, _moveInput.y);
        }

        var control = _isGrounded ? 1f : _airControlMultiplier;
        var targetVelocity = direction * (_moveSpeed * control);

        // Smooth acceleration
        var horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, _acceleration * Time.deltaTime);

        // Apply gravity
        if (!_isGrounded)
        {
            float gravityMultiplier = (_velocity.y < 0) ? _fallMultiplier : 1f;
            if (!_isJumping && _velocity.y > 0)
                gravityMultiplier = _variableJumpMultiplier;

            _velocity.y += _gravity * gravityMultiplier * Time.deltaTime;
        }
        
        _velocity.x = horizontalVelocity.x;
        _velocity.z = horizontalVelocity.z;

        // Apply character movement
        _movementController.Move(_velocity * Time.deltaTime);

        // Add rotation to character movement
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyJump()
    {
        if (!_enableJump) return;
        
        if (_lastJumpPressedTime > 0 && _lastGroundedTime > 0)
        {
            float jumpVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _velocity.y = jumpVelocity;

            _isJumping = true;
            _lastJumpPressedTime = 0;
            _lastGroundedTime = 0;
        }
    }
}
