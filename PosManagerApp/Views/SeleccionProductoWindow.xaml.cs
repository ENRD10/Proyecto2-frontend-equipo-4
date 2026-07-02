using PosManagerApp.Models;
using PosManagerApp.ViewModels;
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
using System.Windows.Shapes;

namespace PosManagerApp.Views
{
    /// <summary>
    /// Lógica de interacción para SeleccionProductoWindow.xaml
    /// </summary>
    public partial class SeleccionProductoWindow : Window
    {
        public SeleccionProductoViewModel ViewModel { get; }

        public SeleccionProductoWindow(IEnumerable<Producto> productos)
        {
            InitializeComponent();

            ViewModel = new SeleccionProductoViewModel(productos);

            DataContext = ViewModel;

            ViewModel.CloseRequested += (_, dialogResult) =>
            {
                DialogResult = dialogResult;
                Close();
            };
        }

        private void Productos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel.AceptarCommand.CanExecute(null))
            {
                ViewModel.AceptarCommand.Execute(null);
            }
        }
    }
}
