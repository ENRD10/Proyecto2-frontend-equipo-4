using System;
using System.Collections.Generic;
using System.Text;

namespace PosManagerApp.Models
{
    public class Categoria : ObservableObject
    {
        private int idCategoria;
        private string nombreCategoria = string.Empty;

        public int CategoriaId { get => idCategoria; set => SetProperty(ref idCategoria, value); }
        public string NombreCategoria { get => nombreCategoria; set => SetProperty(ref nombreCategoria, value); }

        public Categoria Clone()
        {
            return new Categoria
            {
                CategoriaId = CategoriaId,
                NombreCategoria = NombreCategoria
            };
        }

        public override string ToString() { return NombreCategoria; }
    }
}
