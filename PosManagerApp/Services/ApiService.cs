using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace PosManagerApp.Services
{
    public abstract class ApiService
    {
        protected static readonly HttpClient Client = new();

        protected ApiService()
        {
            if (Client.BaseAddress == null)
            {
                string? baseUrl = AppConfig.GetApiBaseUrl();

                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("No se configuró la URL de la API.");

                Client.BaseAddress = new Uri(baseUrl);
            }
        }

        protected async Task<T?> GetAsync<T>(string endpoint)
        {
            return await Client.GetFromJsonAsync<T>(endpoint);
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest data)
        {
            var response = await Client.PostAsJsonAsync(endpoint, data);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>()
                   ?? throw new Exception("La API devolvió una respuesta vacía.");
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(
            string endpoint,
            TRequest data)
        {
            var response = await Client.PutAsJsonAsync(endpoint, data);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TResponse>()
                   ?? throw new Exception("La API devolvió una respuesta vacía.");
        }

        protected async Task DeleteAsync(string endpoint)
        {
            var response = await Client.DeleteAsync(endpoint);

            response.EnsureSuccessStatusCode();
        }
    }
}
