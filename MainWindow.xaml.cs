using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace EduEngineLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EnginePathView _enginePathView;
        private EduProject _eduProject;
        private RecentProjectViewContainer _recentProjects;

        public MainWindow()
        {
            InitializeComponent();            

            _enginePathView = new EnginePathView(EnginePath, InfoTextBox);
            _eduProject = new EduProject(_enginePathView);

            _recentProjects = new RecentProjectViewContainer(RenectProjectsPanel, _eduProject);
            var projects = _recentProjects.Load();

            foreach (var project in projects)
                _recentProjects.TryAddView(project);
        }

        private void BrowseEnginePath_Click(object sender, RoutedEventArgs e)
        {
            _enginePathView.Browse();
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select a Folder",
            };

            if (dialog.ShowDialog() == true)
            {
                string folderPath = dialog.FolderName;

                if (_eduProject.Open(folderPath) == false)
                    return;

                if (_recentProjects.TryAddView(new ProjectInfo() { Path = folderPath, Date = DateTime.Now.Ticks }))
                    _recentProjects.Save();
            }
        }

        private void LaunchDebug_Checked(object sender, RoutedEventArgs e)
        {
            _enginePathView.SetLaunchDebug(true);
        }

        private void LaunchDebug_Unchecked(object sender, RoutedEventArgs e)
        {
            _enginePathView.SetLaunchDebug(false);
        }

        private void CreateProject_Click(object sender, RoutedEventArgs e)
        {
            new CreateProjectWindow().Show();
        }
    }
}