using PosManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace PosManagerApp.Services
{
    public class ProductoService : ApiService
    {
        public async Task<List<Producto>> GetAll()
        {
            return await GetAsync<List<Producto>>("api/producto")
                   ?? new();
        }
    }
}
