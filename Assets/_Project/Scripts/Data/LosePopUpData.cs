using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.PopUps;

namespace _Project.Scripts.Data
{
    public class LosePopUpData : IPopUpParams<LosePopUp>
    {
        public int Score { get; private set; }

        public LosePopUpData(int score)
        {
            Score = score;
        }
    }
}
