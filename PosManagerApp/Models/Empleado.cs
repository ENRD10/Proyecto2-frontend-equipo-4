using System;
using System.Collections.Generic;
using System.Text;

namespace PosManagerApp.Models
{
    public class Empleado : ObservableObject
    {
        private int id;
        private string nombre = string.Empty;
        private string apellidos = string.Empty;
        private string numeroIdentificacion = string.Empty;
        private string loginName = string.Empty;
        private string password = string.Empty;
        private string passwordEncriptada = string.Empty;
        private string direccion = string.Empty;
        private string ciudad = string.Empty;
        private string nacionalidad = string.Empty;
        private string email = string.Empty;
        private string photo = string.Empty;
        private bool activo;

        private string estadoCivil = string.Empty;
        private string telefonoOficina = string.Empty;
        private string telefonoMovil = string.Empty;
        private string passwordTemporal = string.Empty;
        private string numeroCuentaBancaria = string.Empty;
        private string nombreBancoCuenta = string.Empty;

        public int Id { get => id; set => SetProperty(ref id, value); }
        public string Nombre { get => nombre; set => SetProperty(ref nombre, value); }
        public string Apellidos { get => apellidos; set => SetProperty(ref apellidos, value); }
        public string NumeroIdentificacion { get => numeroIdentificacion; set => SetProperty(ref numeroIdentificacion, value); }
        public string LoginName { get => loginName; set => SetProperty(ref loginName, value); }
        public string Password { get => password; set => SetProperty(ref password, value); }
        public string PasswordEncriptada { get => passwordEncriptada; set => SetProperty(ref passwordEncriptada, value); }
        public string Direccion { get => direccion; set => SetProperty(ref direccion, value); }
        public string Ciudad { get => ciudad; set => SetProperty(ref ciudad, value); }
        public string Nacionalidad { get => nacionalidad; set => SetProperty(ref nacionalidad, value); }
        public string Email { get => email; set => SetProperty(ref email, value); }
        public string Photo { get => photo; set => SetProperty(ref photo, value); }
        public bool Activo { get => activo; set => SetProperty(ref activo, value); }
        public string EstadoCivil { get => estadoCivil; set => SetProperty(ref estadoCivil, value); }
        public string TelefonoOficina { get => telefonoOficina; set => SetProperty(ref telefonoOficina, value); }
        public string TelefonoMovil { get => telefonoMovil; set => SetProperty(ref telefonoMovil, value); }
        public string PasswordTemporal { get => passwordTemporal; set => SetProperty(ref passwordTemporal, value); }
        public string NumeroCuentaBancaria { get => numeroCuentaBancaria; set => SetProperty(ref numeroCuentaBancaria, value); }
        public string NombreBancoCuenta { get => nombreBancoCuenta; set => SetProperty(ref nombreBancoCuenta, value); }
        
        public string NombreCompleto => $"{Nombre} {Apellidos}";
        public override string ToString() => NombreCompleto;
    }
}
