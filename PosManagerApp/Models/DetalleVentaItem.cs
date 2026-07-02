using PosManagerApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PosManagerApp.Models
{
    public class DetalleVentaItem : ObservableObject
    {
        private Producto? _producto;
        private int _cantidad = 1;
        private decimal _descuento;
        private decimal _precioUnitario;
        private decimal _impuesto;

        public Producto? Producto
        {
            get => _producto;
            set
            {
                if (SetProperty(ref _producto, value))
                {
                    if (value != null)
                    {
                        PrecioUnitario = value.PrecioNeto;
                        Impuesto = value.PorcentajeImpuesto;
                    }

                    OnPropertyChanged(nameof(NombreProducto));
                    OnPropertyChanged(nameof(CodigoProducto));
                }
            }
        }

        public string NombreProducto => Producto?.NombreEtiqueta ?? string.Empty;
        public string CodigoProducto => Producto?.CodigoBarra ?? string.Empty;

        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value <= 0) value = 1;
                if (SetProperty(ref _cantidad, value))
                    NotificarCalculos();
            }
        }


        // Porcentaje de descuento (ej. 10 = 10%)
        public decimal Descuento
        {
            get => _descuento;
            set
            {
                if (SetProperty(ref _descuento, value))
                    NotificarCalculos();
            }
        }

        public decimal PrecioUnitario
        {
            get => _precioUnitario;
            set
            {
                if (SetProperty(ref _precioUnitario, value))
                    NotificarCalculos();
            }
        }

        // Porcentaje de impuesto (ej. 13 = 13%)
        public decimal Impuesto
        {
            get => _impuesto;
            set
            {
                if (SetProperty(ref _impuesto, value))
                    NotificarCalculos();
            }
        }

        public decimal PrecioSubtotal
        {
            get
            {
                decimal subtotal = Cantidad * PrecioUnitario;
                decimal montoDescuento = subtotal * (Descuento / 100m);

                return subtotal - montoDescuento;
            }
        }

        public decimal MontoImpuesto => PrecioSubtotal * (Impuesto / 100m);

        public decimal PrecioTotal => PrecioSubtotal + MontoImpuesto;

        private void NotificarCalculos()
        {
            OnPropertyChanged(nameof(PrecioSubtotal));
            OnPropertyChanged(nameof(MontoImpuesto));
            OnPropertyChanged(nameof(PrecioTotal));
        }

        public CreateDetalleVentaRequest ToRequest()
        {
            if (Producto == null)
                throw new InvalidOperationException("La línea no posee un producto.");

            return new CreateDetalleVentaRequest
            {
                IdProducto = Producto.ProductId,
                Cantidad = Cantidad,
                PrecioUnitario = PrecioUnitario,
                Descuento = Descuento,
                Impuesto = Impuesto
            };
        }
    }
}
