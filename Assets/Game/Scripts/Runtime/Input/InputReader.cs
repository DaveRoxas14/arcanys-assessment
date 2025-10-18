using System;
using Arcanys.Input;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputHandler.IPlayerActions, PlayerInputHandler.IUIActions
{
   public event Action<Vector2> MoveEvent;

   public event Action StoppedMovingEvent;
   public event Action PauseEvent;
   public event Action JumpEvent;
   public event Action JumpCanceledEvent;

   private PlayerInputHandler _inputHandler;

   public void OnEnable()
   {
      if (_inputHandler == null)
      {
         _inputHandler = new PlayerInputHandler();
         _inputHandler.Player.SetCallbacks(this);
      }
      
      _inputHandler.Player.Enable();
      _inputHandler.UI.Enable();
   }

   public void OnDisable()
   {
      _inputHandler?.Player.Disable();
      _inputHandler?.UI.Disable();
   }

   #region Player Actions

   public void OnMove(InputAction.CallbackContext context)
   {
      MoveEvent?.Invoke(context.ReadValue<Vector2>());

      if (context.canceled)
      {
         StoppedMovingEvent?.Invoke();
      }
   }
   
   public void OnPause(InputAction.CallbackContext context)
   {
      if (context.performed)
         PauseEvent?.Invoke();
   }

   public void OnLook(InputAction.CallbackContext context)
   {
   }

   public void OnAttack(InputAction.CallbackContext context)
   {
   }

   public void OnInteract(InputAction.CallbackContext context)
   {
   }

   public void OnCrouch(InputAction.CallbackContext context)
   {
   }

   public void OnJump(InputAction.CallbackContext context)
   {
      if (context.performed)
         JumpEvent?.Invoke();
      else if (context.canceled)
         JumpCanceledEvent?.Invoke();
   }

   public void OnPrevious(InputAction.CallbackContext context)
   {
   }

   public void OnNext(InputAction.CallbackContext context)
   {
   }

   public void OnSprint(InputAction.CallbackContext context)
   {
   }

   #endregion

   #region UI Actions

   public void OnNavigate(InputAction.CallbackContext context)
   {
   }

   public void OnSubmit(InputAction.CallbackContext context)
   {
   }

   public void OnCancel(InputAction.CallbackContext context)
   {
   }

   public void OnPoint(InputAction.CallbackContext context)
   {
   }

   public void OnClick(InputAction.CallbackContext context)
   {
   }

   public void OnRightClick(InputAction.CallbackContext context)
   {
   }

   public void OnMiddleClick(InputAction.CallbackContext context)
   {
   }

   public void OnScrollWheel(InputAction.CallbackContext context)
   {
   }

   public void OnTrackedDevicePosition(InputAction.CallbackContext context)
   {
   }

   public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
   {
   }

   public void OnUnpause(InputAction.CallbackContext context)
   {
   }

   public void OnResumeGame(InputAction.CallbackContext context)
   {
      if (context.performed)
      {
         PauseEvent?.Invoke();
         Debug.Log("[Unpause] Success");
      }
   }

   #endregion

   #region Helper Functions

   public void SwitchToUI()
   {
      Debug.Log("[Input] Switched to UI input");
      _inputHandler.Player.Disable();
      _inputHandler.UI.Enable();
      
      Debug.Log($"_inputHandler.Player : { _inputHandler.Player.enabled}");
      Debug.Log($"_inputHandler.UI : { _inputHandler.UI.enabled}");
   }

   public void SwitchToPlayer()
   {
      Debug.Log("[Input] Switched to Player input");
      _inputHandler.Player.Enable();
      _inputHandler.UI.Disable();
      
      Debug.Log($"_inputHandler.Player : { _inputHandler.Player.enabled}");
      Debug.Log($"_inputHandler.UI : { _inputHandler.UI.enabled}");
   }

   #endregion
}
