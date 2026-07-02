using PosManagerApp.Commands;
using PosManagerApp.Exceptions;
using PosManagerApp.Models;
using PosManagerApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Xml.Linq;

namespace PosManagerApp.ViewModels
{
    public class BancoViewModel : ViewModel
    {
        private readonly BancoService _service = new();

        private readonly List<Banco> _allBancos = new();

        public ObservableCollection<Banco> Bancos { get; } = new();

        #region Propiedades

        private Banco? _selectedBanco;

        public Banco? SelectedBanco
        {
            get => _selectedBanco;
            set
            {
                if (SetProperty(ref _selectedBanco, value) && value != null)
                {
                    BancoActual = value.Clone();
                }
            }
        }

        private Banco _bancoActual = new();

        public Banco BancoActual
        {
            get => _bancoActual;
            set
            {
                if (SetProperty(ref _bancoActual, value))
                {
                    OnPropertyChanged(nameof(IsEditing));
                    OnPropertyChanged(nameof(FormTitle));
                }
            }
        }

        private string _searchText = "";

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

        public bool IsEditing => BancoActual.Id != 0;

        public string FormTitle =>
            IsEditing
                ? "Editando banco"
                : "Nuevo banco";

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            private set => SetProperty(ref _isBusy, value);
        }

        #endregion

        #region Commands

        public AsyncRelayCommand SaveCommand { get; }

        public AsyncRelayCommand DeleteCommand { get; }

        public RelayCommand NewCommand { get; }

        public AsyncRelayCommand RefreshCommand { get; }

        #endregion

        public BancoViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveAsync);

            DeleteCommand = new AsyncRelayCommand(DeleteAsync);

            NewCommand = new RelayCommand(NewBanco);

            RefreshCommand = new AsyncRelayCommand(LoadAsync);
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var bancos = await _service.GetAll();

                _allBancos.Clear();
                _allBancos.AddRange(bancos);

                ActualizarColeccion(_allBancos);

                BancoActual = new Banco();
            }
            catch (ApiException ex)
            {
                HandleApiException(ex, "Error al cargar bancos");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error al cargar bancos");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SaveAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var isNew = BancoActual.Id == 0;
                if (isNew)
                    BancoActual = await _service.Create(BancoActual);
                else
                    BancoActual = await _service.Update(BancoActual);

                var action = isNew ? "creado" : "actualizado";
                MostrarOperacionExitosa(action);

                IsBusy = false;
                await LoadAsync();
            }
            catch (ApiException ex)
            {
                HandleApiException(ex, "Error al guardar");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error al guardar");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedBanco == null || IsBusy)
                return;

            var result = MessageBox.Show(
                $"¿Desea eliminar el banco '{SelectedBanco.Nombre}'?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            IsBusy = true;
            try
            {
                await _service.Delete(SelectedBanco.Id);

                MostrarOperacionExitosa("eliminado");

                IsBusy = false;
                await LoadAsync();
            }
            catch (ApiException ex)
            {
                HandleApiException(ex, "Error al eliminar");
            }
            catch (Exception ex)
            {
                HandleError(ex, "Error al eliminar");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void NewBanco()
        {
            SelectedBanco = null;

            BancoActual = new Banco()
            {
                Activo = true,
                Id = 0
            };
        }

        private void Filtrar()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ActualizarColeccion(_allBancos);
                return;
            }

            var texto = SearchText.ToLowerInvariant();

            var filtrados = _allBancos.Where(b =>

                (!string.IsNullOrWhiteSpace(b.Nombre) &&
                 b.Nombre.ToLowerInvariant().Contains(texto))

                ||

                (!string.IsNullOrWhiteSpace(b.CodigoIdentificador) &&
                 b.CodigoIdentificador.ToLowerInvariant().Contains(texto))

            );

            ActualizarColeccion(filtrados);
        }

        private void ActualizarColeccion(IEnumerable<Banco> bancos)
        {
            Bancos.Clear();

            foreach (var banco in bancos)
            {
                Bancos.Add(banco);
            }
        }

        private void MostrarOperacionExitosa(string action)
        {
            MessageBox.Show(
                $"El banco ID:{BancoActual.Id} ha sido {action} correctamente.",
                "Banco - Operación exitosa",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}