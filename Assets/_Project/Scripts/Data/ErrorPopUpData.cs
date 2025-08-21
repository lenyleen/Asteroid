using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.PopUps;

namespace _Project.Scripts.Data
{
    public class ErrorPopUpData : IPopUpParams<ErrorPopUp>
    {
        public string Message { get; private set; }

        public ErrorPopUpData(string message)
        {
            Message = message;
        }
    }
}
