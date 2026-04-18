
using System;
using StudyAndOrder.Wpf.ViewModels;
namespace StudyAndOrder.Wpf.ViewModels
{
    public class MainViewModel : BaseViewModel
    
    {
        private object _currentViewModel = new object();

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        private readonly OrdersViewModel _ordersViewModel;

        public MainViewModel(
            OrdersViewModel ordersViewModel,
            StudyInformationViewModel studyInformationViewModel,
            OrderDetailsViewModel orderDetailsViewModel)
        {
            _ordersViewModel = ordersViewModel;

            // Start på OrdersScreen
            CurrentViewModel = _ordersViewModel;

            // Created Study -> StudyInformationScreen
            _ordersViewModel.StudyInformationRequested += () =>
            {
                CurrentViewModel = studyInformationViewModel;
            };

            // Created order -> OrderDetailsScreen
            studyInformationViewModel.OrderDetailsRequested += (studyDbId, studyCode, orderDbId, orderNumber) =>
            {
                orderDetailsViewModel.SetContext(studyDbId, studyCode, orderDbId, orderNumber);
                CurrentViewModel = orderDetailsViewModel;
            };

            // Back to OrdersScreen efter Submit
            orderDetailsViewModel.BackToOrdersRequested += () =>
            {
                studyInformationViewModel.Reset();
                CurrentViewModel = _ordersViewModel;
            };
        }
    }
}