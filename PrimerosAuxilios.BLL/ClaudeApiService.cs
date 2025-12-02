using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimerosAuxilios.BLL
{
    /// <summary>
    /// Servicio para interacción con la API de Claude
    /// </summary>
    public class ClaudeApiService
    {
        private readonly HttpClient _httpClient;
        private const string API_KEY = "";
        private const string API_URL = "";

        public ClaudeApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", API_KEY);
            _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        }

        /// <summary>
        /// Analiza una foto de emergencia médica
        /// </summary>
        public async Task<string> AnalyzeEmergencyPhoto(byte[] imageBytes, string imageExtension)
        {
            try
            {
                if (API_KEY == "TU_API_KEY_AQUI")
                {
                    throw new Exception("⚠️ Configura tu API Key en ClaudeApiService.cs");
                }

                string base64Image = Convert.ToBase64String(imageBytes);
                string mediaType = imageExtension.ToLower().Contains("png") ? "image/png" : "image/jpeg";

                var requestBody = new
                {
                    model = "claude-sonnet-4-20250514",
                    max_tokens = 600,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new
                                {
                                    type = "image",
                                    source = new
                                    {
                                        type = "base64",
                                        media_type = mediaType,
                                        data = base64Image
                                    }
                                },
                                new
                                {
                                    type = "text",
                                    text = @"Analiza esta lesión y responde en MÁXIMO 100 PALABRAS:

                                        QUÉ ES:
                                        [Tipo de lesión en 1 línea]

                                        QUÉ HACER AHORA:
                                        1. [Acción inmediata]
                                        2. [Segunda acción]
                                        3. [Tercera acción]

                                        ¿IR AL HOSPITAL?
                                        SÍ/NO - [razón en 1 línea]

                                        SÉ BREVE. Emergencia real. Español."
                                }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(API_URL, content);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new Exception("❌ API Key inválida. Verifica en console.anthropic.com");
                    }

                    throw new Exception($"Error API: {response.StatusCode}");
                }

                var responseText = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseText);

                return jsonDoc.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Analiza texto de emergencia médica
        /// </summary>
        public async Task<string> AnalyzeEmergencyText(string userMessage)
        {
            try
            {
                if (API_KEY == "TU_API_KEY_AQUI")
                {
                    throw new Exception("⚠️ Configura tu API Key en ClaudeApiService.cs");
                }

                var requestBody = new
                {
                    model = "claude-sonnet-4-20250514",
                    max_tokens = 500,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            content = userMessage + @"

                                Responde en MÁXIMO 90 PALABRAS:

                                DIAGNÓSTICO POSIBLE:
                                [1 línea]

                                QUÉ HACER:
                                • [Acción 1]
                                • [Acción 2]
                                • [Acción 3]

                                ¿HOSPITAL?
                                SÍ/NO - [razón breve]

                                BREVE. Directo. Español.

                                ⚠️ Esto NO reemplaza al médico."
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(API_URL, content);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new Exception("❌ API Key inválida");
                    }

                    throw new Exception($"Error API: {response.StatusCode}");
                }

                var responseText = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(responseText);

                return jsonDoc.RootElement
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}", ex);
            }
        }
    }
}