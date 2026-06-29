using PosManagerApp.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace PosManagerApp.Services
{
    public class VentaService : ApiService
    {
        public async Task<List<VentaResponse>> GetAll()
        {
            return await GetAsync<List<VentaResponse>>("api/venta")
                   ?? new();
        }

        public async Task<VentaResponse?> GetById(int numeroOrden)
        {
            return await GetAsync<VentaResponse>($"api/venta/{numeroOrden}");
        }

        public async Task<CreateVentaResponse> Create(CreateVentaRequest request)
        {
            return await PostAsync<CreateVentaRequest,
                CreateVentaResponse>(
                "api/venta",
                request);
        }
    }
}
