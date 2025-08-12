using System;
using System.Collections.Generic;
using _Project.Scripts.Input;
using _Project.Scripts.Interfaces;
using UniRx;
using IInitializable = Zenject.IInitializable;

namespace _Project.Scripts.Weapon
{
    public class PlayerWeaponsViewModel : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly List<WeaponViewModel> _heavyWeapons;
        private readonly PlayerInputController _inputService;
        private readonly List<WeaponViewModel> _mainWeapons;
        private readonly IPositionProvider _positionProvider;

        public ReactiveCommand OnDeath { get; } = new();

        public PlayerWeaponsViewModel(List<WeaponViewModel> weapons, List<WeaponViewModel> heavyWeapons,
            PlayerInputController inputService, IPositionProvider positionProvider)
        {
            _inputService = inputService;
            _positionProvider = positionProvider;
            _mainWeapons = weapons;
            _heavyWeapons = heavyWeapons;
        }

        public void Initialize()
        {
            _positionProvider.OnDeath.Subscribe(_ =>
                OnLose())
                .AddTo(_disposables);
        }

        public void Update() => HandleFireInput();

        public void Dispose() => _disposables.Dispose();

        private void OnLose()
        {
            DisposeWeapons(_heavyWeapons);
            DisposeWeapons(_mainWeapons);
        }

        private void HandleFireInput()
        {
            var attackInput = _inputService.GetAttackInputData();
            Fire(attackInput.IsMainFirePressed, _heavyWeapons);
            Fire(attackInput.IsHeavyFirePressed, _mainWeapons);
        }

        private async void Fire(bool isPressed, List<WeaponViewModel> weapons)
        {
            if (!isPressed) return;

            foreach (var weaponViewModel in weapons)
                await weaponViewModel.TryFiree(_positionProvider);
        }

        private void DisposeWeapons(List<WeaponViewModel> weapons)
        {
            foreach (var weapon in weapons) weapon.Dispose();

            weapons.Clear();
        }
    }
}
