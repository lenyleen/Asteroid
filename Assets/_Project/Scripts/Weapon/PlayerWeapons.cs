using System;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Weapon
{
    public class PlayerWeapons : MonoBehaviour, IWeaponsHolder
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly HashSet<WeaponView> _addedWeapons = new();

        private PlayerWeaponsViewModel _viewModel;
        private bool _isInitialized;

        public void Initialize(PlayerWeaponsViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnDeath
                .Subscribe(_ => Die())
                .AddTo(_disposables);

            _isInitialized = true;
        }

        public void ApplyWeapon(WeaponType weaponType, WeaponView weapon, Vector3 localPosition)
        {
            switch (weaponType)
            {
                case WeaponType.Main:
                    ApplyWeapon(weapon, localPosition);
                    break;
                case WeaponType.Secondary:
                    ApplyWeapon(weapon, localPosition);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
        }

        private void FixedUpdate()
        {
            if (_isInitialized)
                _viewModel.Update();
        }

        private void ApplyWeapon(WeaponView weaponView, Vector3 localPosition)
        {
            _addedWeapons.Add(weaponView);

            weaponView.transform.SetParent(transform);
            weaponView.transform.localPosition = localPosition;
        }

        private void Die()
        {
            foreach (var slot in _addedWeapons) Destroy(slot.gameObject);

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
