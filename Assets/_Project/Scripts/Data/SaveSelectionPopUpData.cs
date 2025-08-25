using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.PopUps;

namespace _Project.Scripts.Data
{
    public class SaveSelectionPopUpData : IPopUpParams<SaveSelectionPopUp>
    {
        public string LocalSaveCreatedAt { get; private set; }
        public string RemoteSaveCreatedAt { get; private set; }

        public SaveSelectionPopUpData(string localSaveCreatedAt, string remoteSaveCreatedAt)
        {
            LocalSaveCreatedAt = localSaveCreatedAt;
            RemoteSaveCreatedAt = remoteSaveCreatedAt;
        }
    }
}
