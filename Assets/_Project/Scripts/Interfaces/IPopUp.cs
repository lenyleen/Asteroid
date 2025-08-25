using System;
using _Project.Scripts.Other;
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

    public interface IDialog<TParams> : IPopUp
    {
        public void SetParams(TParams data);
        public UniTask<DialogResult> ShowDialogAsync(bool hideAfterChoice);
    }
}
