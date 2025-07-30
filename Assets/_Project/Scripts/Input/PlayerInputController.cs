using UnityEngine;
using Zenject;

public class PlayerInputController : ITickable
{
    private readonly GameInput _gameInput;

    private AttackInputData _attackInputData;
    private Vector2 _inputValues;

    public PlayerInputController(GameInput gameInput)
    {
        _gameInput = gameInput;
        _gameInput.Enable();
    }

    public void Tick()
    {
        _inputValues = _gameInput.Player.Move.ReadValue<Vector2>();
        _attackInputData = new AttackInputData
        {
            IsHeavyFirePressed = _gameInput.Player.HeavyAttack.IsPressed(),
            IsMainFirePressed = _gameInput.Player.MainAttack.IsPressed()
        };
    }

    public Vector2 GetInputValues()
    {
        return _inputValues;
    }

    public AttackInputData GetAttackInputData()
    {
        return _attackInputData;
    }

    public struct AttackInputData
    {
        public bool IsMainFirePressed;
        public bool IsHeavyFirePressed;
    }
}
