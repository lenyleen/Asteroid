using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UniRx;

namespace Services
{
    public class UiService
    {
        private readonly Dictionary<Type, IPopUp> _popUps;

        private HashSet<IPopUp> _spawnedPopUps = new ();

        public UiService(List<IPopUp> popUps)
        {
            _popUps = popUps.ToDictionary(popUp => popUp.GetType(), popUp => popUp);
        }

        public TPopUp Show<TPopUp>() where TPopUp : class,IPopUp
        {
            var popUp = GetPopUp(typeof(TPopUp));
            ((TPopUp)popUp).Show();

            return (TPopUp)popUp;
        }

        public TPopUp ShowDialogMenu<TPopUp, TParam>(TParam param) where TPopUp : class,IDialogMenu<TParam>
        {
            var popUp = GetPopUp(typeof(TPopUp));
            ((TPopUp)popUp).Show(param);

            return (TPopUp)popUp;
        }

        private IPopUp GetPopUp(Type type)
        {
            if(!_popUps.TryGetValue(type, out var popUp))
                throw new Exception($"No popUp found for {type.Name}");

            popUp.OnClose.Take(1)
                .Subscribe(Despawn);
            _spawnedPopUps.Add(popUp);

            return popUp;
        }

        private void Despawn(IPopUp popUp)
        {
            if(_spawnedPopUps.Contains(popUp))
                _spawnedPopUps.Remove(popUp);
        }
    }
}
