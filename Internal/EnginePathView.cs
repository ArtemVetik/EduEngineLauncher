using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EduEngineLauncher
{
    internal class EnginePathView
    {
        private readonly TextBox _pathText;
        private readonly TextBox _infoText;

        private string _engineRoot;
        private bool _launchDebug;

        public EnginePathView(TextBox pathText, TextBox infoText)
        {
            _pathText = pathText;
            _infoText = infoText;
            _launchDebug = false;

            Initialize(Properties.Settings.Default.EduEnginePath);
        }

        public string DebugExePath => _engineRoot + "\\x64\\Debug\\EduEngine.exe";
        public string ReleaseExePath => _engineRoot + "\\x64\\Release\\EduEngine.exe";
        public bool HasDebugExe => Path.Exists(DebugExePath);
        public bool HasReleaseExe => Path.Exists(ReleaseExePath);

        public void SetLaunchDebug(bool value)
        {
            _launchDebug = value;

            ValidateEnginePath();
        }

        public void Browse()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select a EduEngine folder",
            };

            if (dialog.ShowDialog() == true)
                Initialize(dialog.FolderName);
        }

        public bool LaunchEngine(string arguments)
        {
            try
            {
                var exePath = _launchDebug ? DebugExePath : ReleaseExePath;

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    WorkingDirectory = Path.GetDirectoryName(exePath),
                    UseShellExecute = false
                };

                Process process = Process.Start(startInfo);
                process.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error when launching exe: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Initialize(string engineRoot)
        {
            _engineRoot = engineRoot;
            _pathText.Text = engineRoot;

            ValidateEnginePath();

            Properties.Settings.Default.EduEnginePath = engineRoot;
            Properties.Settings.Default.Save();
        }

        private void ValidateEnginePath()
        {
            if (string.IsNullOrEmpty(_pathText.Text))
            {
                _infoText.Text = "Select EduEngine root location";
                _infoText.Visibility = Visibility.Visible;
                return;
            }

            string message = string.Empty;

            if (HasReleaseExe == false)
                message = $"Can't find {ReleaseExePath}";
            else if (_launchDebug && HasDebugExe == false)
                message = $"Can't find {DebugExePath}";

            if (message != string.Empty)
            {
                _infoText.Text = message;
                _infoText.Visibility = Visibility.Visible;
                _pathText.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                _infoText.Visibility = Visibility.Hidden;
                _pathText.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}