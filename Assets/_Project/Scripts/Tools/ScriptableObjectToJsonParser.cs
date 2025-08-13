using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Tools
{
    public class ScriptableObjectToJsonParser : MonoBehaviour
    {
        [SerializeField] private string _toSaveJsonPath;
        [SerializeField] private List<ScriptableObject> _scriptableObjects;

        public void Parse()
        {
            if (!Directory.Exists(_toSaveJsonPath))
                Debug.LogWarning("Folder doesn't exist");

            var serializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            foreach (var scriptableObject in _scriptableObjects)
            {
                var json = JsonConvert.SerializeObject(scriptableObject, serializerSettings);
                var fileName = scriptableObject.name;
                var path = Path.Combine(_toSaveJsonPath, fileName + ".json");
                SaveWithIndex(fileName, path, json);
            }

            Debug.Log("Parsed all files");
        }

        private void SaveWithIndex(string fileName, string path, string json)
        {
            var fileIndex = 0;

            while (File.Exists(path))
            {
                path = Path.Combine(_toSaveJsonPath, $"{fileName}_{fileIndex}.json");
                fileIndex++;
            }

            File.WriteAllText(path, json);
        }
    }
}
