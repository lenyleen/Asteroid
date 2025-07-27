using System;
using Cysharp.Threading.Tasks;
using Other;
using UniRx;

namespace Interfaces
{
    public interface IPopUp
    {
        public IObservable<IPopUp> OnClose { get; }
        public void Show();
        public void Hide();
    }

    public interface IPopUp<TParam> : IPopUp
    {
        public void Show(TParam param);
    }

    public interface IDialogMenu<TParams> : IPopUp<TParams>
    {
        IObservable<bool> OnComplete { get; }
    }
}
