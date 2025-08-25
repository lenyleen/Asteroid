using System;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.UI
{
   public class CrashHandler : MonoBehaviour, IBootstrapInitializable
    {
        private UiService _uiService;

        [Inject]
        private void Construct(UiService uiService)
        {
            _uiService = uiService;
        }

        public UniTask InitializeAsync()
        {
            Application.logMessageReceived += HandleLog;
            Application.logMessageReceivedThreaded += HandleLog;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            UniTaskScheduler.UnobservedTaskException += OnUniTaskUnobserved;

            return UniTask.CompletedTask;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
            Application.logMessageReceivedThreaded -= HandleLog;

            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;

            UniTaskScheduler.UnobservedTaskException -= OnUniTaskUnobserved;
        }

        private void OnUniTaskUnobserved(Exception ex)
        {
            _ = ShowErrorDialog(ex.Message);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                _ = ShowErrorDialog(ex.Message);
            else
                _ = ShowErrorDialog("Unhandled non-Exception error");
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception)
                return;

            _ = ShowErrorDialog(logString);
        }

        private async UniTask ShowErrorDialog(string message)
        {
            try
            {
                var errorData = new ErrorPopUpData($"{message}");
                var popUp = await _uiService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(errorData);

                await popUp.ShowDialogAsync(true);

                await UniTask.NextFrame();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
