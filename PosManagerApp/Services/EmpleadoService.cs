using PosManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace PosManagerApp.Services
{
    public class EmpleadoService : ApiService
    {
        public async Task<List<Empleado>> GetAll()
        {
            return await GetAsync<List<Empleado>>("api/empleado")
                   ?? new();
        }
    }
}
