using System;
using System.Collections.Generic;
using System.Text;

namespace PosManagerApp.Models
{
    public class Producto : ObservableObject
    {
        private int productId;
        private string nombreEtiqueta = string.Empty;
        private string descripcion = string.Empty;

        private int existenciaEnStock;
        private int existenciaLimiteParaAlmacenar;

        private decimal precioNeto;
        private decimal precioMinimo;
        private decimal precioMinimoConImpuesto;

        private DateTime fechaCreacion;

        private bool puedeVenderse;
        private bool puedeComprarse;

        private decimal impuestoValorAgregado;
        private decimal impuestoLocal1;
        private decimal impuestoLocal2;

        private bool tieneImpuesto;

        private string codigoBarra = string.Empty;
        private string imagen = string.Empty;

        private string codigoContableVentas = string.Empty;
        private string codigoContableCompras = string.Empty;

        private decimal peso;
        private string unidadPeso = string.Empty;

        private decimal longitud;
        private string unidadLongitud = string.Empty;

        private decimal superficie;
        private string unidadSuperficie = string.Empty;

        private decimal volumen;
        private string unidadVolumen = string.Empty;

        private List<Categoria> categorias = new();

        public int ProductId { get => productId; set => SetProperty(ref productId, value); }
        public string NombreEtiqueta { get => nombreEtiqueta; set => SetProperty(ref nombreEtiqueta, value); }
        public string Descripcion { get => descripcion; set => SetProperty(ref descripcion, value); }
        public int ExistenciaEnStock { get => existenciaEnStock; set => SetProperty(ref existenciaEnStock, value); }
        public int ExistenciaLimiteParaAlmacenar { get => existenciaLimiteParaAlmacenar; set => SetProperty(ref existenciaLimiteParaAlmacenar, value); }
        public decimal PrecioNeto { get => precioNeto; set => SetProperty(ref precioNeto, value); }
        public decimal PrecioMinimo { get => precioMinimo; set => SetProperty(ref precioMinimo, value); }
        public decimal PrecioMinimoConImpuesto { get => precioMinimoConImpuesto; set => SetProperty(ref precioMinimoConImpuesto, value); }
        public DateTime FechaCreacion { get => fechaCreacion; set => SetProperty(ref fechaCreacion, value); }
        public bool PuedeVenderse { get => puedeVenderse; set => SetProperty(ref puedeVenderse, value); }
        public bool PuedeComprarse { get => puedeComprarse; set => SetProperty(ref puedeComprarse, value); }
        public decimal ImpuestoValorAgregado { get => impuestoValorAgregado; set => SetProperty(ref impuestoValorAgregado, value); }
        public decimal ImpuestoLocal1 { get => impuestoLocal1; set => SetProperty(ref impuestoLocal1, value); }
        public decimal ImpuestoLocal2 { get => impuestoLocal2; set => SetProperty(ref impuestoLocal2, value); }
        public bool TieneImpuesto { get => tieneImpuesto; set => SetProperty(ref tieneImpuesto, value); }
        public string CodigoBarra { get => codigoBarra; set => SetProperty(ref codigoBarra, value); }
        public string Imagen { get => imagen; set => SetProperty(ref imagen, value); }
        public string CodigoContableVentas { get => codigoContableVentas; set => SetProperty(ref codigoContableVentas, value); }
        public string CodigoContableCompras { get => codigoContableCompras; set => SetProperty(ref codigoContableCompras, value); }
        public decimal Peso { get => peso; set => SetProperty(ref peso, value); }
        public string UnidadPeso { get => unidadPeso; set => SetProperty(ref unidadPeso, value); }
        public decimal Longitud { get => longitud; set => SetProperty(ref longitud, value); }
        public string UnidadLongitud { get => unidadLongitud; set => SetProperty(ref unidadLongitud, value); }
        public decimal Superficie { get => superficie; set => SetProperty(ref superficie, value); }
        public string UnidadSuperficie { get => unidadSuperficie; set => SetProperty(ref unidadSuperficie, value); }
        public decimal Volumen { get => volumen; set => SetProperty(ref volumen, value); }
        public string UnidadVolumen { get => unidadVolumen; set => SetProperty(ref unidadVolumen, value); }
        public List<Categoria> Categorias { get => categorias; set => SetProperty(ref categorias, value); }

        public Producto Clone()
        {
            return new Producto
            {
                ProductId = ProductId,
                NombreEtiqueta = NombreEtiqueta,
                Descripcion = Descripcion,
                ExistenciaEnStock = ExistenciaEnStock,
                ExistenciaLimiteParaAlmacenar = ExistenciaLimiteParaAlmacenar,
                PrecioNeto = PrecioNeto,
                PrecioMinimo = PrecioMinimo,
                PrecioMinimoConImpuesto = PrecioMinimoConImpuesto,
                FechaCreacion = FechaCreacion,
                PuedeVenderse = PuedeVenderse,
                PuedeComprarse = PuedeComprarse,
                ImpuestoValorAgregado = ImpuestoValorAgregado,
                ImpuestoLocal1 = ImpuestoLocal1,
                ImpuestoLocal2 = ImpuestoLocal2,
                TieneImpuesto = TieneImpuesto,
                CodigoBarra = CodigoBarra,
                Imagen = Imagen,
                CodigoContableVentas = CodigoContableVentas,
                CodigoContableCompras = CodigoContableCompras,
                Peso = Peso,
                UnidadPeso = UnidadPeso,
                Longitud = Longitud,
                UnidadLongitud = UnidadLongitud,
                Superficie = Superficie,
                UnidadSuperficie = UnidadSuperficie,
                Volumen = Volumen,
                UnidadVolumen = UnidadVolumen,
                Categorias = Categorias?.Select(c => c.Clone()).ToList() ?? new List<Categoria>()
            };
        }

        // Propiedades calculadas
        public decimal PorcentajeImpuesto =>
            TieneImpuesto
                ? ImpuestoValorAgregado + ImpuestoLocal1 + ImpuestoLocal2
                : 0m;

        public decimal PrecioConImpuesto =>
            PrecioNeto * (1 + (PorcentajeImpuesto / 100));

        public bool HayExistencias => ExistenciaEnStock > 0;

        public override string ToString() => NombreEtiqueta;
    }
}
