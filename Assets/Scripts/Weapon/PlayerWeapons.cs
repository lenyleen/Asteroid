using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class PlayerWeapons : MonoBehaviour, IWeaponsHolder
    {
        public int HeavySlotsCapacity => _heavySlots.Capacity;
        public int MainSlotsCapacity => _mainSlots.Capacity;
        
        private PlayerWeaponsViewModel _viewModel;
        private HashSet<Transform> _occupiedSlots = new ();
        
        private readonly CompositeDisposable _disposables = new ();
        
        [SerializeField] private List<Transform> _heavySlots;
        [SerializeField] private List<Transform> _mainSlots;

        public void Initialize(PlayerWeaponsViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnDeath
                .Subscribe(_ => Die())
                .AddTo(_disposables);
        }
        
        public void ApplyWeapons(WeaponType weaponType, IWeapon weapon)
        {
            if(weapon is not WeaponView weaponView)
                throw new ArgumentException("Weapon must be of type WeaponView", nameof(weapon));
            
            switch (weaponType)
            {
                case WeaponType.Main:
                    ApplyWeaponInSlot(_mainSlots,weaponView);
                    break;
                case WeaponType.Secondary :
                    ApplyWeaponInSlot(_heavySlots,weaponView);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null);
            }
        }

        private void Die()
        {
            foreach (var slot in _occupiedSlots)
            {
                Destroy(slot.gameObject);
            }
            Destroy(this.gameObject);
        }


        private void FixedUpdate()
        {
            _viewModel.Update();
        }


        private void ApplyWeaponInSlot(List<Transform> slots, WeaponView weaponView)
        {
            var emptySlot = slots.FirstOrDefault(sl => !_occupiedSlots.Contains(sl));
            
            if (emptySlot == null)
                throw new Exception("No empty slot available for weapon application.");

            _occupiedSlots.Add(emptySlot);
            
            weaponView.transform.SetParent(emptySlot);
            weaponView.transform.localPosition = Vector3.zero;
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}