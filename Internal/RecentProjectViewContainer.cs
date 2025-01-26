using Newtonsoft.Json;
using System.Windows.Controls;

namespace EduEngineLauncher
{
    class ProjectInfo
    {
        public string Path { get; set; }
        public long Date { get; set; }
    }
    internal class RecentProjectViewContainer
    {
        private readonly StackPanel _parent;
        private readonly EduProject _eduProject;
        private readonly List<RecentProjectView> _viewList;

        public RecentProjectViewContainer(StackPanel parent, EduProject eduProject)
        {
            _parent = parent;
            _eduProject = eduProject;
            _viewList = new List<RecentProjectView>();
        }

        public List<ProjectInfo> Load()
        {
            return JsonConvert.DeserializeObject<List<ProjectInfo>>(Properties.Settings.Default.RecentProjects) ?? [];
        }

        public void Save()
        {
            var recentProjects = _viewList.Select(view => view.ProjectInfo).ToList();
            Properties.Settings.Default.RecentProjects = JsonConvert.SerializeObject(recentProjects);
            Properties.Settings.Default.Save();
        }

        public bool TryAddView(ProjectInfo projectInfo)
        {
            if (_viewList.Any(view => view.ProjectInfo.Path == projectInfo.Path))
                return false;

            var view = new RecentProjectView(projectInfo, OnOpen, OnRemove);
            view.Build();

            _parent.Children.Add(view.Panel);
            _viewList.Add(view);
            return true;
        }

        private void OnOpen(RecentProjectView view)
        {
            if (_eduProject.Open(view.ProjectInfo.Path) == false)
                return;

            if (TryAddView(view.ProjectInfo))
                Save();
        }

        private void OnRemove(RecentProjectView view)
        {
            _parent.Children.Remove(view.Panel);
            _viewList.Remove(view);
            view.Dispose();

            Save();
        }
    }
}