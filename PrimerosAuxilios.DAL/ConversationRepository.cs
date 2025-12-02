using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PrimerosAuxilios.Entity;

namespace PrimerosAuxilios.DAL
{
    /// <summary>
    /// Repositorio para acceso a datos de conversaciones
    /// Maneja la persistencia en archivo JSON
    /// </summary>
    public class ConversationRepository
    {
        private readonly string _conversationsFilePath;

        public ConversationRepository()
        {
            string historyFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "HEALIA",
                "Conversations"
            );

            _conversationsFilePath = Path.Combine(historyFolder, "conversations.json");

            if (!Directory.Exists(historyFolder))
            {
                Directory.CreateDirectory(historyFolder);
            }
        }

        /// <summary>
        /// Obtiene todas las conversaciones
        /// </summary>
        public List<SavedConversation> GetAll()
        {
            if (!File.Exists(_conversationsFilePath))
            {
                return new List<SavedConversation>();
            }

            try
            {
                string json = File.ReadAllText(_conversationsFilePath);
                var conversations = JsonSerializer.Deserialize<List<SavedConversation>>(json);
                return conversations?.OrderByDescending(c => c.LastModified).ToList()
                       ?? new List<SavedConversation>();
            }
            catch (Exception)
            {
                return new List<SavedConversation>();
            }
        }

        /// <summary>
        /// Obtiene conversaciones de un usuario específico
        /// </summary>
        public List<SavedConversation> GetByUsername(string username)
        {
            var allConversations = GetAll();

            // Si es admin, devolver todas
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return allConversations;
            }

            // Si es usuario normal, filtrar solo sus conversaciones
            return allConversations
                .Where(c => c.Username != null &&
                            c.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Obtiene una conversación por ID
        /// </summary>
        public SavedConversation GetById(string id)
        {
            var allConversations = GetAll();
            return allConversations.FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Guarda o actualiza una conversación
        /// </summary>
        public void Save(SavedConversation conversation)
        {
            try
            {
                var allConversations = GetAll();

                // Si ya existe, actualizar; si no, agregar
                var existing = allConversations.FirstOrDefault(c => c.Id == conversation.Id);
                if (existing != null)
                {
                    allConversations.Remove(existing);
                }

                conversation.LastModified = DateTime.Now;
                conversation.MessageCount = conversation.Messages.Count;
                allConversations.Add(conversation);

                // Guardar en archivo JSON
                SaveAll(allConversations);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar conversación: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Elimina una conversación
        /// </summary>
        public void Delete(string id)
        {
            try
            {
                var allConversations = GetAll();
                var toRemove = allConversations.FirstOrDefault(c => c.Id == id);

                if (toRemove != null)
                {
                    allConversations.Remove(toRemove);
                    SaveAll(allConversations);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar conversación: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Guarda todas las conversaciones en el archivo JSON
        /// </summary>
        private void SaveAll(List<SavedConversation> conversations)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            string json = JsonSerializer.Serialize(conversations, options);
            File.WriteAllText(_conversationsFilePath, json);
        }
    }
}