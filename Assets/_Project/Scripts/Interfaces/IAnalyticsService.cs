using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAnalyticsService : IAnalyticsDataObserver, IAsyncInitializable
    {
        public void SendStartGameAnalytics();
        public void SendEndGameAnalytics();
    }
}
