using PosManagerApp.Commands;
using PosManagerApp.Models;
using System.Collections.ObjectModel;

namespace PosManagerApp.ViewModels
{
    public class SeleccionProductoViewModel : ObservableObject
    {
        private readonly List<Producto> _todosLosProductos;

        public ObservableCollection<Producto> Productos { get; } = new();

        #region Propiedades

        private Producto? _productoSeleccionado;

        public Producto? ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                if (SetProperty(ref _productoSeleccionado, value))
                {
                    AceptarCommand?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    Filtrar();
                }
            }
        }

        private int _cantidad = 1;

        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value <= 0)
                    value = 1;

                if (SetProperty(ref _cantidad, value))
                {
                    AceptarCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand AceptarCommand { get; }

        public RelayCommand CancelarCommand { get; }

        #endregion

        #region Eventos

        public event EventHandler<bool?>? CloseRequested;

        #endregion

        public SeleccionProductoViewModel(IEnumerable<Producto> productos)
        {
            _todosLosProductos = productos.ToList();

            ActualizarColeccion(_todosLosProductos);

            ProductoSeleccionado = Productos.FirstOrDefault();

            AceptarCommand = new RelayCommand(
                Aceptar,
                PuedeAceptar);

            CancelarCommand = new RelayCommand(Cancelar);
        }

        #region Métodos privados

        private bool PuedeAceptar()
        {
            return ProductoSeleccionado != null
                   && Cantidad > 0;
        }

        private void Aceptar()
        {
            if (ProductoSeleccionado == null)
                return;

            CloseRequested?.Invoke(this, true);
        }

        private void Cancelar()
        {
            CloseRequested?.Invoke(this, false);
        }

        private void Filtrar()
        {
            IEnumerable<Producto> resultado;

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                resultado = _todosLosProductos;
            }
            else
            {
                string texto = SearchText.ToLower();

                resultado = _todosLosProductos.Where(p =>

                    (!string.IsNullOrWhiteSpace(p.NombreEtiqueta) &&
                     p.NombreEtiqueta.ToLower().Contains(texto))

                    ||

                    (!string.IsNullOrWhiteSpace(p.CodigoBarra) &&
                     p.CodigoBarra.ToLower().Contains(texto))
                );
            }

            ActualizarColeccion(resultado);

            ProductoSeleccionado = Productos.FirstOrDefault();
        }

        private void ActualizarColeccion(IEnumerable<Producto> productos)
        {
            Productos.Clear();

            foreach (var producto in productos)
            {
                Productos.Add(producto);
            }
        }

        #endregion
    }
}