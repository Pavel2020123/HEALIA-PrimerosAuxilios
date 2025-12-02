using System;

namespace PrimerosAuxilios.Entity
{
    /// <summary>
    /// Entidad que representa un mensaje individual guardado
    /// </summary>
    public class SavedMessage
    {
        public string Contenido { get; set; }
        public bool EsEnviado { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsAudioMessage { get; set; }
        public int AudioDurationSeconds { get; set; }
    }
}