using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Client
{
    public interface IInputManager
    {
        event Action<Vector2> GeneralMovement;
        event Action<Vector2> GeneralMovementFixed;
        event Action<Vector2> GeneralMovementChanged;
        event Action<Vector2> Pointer1Pressed;
        event Action<Vector2> Pointer2Pressed;
        public event Action<Vector2> Pointer1Hold;
        event Action OnConfirm;
        event Action OnSpeedUpDialog;
        event Action OnSkip;
    }

    public class InputManager : MonoBehaviour, IInputManager
    {
        public event Action<Vector2> GeneralMovement;
        public event Action<Vector2> GeneralMovementFixed;
        public event Action<Vector2> GeneralMovementChanged;
        
        public event Action<Vector2> CameraMovement;
        
        public event Action<Vector2> Pointer1Pressed;
        public event Action<Vector2> Pointer2Pressed;
        public event Action<Vector2> Pointer1Hold;
        public event Action OnConfirm;
        public event Action OnSpeedUpDialog;
        public event Action OnSkip;

        [SerializeField] private int uiLayer = 5;

        private InputActions _inputActions;
        private Vector2 _cachedCharacterMovement = Vector2.zero;

        private void Awake()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();

            _inputActions.General.Movement.performed +=
                ctx => GeneralMovement?.Invoke(ctx.ReadValue<Vector2>());
            
            _inputActions.Camera.Movement.performed +=
                ctx => CameraMovement?.Invoke(ctx.ReadValue<Vector2>());

            _inputActions.General.Pointer1.performed += Pointer1OnPerformed;
            _inputActions.General.Pointer2.performed += Pointer2OnPerformed;

            _inputActions.UI.Confirm.performed += _ => OnConfirm?.Invoke();

            _inputActions.UI.SkipDialog.performed += _ => OnSkip?.Invoke();
        
            _inputActions.UI.SpeedUpDialog.performed += _ => OnSpeedUpDialog?.Invoke();
        }

        private void Update()
        {
            var movement = _inputActions.General.Movement.ReadValue<Vector2>();
            if (movement.magnitude > 0)
                GeneralMovement?.Invoke(movement);

            if (_inputActions.General.Pointer1.IsPressed())
            {
                Pointer1Hold?.Invoke(Mouse.current.position.ReadValue());
            }
        }

        private void FixedUpdate()
        {
            var movement = _inputActions.General.Movement.ReadValue<Vector2>();
            if (movement.magnitude > 0)
                GeneralMovementFixed?.Invoke(movement);

            if (_cachedCharacterMovement != movement)
            {
                _cachedCharacterMovement = movement;
                GeneralMovementChanged?.Invoke(movement);
            }
        }

        private void Pointer1OnPerformed(InputAction.CallbackContext callback)
        {
            if (IsPointerOverUI()) return;

            var pointerPosition = Mouse.current.position;
            Pointer1Pressed?.Invoke(pointerPosition.ReadValue());
        }

        private void Pointer2OnPerformed(InputAction.CallbackContext callback)
        {
            if (IsPointerOverUI()) return;

            var pointerPosition = Mouse.current.position;
            Pointer2Pressed?.Invoke(pointerPosition.ReadValue());
        }

        private bool IsPointerOverUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Mouse.current.position.ReadValue();
            List<RaycastResult> raycastResultsList = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

            for (int i = 0; i < raycastResultsList.Count; i++)
            {
                var raycastResult = raycastResultsList[i];
                if (raycastResult.gameObject.transform is RectTransform && raycastResult.gameObject.layer == uiLayer)
                {
                    return true;
                }
            }

            return false;
        }
    }
}