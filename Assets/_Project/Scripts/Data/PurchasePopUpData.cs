using System.Threading;

namespace _Project.Scripts.Data
{
    public class PurchasePopUpData
    {
        public string ConfigId { get; }
        public CancellationTokenSource CancellationTokenSource { get; }

        public PurchasePopUpData(string configId, CancellationTokenSource cancellationTokenSource)
        {
            ConfigId = configId;
            CancellationTokenSource = cancellationTokenSource;
        }
    }
}
