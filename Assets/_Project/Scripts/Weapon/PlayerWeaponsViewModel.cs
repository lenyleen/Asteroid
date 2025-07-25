﻿using System;
using System.Collections.Generic;
using Interfaces;
using UniRx;
using IInitializable = Zenject.IInitializable;

namespace Weapon
{
    public class PlayerWeaponsViewModel : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IGameEvents _gameEvents;
        private readonly List<WeaponViewModel> _heavyWeapons;
        private readonly PlayerInputController _inputService;
        private readonly List<WeaponViewModel> _mainWeapons;
        private readonly IPositionProvider _positionProvider;

        public ReactiveCommand OnDeath { get; } = new();

        public PlayerWeaponsViewModel(List<WeaponViewModel> weapons, List<WeaponViewModel> heavyWeapons,
            PlayerInputController inputService, IPositionProvider positionProvider, IGameEvents gameEvents)
        {
            _inputService = inputService;
            _positionProvider = positionProvider;
            _mainWeapons = weapons;
            _heavyWeapons = heavyWeapons;
            _gameEvents = gameEvents;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Initialize()
        {
            _gameEvents.OnGameEnded.Subscribe(_
                    => OnLose())
                .AddTo(_disposables);
        }

        public void Update()
        {
            HandleFireInput();
        }

        private void OnLose()
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

        private void HandleFireInput()
        {
            var attackInput = _inputService.GetAttackInputData();
            Fire(attackInput.IsMainFirePressed, _heavyWeapons);
            Fire(attackInput.IsHeavyFirePressed, _mainWeapons);
        }

        private void Fire(bool isPressed, List<WeaponViewModel> weapons)
        {
            if (!isPressed)
            {
                return;
            }

            weapons.ForEach(weapon =>
                weapon.TryFiree(_positionProvider));
        }
    }
}
