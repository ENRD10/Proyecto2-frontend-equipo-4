using System.Text;
using System.Windows;
using PosManagerApp.Views;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PosManagerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BancoUserControl _bancoView = new();
        private readonly VentaUserControl _ventaView = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Cambia la vista mostrada en el ContentControl
        private void Navigate(UserControl view, string pageTitle, Button activeButton)
        {
            MainContent.Content = view;

            PageTitle.Text = pageTitle;

            SetActiveButton(activeButton);
        }

        // Resalta el botón activo del menú lateral
        private void SetActiveButton(Button activeButton)
        {
            // Apagamos el estado en los botones normales
            BtnBancos.Tag = "Normal";
            BtnVentas.Tag = "Normal";

            // Encendemos el estado en el botón activo
            activeButton.Tag = "Selected";
        }

        private void Bancos_Click(object sender, RoutedEventArgs e)
        {
            Navigate(_bancoView, "Gestión de Bancos", BtnBancos);
        }

        private void Ventas_Click(object sender, RoutedEventArgs e)
        {
            Navigate(_ventaView, "Registrar Venta", BtnVentas);
        }

        private void Salir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}