using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EduEngineLauncher
{
    internal class RecentProjectView : IDisposable
    {
        public readonly ProjectInfo ProjectInfo;

        private readonly Action<RecentProjectView> _onOpen;
        private readonly Action<RecentProjectView> _onRemove;

        private StackPanel _panel;
        private Button _openButton;
        private Button _removeButton;

        public RecentProjectView(ProjectInfo projectInfo, Action<RecentProjectView> onOpen, Action<RecentProjectView> onRemove)
        {
            ProjectInfo = projectInfo;
            _onOpen = onOpen;
            _onRemove = onRemove;
        }

        public StackPanel Panel => _panel;

        public StackPanel Build()
        {
            _panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 5, 10, 5)
            };

            StackPanel textStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 0, 10, 0)
            };

            TextBlock projectTitle = new TextBlock
            {
                Text = Path.GetFileNameWithoutExtension(ProjectInfo.Path),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 5),
                FontSize = 16,
                FontWeight = FontWeights.Bold,
            };
            textStackPanel.Children.Add(projectTitle);

            TextBox projectPath = new TextBox
            {
                Text = ProjectInfo.Path,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 400,
                Margin = new Thickness(5, 0, 0, 5),
                IsReadOnly = true,
                FontSize = 10,
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
            };
            textStackPanel.Children.Add(projectPath);

            TextBlock lastOpenDate = new TextBlock
            {
                Text = "Last open: " + new DateTime(ProjectInfo.Date).ToString(),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 5),
                FontSize = 10,
            };
            textStackPanel.Children.Add(lastOpenDate);

            Grid buttonGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Right
            };
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition());
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition());

            _openButton = new Button
            {
                Content = "Open",
                Width = 50,
                Height = 30,
                Margin = new Thickness(5),
            };
            _openButton.Click += OnOpenButtonClick;
            Grid.SetColumn(_openButton, 0);
            buttonGrid.Children.Add(_openButton);

            _removeButton = new Button
            {
                Content = "Remove",
                Width = 50,
                Height = 30,
                Margin = new Thickness(5),
            };
            _removeButton.Click += OnRemoveButtonClick;
            Grid.SetColumn(_removeButton, 1);
            buttonGrid.Children.Add(_removeButton);

            _panel.Children.Add(textStackPanel);
            _panel.Children.Add(buttonGrid);
            _panel.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));

            return _panel;
        }

        public void Dispose()
        {
            if (_openButton != null)
                _openButton.Click -= OnOpenButtonClick;

            if (_removeButton != null)
                _removeButton.Click -= OnOpenButtonClick;
        }

        private void OnOpenButtonClick(object sender, RoutedEventArgs e)
        {
            _onOpen?.Invoke(this);
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            _onRemove?.Invoke(this);
        }
    }
}