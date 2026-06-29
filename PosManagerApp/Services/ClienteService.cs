using PosManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace PosManagerApp.Services
{
    public class ClienteService : ApiService
    {
        public async Task<List<Cliente>> GetAll()
        {
            return await GetAsync<List<Cliente>>("api/cliente")
                   ?? new();
        }

        public async Task<Cliente?> GetById(int id)
        {
            return await GetAsync<Cliente>($"api/cliente/{id}");
        }
    }
}
