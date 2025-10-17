using System;
using Arcanys.Input;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputHandler.IPlayerActions, PlayerInputHandler.IUIActions
{
   public event Action<Vector2> MoveEvent;
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
   }

   public void OnDisable()
   {
      _inputHandler?.Player.Disable();
   }

   #region Player Actions

   public void OnMove(InputAction.CallbackContext context)
   {
      MoveEvent?.Invoke(context.ReadValue<Vector2>());
   }
   
   public void OnPause(InputAction.CallbackContext context)
   {
      if (context.performed)
         PauseEvent?.Invoke();
   }

   public void OnQuit(InputAction.CallbackContext context)
   {
      
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

   #endregion

   #region Helper Functions

   public void SwitchToUI()
   {
      _inputHandler.Player.Disable();
      _inputHandler.UI.Enable();
   }

   public void SwitchToPlayer()
   {
      _inputHandler.Player.Enable();
      _inputHandler.UI.Disable();
   }

   #endregion
}
