using System;

namespace PrimerosAuxilios.Entity
{
    /// <summary>
    /// Entidad que representa un mensaje en el chat
    /// </summary>
    public class ChatMessage
    {
        public string Contenido { get; set; }
        public bool EsEnviado { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsTyping { get; set; } = false;
        public bool IsTemporary { get; set; } = false;
        public bool IsAudioMessage { get; set; } = false;
        public int AudioDurationSeconds { get; set; } = 0;
        public string ImagePath { get; set; } = null;
    }
}