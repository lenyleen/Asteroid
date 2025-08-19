using _Project.Scripts.Interfaces;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class PopUpVmFactory
    {
        private readonly IInstantiator _instantiator;

        public TVm Create<TVm, TParam>(TParam param) where TVm : IPopUpViewModel
        {
            return _instantiator.Instantiate<TVm>(new []{(object)param});
            //TODO: ????? подумать нужно ли что-то добавить ?????
        }
    }
}
