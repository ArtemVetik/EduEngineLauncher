using System.IO;
using System.Windows;

namespace EduEngineLauncher
{
    internal class EduProject
    {
        private readonly EnginePathView _enginePathView;

        public EduProject(EnginePathView enginePathView)
        {
            _enginePathView = enginePathView;
        }

        public bool Open(string projectPath)
        {
            var assetsFolder = projectPath + "\\Assets";
            var dllPath = string.Empty;

            if (Directory.Exists(assetsFolder) == false)
            {
                MessageBox.Show($"Can't find {assetsFolder} directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            var binPath = projectPath + "\\bin";
            if (Directory.Exists(binPath) == false)
            {
                MessageBox.Show($"Can't find {binPath} directory", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            try
            {
                var targetDllName = Path.GetFileNameWithoutExtension(projectPath) + ".dll";
                foreach (string file in Directory.EnumerateFiles(binPath, targetDllName, SearchOption.AllDirectories))
                {
                    dllPath = file;
                    break;
                }

                if (string.IsNullOrEmpty(dllPath))
                    throw new Exception($"Can't find {targetDllName}");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            _enginePathView.LaunchEngine($"\"{assetsFolder}\" \"{dllPath}\"");

            return true;
        }
    }
}