using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAnalyticsService : IAnalyticsDataObserver
    {
        public UniTask InitializeAsync();
        public void SendStartGameAnalytics();
        public void SendEndGameAnalytics();
    }
}
