using PosManagerApp.Exceptions;
using PosManagerApp.Models;
using PosManagerApp.Services;
using PosManagerApp.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace PosManagerApp.ViewModels
{
    public class BancoViewModel : ObservableObject
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

        #endregion

        #region Commands

        public AsyncRelayCommand SaveCommand { get; }

        public AsyncRelayCommand DeleteCommand { get; }

        public RelayCommand NewCommand { get; }

        #endregion

        public BancoViewModel()
        {
            SaveCommand = new AsyncRelayCommand(SaveAsync);

            DeleteCommand = new AsyncRelayCommand(DeleteAsync);

            NewCommand = new RelayCommand(NewBanco);
        }

        public async Task LoadAsync()
        {
            var bancos = await _service.GetAll();

            _allBancos.Clear();
            _allBancos.AddRange(bancos);

            ActualizarColeccion(_allBancos);

            BancoActual = new Banco();
        }

        private async Task SaveAsync()
        {
            try
            {
                if (BancoActual.Id == 0)
                {
                    await _service.Create(BancoActual);
                }
                else
                {
                    await _service.Update(BancoActual);
                }

                await LoadAsync();

                MessageBox.Show(
                    "La información fue guardada correctamente.",
                    "Banco",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (ApiException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task DeleteAsync()
        {
            if (SelectedBanco == null)
                return;

            var result = MessageBox.Show(
                $"¿Desea eliminar el banco '{SelectedBanco.Nombre}'?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                await _service.Delete(SelectedBanco.Id);

                await LoadAsync();
            }
            catch (ApiException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void NewBanco()
        {
            SelectedBanco = null;

            BancoActual = new Banco()
            {
                Activo = true
            };
        }

        private void Filtrar()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                ActualizarColeccion(_allBancos);
                return;
            }

            var texto = SearchText.ToLower();

            var filtrados = _allBancos.Where(b =>

                (!string.IsNullOrWhiteSpace(b.Nombre) &&
                 b.Nombre.ToLower().Contains(texto))

                ||

                (!string.IsNullOrWhiteSpace(b.CodigoIdentificador) &&
                 b.CodigoIdentificador.ToLower().Contains(texto))

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
    }
}