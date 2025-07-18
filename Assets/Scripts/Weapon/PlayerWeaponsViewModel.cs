using System;
using System.Collections.Generic;
using DataObjects;
using Interfaces;
using Signals;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace Weapon
{
    public class PlayerWeaponsViewModel : IInitializable
    {
        private readonly List<WeaponViewModel> _mainWeapons = new();
        private readonly List<WeaponViewModel> _heavyWeapons = new();
        private readonly PlayerInputController _inputService;
        private readonly IPositionProvider _positionProvider;
        private readonly SignalBus _signalBus;

        public ReactiveCommand OnDeath { get; } = new();
        
        public PlayerWeaponsViewModel(List<WeaponViewModel> weapons,List<WeaponViewModel> heavyWeapons, 
            PlayerInputController inputService, IPositionProvider positionProvider, SignalBus signalBus)
        {
            _inputService = inputService;
            _positionProvider = positionProvider;
            _mainWeapons = weapons;
            _heavyWeapons = heavyWeapons;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LoseSignal>(OnLose);
        }

        private void OnLose(LoseSignal loseSignal)
        {
            DisposeWeapons(_heavyWeapons);
            DisposeWeapons(_mainWeapons);
        }

        private void DisposeWeapons(List<WeaponViewModel> weapons)
        {
            foreach (var weapon in weapons)
            {
                weapon.Dispose();
            }
            
            weapons.Clear();
        }
        
        public void Update()
        {
            HandleFireInput();
        }
        
        private void HandleFireInput()
        {
            var attackInput = _inputService.GetAttackInputData();
            Fire(attackInput.isMainFirePressed, _heavyWeapons);
            Fire(attackInput.isHeavyFirePressed, _mainWeapons);
        }

        private void Fire(bool isPressed, List<WeaponViewModel> weapons)
        {
            if (!isPressed)
                return;

            weapons.ForEach(weapon =>
                weapon.TryFiree(_positionProvider));
        }

    }
    
}