using System;
using Arcanys.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Serialized Members
    
    [Header("References")]
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private Transform _cameraTransform;
    
    [Header("Input Reference")]
    [SerializeField] private InputReader _inputReader;
    
    [Header("Movement Handler")]
    [SerializeField] private CharacterController _movementController;
    
    [Header("Settings")]
    [SerializeField] private bool _enableMove;
    [SerializeField] private bool _enableJump;
    
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration = 12f;
    [SerializeField] private float _airControlMultiplier = 0.5f;
    [SerializeField] private float _rotationSpeed = 12f;
    
    [Header("Jump Settings")]
    [SerializeField] private float _jumpHeight = 2.5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _fallMultiplier = 2.0f;
    [SerializeField] private float _coyoteTime = 0.15f;
    [SerializeField] private float _jumpBufferTime = 0.1f;
    [SerializeField] private float _variableJumpMultiplier = 0.5f;

    #endregion

    #region Private Members

    private Vector2 _moveInput;
    private float _turnSmoothVelocity;
    private bool _isJumping;
    private bool _isGrounded;
    private float _lastGroundedTime;
    private float _lastJumpPressedTime;
    private Vector3 _velocity;

    #endregion
    
    #region Unity Function

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
    
    private void Update()
    {
        HandleGroundCheck();
        HandleTimers();
        ApplyJump();
        ApplyMovement();
    }

    #endregion

    #region Movement Funtions

    private void OnMove(Vector2 value)
    {
        _moveInput = value;
    }

    private void OnJump()
    {
        _lastJumpPressedTime = _jumpBufferTime;
        _isJumping = true;
    }

    private void OnJumpReleased()
    {
        _isJumping = false;
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
        Vector3 direction;
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
            var gravityMultiplier = 1f;

            if (_velocity.y < 0)
            {
                gravityMultiplier = _fallMultiplier;
            }
            else if (_velocity.y > 0 && !_isJumping)
            {
                // if jump button is releasd early
                gravityMultiplier = _variableJumpMultiplier;
            }

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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
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
    
    #endregion
    
    
   

    
}
