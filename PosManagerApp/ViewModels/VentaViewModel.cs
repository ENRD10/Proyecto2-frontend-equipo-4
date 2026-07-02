using PosManagerApp.Commands;
using PosManagerApp.Exceptions;
using PosManagerApp.Models;
using PosManagerApp.Models.DTOs;
using PosManagerApp.Services;
using PosManagerApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PosManagerApp.ViewModels
{
    public class VentaViewModel : ViewModel
    {
        #region Services

        private readonly VentaService _ventaService = new();
        private readonly ClienteService _clienteService = new();
        private readonly EmpleadoService _empleadoService = new();
        private readonly ProductoService _productoService = new();

        #endregion

        #region Collections

        public ObservableCollection<Cliente> Clientes { get; } = new();
        public ObservableCollection<Empleado> Empleados { get; } = new();
        public ObservableCollection<Producto> Productos { get; } = new();
        public ObservableCollection<DetalleVentaItem> Detalles { get; } = new();
        public ICollectionView ClientesView { get; }
        public ICollectionView EmpleadosView { get; }

        #endregion

        #region Propiedades

        private Cliente? _clienteSeleccionado;
        public Cliente? ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set => SetProperty(ref _clienteSeleccionado, value);
        }

        private Empleado? _empleadoSeleccionado;
        public Empleado? EmpleadoSeleccionado
        {
            get => _empleadoSeleccionado;
            set => SetProperty(ref _empleadoSeleccionado, value);
        }

        private string _buscarCliente = "";
        public string BuscarCliente
        {
            get => _buscarCliente;
            set
            {
                if (SetProperty(ref _buscarCliente, value))
                {
                    ClientesView.Refresh();
                }
            }
        }

        private string _buscarEmpleado = "";
        public string BuscarEmpleado
        {
            get => _buscarEmpleado;
            set
            {
                if (SetProperty(ref _buscarEmpleado, value))
                {
                    EmpleadosView.Refresh();
                }
            }
        }

        private DateTime _fechaVenta = DateTime.Today;
        public DateTime FechaVenta
        {
            get => _fechaVenta;
            set => SetProperty(ref _fechaVenta, value);
        }

        private DetalleVentaItem? _detalleSeleccionado;
        public DetalleVentaItem? DetalleSeleccionado
        {
            get => _detalleSeleccionado;
            set => SetProperty(ref _detalleSeleccionado, value);
        }

        #endregion

        #region Resumen

        public decimal Subtotal =>
            Detalles.Sum(d => d.PrecioSubtotal);
        public decimal TotalImpuestos =>
            Detalles.Sum(d => d.MontoImpuesto);
        public decimal TotalVenta =>
            Detalles.Sum(d => d.PrecioTotal);

        #endregion

        #region Commands

        public AsyncRelayCommand RegistrarVentaCommand { get; }
        public RelayCommand AgregarProductoCommand { get; }
        public RelayCommand EliminarProductoCommand { get; }

        #endregion

        public VentaViewModel()
        {
            RegistrarVentaCommand = new AsyncRelayCommand(RegistrarVentaAsync);

            AgregarProductoCommand = new RelayCommand(AgregarProducto);

            EliminarProductoCommand = new RelayCommand(EliminarProducto);

            ClientesView = CollectionViewSource.GetDefaultView(Clientes);

            ClientesView.Filter = FiltrarClientes;

            EmpleadosView = CollectionViewSource.GetDefaultView(Empleados);

            EmpleadosView.Filter = FiltrarEmpleados;
        }

        public async Task LoadAsync()
        {
            try
            {
                ActualizarColeccion(Clientes, await _clienteService.GetAll());

                ActualizarColeccion(Empleados, await _empleadoService.GetAll());

                ActualizarColeccion(Productos, await _productoService.GetAll());

                LimpiarVenta();
            }
            catch (ApiException ex)
            {
                HandleApiException(ex, "Error al cargar los datos");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error al cargar los datos");
            }
        }

        private void AgregarProducto()
        {
            var ventana = new SeleccionProductoWindow(Productos);
            if (ventana.ShowDialog() != true) return;

            var vm = ventana.ViewModel;
            if (vm.ProductoSeleccionado == null) return;

            var detalle = new DetalleVentaItem
            {
                Producto = vm.ProductoSeleccionado,
                Cantidad = vm.Cantidad
            };

            // Si ya existe el producto solamente aumenta la cantidad
            var existente = Detalles.FirstOrDefault(d =>
                d.Producto?.ProductId == detalle.Producto.ProductId);

            if (existente != null) existente.Cantidad += detalle.Cantidad;
            else
            {
                SuscribirDetalle(detalle);
                Detalles.Add(detalle);
            }

            ActualizarResumen();
        }

        private void EliminarProducto()
        {
            if (DetalleSeleccionado == null)
                return;

            Detalles.Remove(DetalleSeleccionado);

            ActualizarResumen();
        }

        private async Task RegistrarVentaAsync()
        {
            if (ClienteSeleccionado == null)
            {
                MostrarAdvertencia("Debe seleccionar un cliente para registrar la venta.");
                return;
            }

            if (EmpleadoSeleccionado == null)
            {
                MostrarAdvertencia("Debe seleccionar un empleado para registrar la venta.");
                return;
            }

            if (Detalles.Count == 0)
            {
                MostrarAdvertencia("Debe agregar al menos un producto.");
                return;
            }

            try
            {
                var request = new CreateVentaRequest
                {
                    IdCliente = ClienteSeleccionado.Id,
                    IdEmpleado = EmpleadoSeleccionado.Id,
                    FechaOrden = FechaVenta,
                    Impuesto = TotalImpuestos,
                    Detalles = Detalles
                        .Select(d => d.ToRequest())
                        .ToList()
                };

                var response = await _ventaService.Create(request);

                MessageBox.Show(
                    $"La venta fue registrada correctamente.\n\nNúmero de orden: {response.NumeroOrden}",
                    "Venta",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                LimpiarVenta();
            }
            catch (ApiException ex)
            {
                HandleApiException(ex, "Error al registrar la venta");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error al registrar la venta");
            }
        }

        private void LimpiarVenta()
        {
            ClienteSeleccionado = null;

            EmpleadoSeleccionado = null;

            FechaVenta = DateTime.Today;

            Detalles.Clear();

            ActualizarResumen();
        }

        private void ActualizarColeccion<T>(
            ObservableCollection<T> destino,
            IEnumerable<T> origen)
        {
            destino.Clear();

            foreach (var item in origen)
                destino.Add(item);
        }

        private void SuscribirDetalle(DetalleVentaItem detalle)
        {
            detalle.PropertyChanged += Detalle_PropertyChanged;
        }

        private void Detalle_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ActualizarResumen();
        }

        private void ActualizarResumen()
        {
            OnPropertyChanged(nameof(Subtotal));
            OnPropertyChanged(nameof(TotalImpuestos));
            OnPropertyChanged(nameof(TotalVenta));
        }

        private static void MostrarAdvertencia(String mensaje)
        {
            MessageBox.Show(mensaje, "Venta", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private bool FiltrarClientes(object obj)
        {
            if (obj is not Cliente cliente)
                return false;

            if (string.IsNullOrWhiteSpace(BuscarCliente))
                return true;

            string texto = BuscarCliente.ToLower();

            return
                cliente.NombreCompleto.ToLower().Contains(texto)
                ||
                cliente.NumeroIdentificacion.ToLower().Contains(texto);
        }

        private bool FiltrarEmpleados(object obj)
        {
            if (obj is not Empleado empleado)
                return false;

            if (string.IsNullOrWhiteSpace(BuscarEmpleado))
                return true;

            string texto = BuscarEmpleado.ToLower();

            return
                empleado.NombreCompleto.ToLower().Contains(texto)
                ||
                empleado.NumeroIdentificacion.ToLower().Contains(texto);
        }
    }
}
