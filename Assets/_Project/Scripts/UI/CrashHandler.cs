using System;
using System.Threading.Tasks;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            return UniTask.CompletedTask;
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowErrorDialog(e.Exception.Message, e.Exception.StackTrace).Forget();
            e.SetObserved();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                ShowErrorDialog(ex.Message, ex.StackTrace).Forget();
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if(type != LogType.Error)
                return;

            _ = ShowErrorDialog(logString, stackTrace);
        }

        private async UniTaskVoid ShowErrorDialog(string logString, string stackTrace)
        {
            try
            {
                await _uiService.ShowDialogAwaitable<ErrorPopUp, string, DialogResult>(logString + "\n" + stackTrace);
            }
            catch (Exception)
            {
                //ignored
            }

            CloseApp();
        }

        private void CloseApp()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }
    }
}
