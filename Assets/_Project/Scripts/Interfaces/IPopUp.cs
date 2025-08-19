using System;
using _Project.Scripts.Data;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IPopUp
    {
        public void Initialize(Transform parent);
        public IObservable<IPopUp> OnClose { get; }
        public void Show();
    }

    public interface IPopUp<TVm> : IPopUp
    {
        public void Show(TVm vm);
    }

    public interface IDialogMenu<TParams, TResult> : IPopUp
    {
        public UniTask<TResult> ShowDialogAsync(TParams param);
    }
}
