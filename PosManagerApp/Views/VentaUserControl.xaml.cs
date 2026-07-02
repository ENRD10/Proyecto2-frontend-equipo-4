using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PosManagerApp.ViewModels;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PosManagerApp.Views
{
    /// <summary>
    /// Lógica de interacción para VentaUserControl.xaml
    /// </summary>
    public partial class VentaUserControl : UserControl
    {
        private readonly VentaViewModel _viewModel;

        public VentaUserControl()
        {
            InitializeComponent();

            _viewModel = new VentaViewModel();

            DataContext = _viewModel;

            Loaded += VentaUserControl_Loaded;
        }

        private async void VentaUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= VentaUserControl_Loaded;

            await _viewModel.LoadAsync();
        }
    }
}
