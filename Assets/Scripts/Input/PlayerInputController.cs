using System;
using Interfaces;
using Player;
using UnityEngine;
using Zenject;

public class PlayerInputController : ITickable
{ 
    private GameInput _gameInput;
    private IMoveControllable _moveControllable;

    public PlayerInputController(GameInput gameInput, IMoveControllable moveControllable)
    {
        _gameInput = gameInput;
        _gameInput.Enable();
        _moveControllable = moveControllable;
    }
    
    public void Tick()
    {
        var move = _gameInput.Player.Move.ReadValue<Vector2>();
        _moveControllable.Move(move);
    }
}
