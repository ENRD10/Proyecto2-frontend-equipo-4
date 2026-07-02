using PosManagerApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PosManagerApp.Services
{
    public abstract class ApiService
    {
        protected static readonly HttpClient Client = new();
        private readonly JsonSerializerOptions _jsonOptions;

        protected ApiService()
        {
            if (Client.BaseAddress == null)
            {
                string? baseUrl = AppConfig.GetApiBaseUrl();

                if (string.IsNullOrWhiteSpace(baseUrl))
                    throw new InvalidOperationException("No se configuró la URL de la API.");

                Client.BaseAddress = new Uri(baseUrl);
            }
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        protected Task<T?> GetAsync<T>(string endpoint)
        {
            return SendAndDeserializeAsync<T>(() => Client.GetAsync(endpoint), allowEmpty: true);
        }

        protected async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var result = await SendAndDeserializeAsync<TResponse>(() => Client.PostAsJsonAsync(endpoint, data), allowEmpty: false);
            return result ?? throw new ApiException("La API devolvió una respuesta nula o con formato inesperado para la operación POST.", null, null);
        }

        protected async Task<TResponse> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var result = await SendAndDeserializeAsync<TResponse>(() => Client.PutAsJsonAsync(endpoint, data), allowEmpty: false);
            return result ?? throw new ApiException("La API devolvió una respuesta nula o con formato inesperado para la operación PUT.", null, null);
        }

        protected async Task DeleteAsync(string endpoint)
        {
            await SendAndDeserializeAsync<object>(() => Client.DeleteAsync(endpoint), allowEmpty: true);
        }

        private async Task<T?> SendAndDeserializeAsync<T>(Func<Task<HttpResponseMessage>> httpAction, bool allowEmpty)
        {
            try
            {
                using var response = await httpAction();

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new ApiException($"La API respondió con error {(int)response.StatusCode}.", (int)response.StatusCode, errorContent);
                }

                using var stream = await response.Content.ReadAsStreamAsync();
                if (stream.Length == 0 || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    if (allowEmpty) return default;
                    throw new ApiException("La API devolvió una respuesta vacía.", (int)response.StatusCode, null);
                }

                try
                {
                    var result = JsonSerializer.Deserialize<T>(stream,_jsonOptions);
                    return result ?? throw new ApiException("La API devolvió una respuesta con formato inesperado.", (int)response.StatusCode, null);
                }
                catch (JsonException ex)
                {
                    throw new ApiException("Error al deserializar la respuesta JSON.", (int)response.StatusCode, null, ex);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException("Error de red al conectar con la API.", null, null, ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new ApiException("La solicitud a la API fue cancelada o el tiempo de espera se agotó.", null, null, ex);
            }
        }
    }
}
