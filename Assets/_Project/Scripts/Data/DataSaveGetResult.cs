using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;

namespace _Project.Scripts.Data
{
    public class DataSaveGetResult<T> where T : ILoadedData
    {
        public bool Success { get; private set; }
        public SavedData<T> Data { get; private set; }

        public DataSaveGetResult(bool success, SavedData<T> data)
        {
            Success = success;
            Data = data;
        }
    }
}
