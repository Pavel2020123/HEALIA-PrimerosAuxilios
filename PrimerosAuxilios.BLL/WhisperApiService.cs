using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrimerosAuxilios.BLL
{
    /// <summary>
    /// Servicio para interacción con la API de Whisper (Azure OpenAI)
    /// </summary>
    public class WhisperApiService
    {
        private readonly HttpClient _httpClient;
        private const string AZURE_API_KEY = "";
        private const string WHISPER_API_URL = "";

        public WhisperApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", AZURE_API_KEY);
        }

        /// <summary>
        /// Transcribe audio a texto usando Whisper de Azure OpenAI
        /// </summary>
        public async Task<string> TranscribeAudio(byte[] audioData)
        {
            try
            {
                if (AZURE_API_KEY == "TU_AZURE_API_KEY_AQUI")
                {
                    throw new Exception("⚠️ Configura tu Azure API Key en WhisperApiService.cs");
                }

                using (var content = new MultipartFormDataContent())
                {
                    var audioContent = new ByteArrayContent(audioData);
                    audioContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                    content.Add(audioContent, "file", "audio.wav");

                    content.Add(new StringContent("es"), "language");

                    content.Add(new StringContent(
                        "Transcripción de emergencia médica. Palabras: dolor, herida, sangre, quemadura, fractura, alergia, medicamento, síntomas, hospital, ambulancia."
                    ), "prompt");

                    content.Add(new StringContent("0"), "temperature");

                    var response = await _httpClient.PostAsync(WHISPER_API_URL, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        var error = await response.Content.ReadAsStringAsync();

                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new Exception("❌ Azure API Key inválida. Verifica tu clave en Azure Portal");
                        }

                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            throw new Exception("❌ Deployment 'whisper' no encontrado. Verifica la URL en Azure Portal");
                        }

                        throw new Exception($"Error de Azure Whisper API: {response.StatusCode} - {error}");
                    }

                    var responseText = await response.Content.ReadAsStringAsync();
                    var jsonDoc = JsonDocument.Parse(responseText);

                    string transcription = jsonDoc.RootElement
                        .GetProperty("text")
                        .GetString();

                    return transcription?.Trim() ?? string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al transcribir audio: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Convierte MemoryStream de audio grabado a formato WAV compatible con Whisper
        /// </summary>
        public byte[] ConvertToWavFormat(MemoryStream audioStream, NAudio.Wave.WaveFormat waveFormat)
        {
            try
            {
                audioStream.Position = 0;

                using (var outputStream = new MemoryStream())
                using (var waveFileWriter = new NAudio.Wave.WaveFileWriter(outputStream, waveFormat))
                {
                    audioStream.Position = 0;
                    audioStream.CopyTo(waveFileWriter);
                    waveFileWriter.Flush();

                    return outputStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al convertir audio: {ex.Message}", ex);
            }
        }
    }
}