using UniRx;

namespace UI
{
    public class PlayerModel
    {
        public ReactiveProperty<int> Score { get; } = new();

        public void SavePlayerDataToScore(string playerName)
        {
            
        }
    }
}