using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Interfaces;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class PlayerWeapons : MonoBehaviour, IWeaponsHolder
    {
        [SerializeField] private List<Transform> _heavySlots;
        [SerializeField] private List<Transform> _mainSlots;

        private readonly CompositeDisposable _disposables = new();
        private readonly HashSet<Transform> _occupiedSlots = new();

        private PlayerWeaponsViewModel _viewModel;

        public void Initialize(PlayerWeaponsViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnDeath
                .Subscribe(_ => Die())
                .AddTo(_disposables);
        }

        public Vector3 ApplyWeapon(WeaponType weaponType, WeaponView weapon)
        {
            return weaponType switch
            {
                WeaponType.Main => ApplyWeaponInSlot(_mainSlots, weapon),
                WeaponType.Secondary => ApplyWeaponInSlot(_heavySlots, weapon),
                _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null)
            };
        }

        private void FixedUpdate()
        {
            _viewModel.Update();
        }

        private Vector3 ApplyWeaponInSlot(List<Transform> slots, WeaponView weaponView)
        {
            var emptySlot = slots.FirstOrDefault(sl => !_occupiedSlots.Contains(sl));

            if (emptySlot == null) throw new Exception("No empty slot available for weapon application.");

            _occupiedSlots.Add(emptySlot);

            weaponView.transform.SetParent(emptySlot);
            weaponView.transform.localPosition = Vector3.zero;
            return emptySlot.transform.localPosition;
        }

        private void Die()
        {
            foreach (var slot in _occupiedSlots) Destroy(slot.gameObject);

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
