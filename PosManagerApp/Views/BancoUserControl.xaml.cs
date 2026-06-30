using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PosManagerApp.ViewModels;

namespace PosManagerApp.Views
{
    /// <summary>
    /// Lógica de interacción para BancoUserControl.xaml
    /// </summary>
    public partial class BancoUserControl : UserControl
    {
        private readonly BancoViewModel _viewModel;

        public BancoUserControl()
        {
            InitializeComponent();

            _viewModel = new BancoViewModel();

            DataContext = _viewModel;

            Loaded += BancoUserControl_Loaded;
        }

        private async void BancoUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= BancoUserControl_Loaded;

            await _viewModel.LoadAsync();
        }
    }
}
