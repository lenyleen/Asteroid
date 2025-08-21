using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IPopUp
    {
        public void Initialize(Transform parent);
        public IObservable<IPopUp> OnClose { get; }
        public void Hide();
    }

    public interface IDialog<TParams, TResult> : IPopUp
    {
        public void SetParams(TParams message);
        public UniTask<TResult> ShowDialogAsync(bool hideAfterChoice = true);
    }
}
