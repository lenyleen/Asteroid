using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IPopUpService
    {
        public void ShowPopUp<TPopUp>() where TPopUp : IUnParametrizedPopUp;

        public UniTask<TPopUp> ShowDialogAwaitable<TPopUp, TParams>(TParams param)
            where TPopUp : class, IDialog<TParams> where TParams : IPopUpParams<TPopUp>;
    }
}
