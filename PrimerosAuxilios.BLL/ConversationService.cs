using System;
using System.Collections.Generic;
using System.Linq;
using PrimerosAuxilios.Entity;
using PrimerosAuxilios.DAL;

namespace PrimerosAuxilios.BLL
{
    /// <summary>
    /// Servicio de lógica de negocio para conversaciones
    /// </summary>
    public class ConversationService
    {
        private readonly ConversationRepository _conversationRepository;

        public ConversationService()
        {
            _conversationRepository = new ConversationRepository();
        }

        /// <summary>
        /// Obtiene todas las conversaciones
        /// </summary>
        public List<SavedConversation> GetAllConversations()
        {
            return _conversationRepository.GetAll();
        }

        /// <summary>
        /// Obtiene conversaciones de un usuario específico
        /// Si es admin, obtiene todas las conversaciones
        /// </summary>
        public List<SavedConversation> GetConversationsForUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return new List<SavedConversation>();
            }

            return _conversationRepository.GetByUsername(username);
        }

        /// <summary>
        /// Obtiene una conversación por ID
        /// </summary>
        public SavedConversation GetConversationById(string id)
        {
            return _conversationRepository.GetById(id);
        }

        /// <summary>
        /// Guarda una conversación
        /// </summary>
        public void SaveConversation(SavedConversation conversation)
        {
            // Validación: La conversación debe tener al menos un mensaje
            if (conversation == null || conversation.Messages == null || conversation.Messages.Count == 0)
            {
                throw new ArgumentException("La conversación debe tener al menos un mensaje");
            }

            // Validación: La conversación debe tener un username asignado
            if (string.IsNullOrWhiteSpace(conversation.Username))
            {
                throw new ArgumentException("La conversación debe tener un usuario asignado");
            }

            // Si no tiene título, generar uno automático
            if (string.IsNullOrWhiteSpace(conversation.Title))
            {
                conversation.Title = GenerateConversationTitle(conversation.Messages);
            }

            _conversationRepository.Save(conversation);
        }

        /// <summary>
        /// Elimina una conversación
        /// </summary>
        public void DeleteConversation(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("El ID de la conversación no puede estar vacío");
            }

            _conversationRepository.Delete(id);
        }

        /// <summary>
        /// Genera un título automático basado en los mensajes
        /// </summary>
        public string GenerateConversationTitle(List<SavedMessage> messages)
        {
            var firstUserMessage = messages.FirstOrDefault(m => m.EsEnviado);

            if (firstUserMessage != null)
            {
                string content = firstUserMessage.Contenido;

                // Tomar máximo 50 caracteres
                if (content.Length > 50)
                {
                    return content.Substring(0, 47) + "...";
                }

                return content;
            }

            return $"Conversación {DateTime.Now:dd/MM/yyyy HH:mm}";
        }

        /// <summary>
        /// Valida si un usuario puede acceder a una conversación
        /// </summary>
        public bool CanUserAccessConversation(string username, SavedConversation conversation)
        {
            if (conversation == null)
            {
                return false;
            }

            // El admin puede ver todas las conversaciones
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // El usuario solo puede ver sus propias conversaciones
            return conversation.Username != null &&
                   conversation.Username.Equals(username, StringComparison.OrdinalIgnoreCase);
        }
    }
}