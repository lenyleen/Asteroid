using System;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IPopUp
    {
        public IObservable<IPopUp> OnClose { get; }
        public void Show();
    }

    /*public interface IPopUp<TParam> : IPopUp
    {
        public void Show(TParam param);
    }*/

    public interface IDialogMenu<TParams, TResult> : IPopUp
    {
        UniTask<TResult> ShowDialogAsync(TParams param);
    }
}
