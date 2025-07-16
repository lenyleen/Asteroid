using System;
using Interfaces;
using Player;
using UnityEngine;
using Zenject;

public class PlayerInputController : ITickable
{ 
    private GameInput _gameInput;
    private Vector2 _inputValues;
    private AttackInputData _attackInputData;
    public PlayerInputController(GameInput gameInput)
    {
        _gameInput = gameInput;
        _gameInput.Enable();
    }
    
    public Vector2 GetInputValues()
    {
        return _inputValues;
    }
    public AttackInputData GetAttackInputData()
    {
        return _attackInputData;
    }
    
    public void Tick()
    {
        _inputValues = _gameInput.Player.Move.ReadValue<Vector2>();
        _attackInputData = new AttackInputData{isHeavyFirePressed = _gameInput.Player.HeavyAttack.IsPressed(),
            isMainFirePressed = _gameInput.Player.MainAttack.IsPressed()};
    }

    public struct AttackInputData
    {
        public bool isMainFirePressed;
        public bool isHeavyFirePressed;
        
    }
}
