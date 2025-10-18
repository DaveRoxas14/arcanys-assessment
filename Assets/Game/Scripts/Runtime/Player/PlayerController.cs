using System;
using Game.Scripts.Runtime;
using Game.Scripts.Runtime.Audio;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Serialized Members
    
    [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Animator _animator;
    [SerializeField] private SoundEffect _jumpSfx;
    
    [Header(ArcanysConstants.INSPECTOR.INPUT_REFERENCE)]
    [SerializeField] private InputReader _inputReader;
    
    [Header(ArcanysConstants.INSPECTOR.MOVEMENT_HANDLER)]
    [SerializeField] private CharacterController _movementController;
    
    [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
    [SerializeField] private bool _enableMove;
    [SerializeField] private bool _enableJump;
    
    [Header(ArcanysConstants.INSPECTOR.MOVEMENT_SETTINGS)]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration = 12f;
    [SerializeField] private float _airControlMultiplier = 0.5f;
    [SerializeField] private float _rotationSpeed = 12f;
    
    [Header(ArcanysConstants.INSPECTOR.JUMP_SETTINGS)]
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

    #region Properties

    public Transform CameraTransform
    {
        get => _cameraTransform;
        set => _cameraTransform = value;
    }

    #endregion


    #region Unity Function

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayer(this);
        _inputReader.MoveEvent += OnMove;
        _inputReader.JumpEvent += OnJump;
        _inputReader.JumpCanceledEvent += OnJumpReleased;
        _inputReader.StoppedMovingEvent += OnStoppedMoving;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.JumpEvent -= OnJump;
        _inputReader.JumpCanceledEvent -= OnJumpReleased;
        _inputReader.StoppedMovingEvent -= OnStoppedMoving;
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

        if (value.magnitude > 0)
        {
            _animator.SetBool(ArcanysConstants.ANIMATIONS.RUN, true);
        }
    }

    private void OnStoppedMoving()
    {
        _animator.SetBool(ArcanysConstants.ANIMATIONS.RUN, false);
    }

    private void OnJump()
    {
        _lastJumpPressedTime = _jumpBufferTime;
        _isJumping = true;
        _animator.SetTrigger(ArcanysConstants.ANIMATIONS.JUMP);
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
        // Apply movement relative to camera...
        Vector3 direction;
        if (CameraTransform)
        {
            var forward = CameraTransform.forward;
            forward.y = 0;
            forward.Normalize();

            var right = CameraTransform.right;
            right.y = 0;
            right.Normalize();

            direction = forward * _moveInput.y + right * _moveInput.x;
        }
        else // if there's no camera references for some reason... why?...
        {
            direction = new Vector3(_moveInput.x, 0, _moveInput.y);
        }

        var control = _isGrounded ? 1f : _airControlMultiplier;
        var targetVelocity = direction * (_moveSpeed * control);

        var horizontalVelocity = new Vector3(_velocity.x, 0, _velocity.z);
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, _acceleration * Time.deltaTime);

        // Apply gravity...
        if (!_isGrounded)
        {
            var gravityMultiplier = 1f;

            if (_velocity.y < 0)
                gravityMultiplier = _fallMultiplier;
            else if (_velocity.y > 0 && !_isJumping)
                // if jump button is releasd early...
                gravityMultiplier = _variableJumpMultiplier;

            _velocity.y += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _velocity.x = horizontalVelocity.x;
        _velocity.z = horizontalVelocity.z;

        // Apply character movement...
        _movementController.Move(_velocity * Time.deltaTime);

        // Add rotation to character movement...
        if (direction.sqrMagnitude > 0.001f)
        {
            var targetRotation = Quaternion.LookRotation(direction);
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
            AudioManager.Instance.PlaySFX(_jumpSfx.clip);
        }
    }
    
    #endregion


    #region Helpers

    public void OnScoreUpdated(int value)
    {
        if(value >= 0) return;
        
        _animator.SetTrigger(ArcanysConstants.ANIMATIONS.TAKE_DAMAGE);
    }

    #endregion

    
}
