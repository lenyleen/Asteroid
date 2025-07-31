using System;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class PopUpFactory
    {
        private readonly Dictionary<Type, GameObject> _popUpPrefabs;
        private readonly Transform _popUpParent;
        private readonly IInstantiator _instantiator;

        public PopUpFactory(PopUpsConfig config, Transform popUpParent,
            IInstantiator instantiator)
        {
            _popUpPrefabs = InitializePopUps(config.PopUpPrefabs);

            _popUpParent = popUpParent;
            _instantiator = instantiator;
        }

        public T CreatePopUp<T>() where T : IPopUp
        {
            var type = typeof(T);
            if (!_popUpPrefabs.TryGetValue(type, out var prefab))
                throw new Exception($"No prefab found for {type.Name}");

            var instance = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            instance.Initialize(_popUpParent);
            return instance;
        }

        private Dictionary<Type, GameObject> InitializePopUps(List<GameObject> popUps)
        {
            var popUpDictionary = new Dictionary<Type, GameObject>();

            foreach (var popUp in popUps)
            {
                var componentGetResul = popUp.TryGetComponent<IPopUp>(out var component);

                if(componentGetResul)
                {
                    popUpDictionary.TryAdd(component.GetType(), popUp);
                    continue;
                }

                Debug.LogWarning("PopUp prefab does not implement IPopUp interface: " + popUp.name);
            }

            return popUpDictionary;
        }
    }
}
