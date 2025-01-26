using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EduEngineLauncher
{
    /// <summary>
    /// Interaction logic for CreateProjectWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        public CreateProjectWindow()
        {
            InitializeComponent();
            UpdateFinalPath();
        }

        private string ProjectFilePath => $"{ProjectPath.Text}\\{ProjectNameInput.Text}\\{ProjectNameInput.Text}.csproj";

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select a project root folder",
            };

            if (dialog.ShowDialog() == true)
            {
                ProjectPath.Text = dialog.FolderName;
                UpdateFinalPath();
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ProjectNameInput.Text))
            {
                MessageBox.Show("Project Name cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(ProjectPath.Text))
            {
                MessageBox.Show("Project Root Path cannot be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Directory.CreateDirectory(ProjectPath.Text);
            Process.Start(new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"new classlib -n {ProjectNameInput.Text} -f net8.0",
                WorkingDirectory = ProjectPath.Text,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            })?.WaitForExit();

            File.WriteAllText(ProjectFilePath, ProjectTemplate.ProjectFile);

            var assetsFolder = $"{ProjectPath.Text}\\{ProjectNameInput.Text}\\Assets";
            Directory.CreateDirectory(assetsFolder);

            foreach (var csFile in Directory.EnumerateFiles($"{ProjectPath.Text}\\{ProjectNameInput.Text}", "*.cs"))
                File.Delete(csFile);

            File.WriteAllText($"{assetsFolder}\\FirstScript.cs",ProjectTemplate.ScriptTemplate(ProjectNameInput.Text, "FirstScript"));

            MessageBox.Show($"The project has been successfully created in the {ProjectPath.Text}\\{ProjectNameInput.Text} folder",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }

        private void ProjectNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFinalPath();
        }

        private void UpdateFinalPath()
        {
            if (string.IsNullOrEmpty(ProjectPath.Text))
            {
                FinalProjectPath.Foreground = new SolidColorBrush(Colors.Red);
                FinalProjectPath.Text = "Project Root Path is empty";
                return;
            }

            if (string.IsNullOrEmpty(ProjectNameInput.Text))
            {
                FinalProjectPath.Foreground = new SolidColorBrush(Colors.Red);
                FinalProjectPath.Text = "Project Name is empty";
                return;
            }

            FinalProjectPath.Foreground = new SolidColorBrush(Colors.Black);
            FinalProjectPath.Text = ProjectFilePath;
        }
    }
}
