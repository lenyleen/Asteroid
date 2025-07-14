using DataObjects;
using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerModel
    {
        public  PlayerPreferences Preferences => _playerPreferences;
        private readonly PlayerPreferences _playerPreferences;

        public PlayerModel(PlayerPreferences playerPreferences)
        {
            _playerPreferences = playerPreferences;
        }
    }
}