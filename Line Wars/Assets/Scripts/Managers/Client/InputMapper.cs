using System;
using Client;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public interface IInputMapper
{
    public event Action<AttackContext> PlayerCharacterAttacked;
    public event Action<Vector2> Moved;
    public Vector2 PointerWorldPosition { get; }
    public event Action<Vector2> CameraMoved;
}

public class InputMapper : MonoBehaviour, IInputMapper
{
    public event Action<Vector2> CameraMoved;
    public event Action<AttackContext> PlayerCharacterAttacked;
    public event Action<Vector2> Moved;
    public Vector2 PointerWorldPosition => _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    [Inject] private IInputManager _inputManager;
    
    private Camera _camera;

    [Inject]
    private void Construct()
    {
        // _inputManager.Pointer1Hold += OnCharacterAttack;
        _inputManager.Pointer2Pressed += OnPointer2Pressed;
        _camera = Camera.main;
        
        _inputManager.GeneralMovement += OnCameraMoved;
        
    }

    private void OnCameraMoved(Vector2 obj)
    {
        CameraMoved?.Invoke(obj);
    }

    private void OnPointer2Pressed(Vector2 pointerPosition)
    {
        var pointerRealPosition = _camera.ScreenToWorldPoint(pointerPosition);
        Moved?.Invoke(pointerRealPosition);
    }

    private void OnCharacterAttack(Vector2 pointerPosition)
    {
        var pointerRealPosition = _camera.ScreenToWorldPoint(pointerPosition);
        var playerCharacter = GetPlayerCharacter();
        var direction = ((Vector2)pointerRealPosition - (Vector2)playerCharacter.transform.position).normalized;
        var attackContext = new AttackContext
        {
            Attacker = playerCharacter,
            Direction = direction,
        };
        
        PlayerCharacterAttacked?.Invoke(attackContext);
    }

    private PlayerCharacter GetPlayerCharacter()
    {
        return FindObjectOfType<PlayerCharacter>();
    }

}