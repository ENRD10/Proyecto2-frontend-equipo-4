using PosManagerApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace PosManagerApp.Services
{
    public class BancoService : ApiService
    {
        public async Task<List<Banco>> GetAll()
        {
            return await GetAsync<List<Banco>>("api/banco")
                   ?? new();
        }

        public async Task<Banco?> GetById(int id)
        {
            return await GetAsync<Banco>($"api/banco/{id}");
        }

        public async Task<Banco> Create(Banco banco)
        {
            return await PostAsync<Banco, Banco>("api/banco", banco);
        }

        public async Task<Banco> Update(Banco banco)
        {
            return await PutAsync<Banco, Banco>(
                $"api/banco/{banco.Id}",
                banco);
        }

        public async Task Delete(int id)
        {
            await DeleteAsync($"api/banco/{id}");
        }
    }
}
