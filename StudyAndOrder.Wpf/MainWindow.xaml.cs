using System.Windows;
using StudyAndOrder.Wpf.ViewModels;

namespace StudyAndOrder.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}