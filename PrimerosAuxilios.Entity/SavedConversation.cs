using System;
using System.Collections.Generic;

namespace PrimerosAuxilios.Entity
{
    /// <summary>
    /// Entidad que representa una conversación guardada completa
    /// </summary>
    public class SavedConversation
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public List<SavedMessage> Messages { get; set; }
        public int MessageCount { get; set; }

        public SavedConversation()
        {
            Id = Guid.NewGuid().ToString();
            Messages = new List<SavedMessage>();
            CreatedAt = DateTime.Now;
            LastModified = DateTime.Now;
        }
    }
}