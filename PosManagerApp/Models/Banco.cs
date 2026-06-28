using System;
using System.Collections.Generic;
using System.Text;

namespace PosManagerApp.Models
{
    public class Banco : ObservableObject
    {
        private int id;
        private string codigoIdentificador = string.Empty;
        private string nombre = string.Empty;
        private string email = string.Empty;
        private string ciudad = string.Empty;
        private string fax = string.Empty;
        private string zip = string.Empty;
        private string pais = string.Empty;
        private string direccion = string.Empty;
        private string telefono = string.Empty;
        private string estadoPais = string.Empty;
        private bool activo;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public string CodigoIdentificador { get => codigoIdentificador; set => SetProperty(ref codigoIdentificador, value); }
        public string Nombre { get => nombre; set => SetProperty(ref nombre, value); }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Ciudad { get => ciudad; set => SetProperty(ref ciudad, value); }
        public string Fax { get => fax; set => SetProperty(ref fax, value); }
        public string Zip { get => zip; set => SetProperty(ref zip, value); }
        public string Pais { get => pais; set => SetProperty(ref pais, value); }
        public string Direccion { get => direccion; set => SetProperty(ref direccion, value); }
        public string Telefono { get => telefono; set => SetProperty(ref telefono, value); }
        public string EstadoPais { get => estadoPais; set => SetProperty(ref estadoPais, value); }
        public bool Activo { get => activo; set => SetProperty(ref activo, value); }
    }
}
