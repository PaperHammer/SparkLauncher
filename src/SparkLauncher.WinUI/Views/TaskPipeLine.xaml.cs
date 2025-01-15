using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SparkLauncher.WinUI.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SparkLauncher.WinUI.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskPipeLine : Page {
        public TaskPipeLine() {
            this.InitializeComponent();

            _viewModel = new TaskPipeLineViewModel();
            this.MainGrid.DataContext = _viewModel;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            _viewModel.Run();
        }

        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems != null) {
                foreach (var item in e.AddedItems) {
                    ListViewItem litem = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                    if (litem != null) {
                        var v = VisualStateManager.GoToState(litem, "Selected", true);
                    }
                }
            }
            if (e.RemovedItems != null) {
                foreach (var item in e.RemovedItems) {
                    ListViewItem litem = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                    if (litem != null) {
                        VisualStateManager.GoToState(litem, "UnSelected", true);
                    }
                }
            }
        }

        private readonly TaskPipeLineViewModel _viewModel;
    }
}
