using System.Collections.Generic;
using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class PlayerWeaponsViewModel : IFixedTickable
    {
        private readonly List<WeaponViewModel> _mainWeapons = new();
        private readonly List<WeaponViewModel> _heavyWeapons = new();
        private readonly PlayerInputController _inputService;
        private readonly IPositionProvider _positionProvider;
        
        public ReactiveCommand OnHeavyFirePressed { get; } = new ReactiveCommand();
        public ReactiveCommand OnMainFirePressed { get; } = new ReactiveCommand();
        
        public PlayerWeaponsViewModel(List<WeaponViewModel> weapons,List<WeaponViewModel> heavyWeapons, 
            PlayerInputController inputService, IPositionProvider positionProvider)
        {
            _inputService = inputService;
            _positionProvider = positionProvider;
            _mainWeapons = weapons;
            _heavyWeapons = heavyWeapons;
        }
        
        public void FixedTick()
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
            if(!isPressed)
                return;

            weapons.ForEach(weapon => weapon.TryFiree(_positionProvider.Position.Value, _positionProvider.Rotation.Value));
            
        }

        
    }
    
}