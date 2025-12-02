using Microsoft.Win32;
using NAudio.Wave;
using PrimerosAuxilios.BLL;
using PrimerosAuxilios.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI
{
    public partial class ChatView : UserControl
    {
        public ObservableCollection<ChatMessage> Messages { get; set; }
        private ClaudeApiService claudeService;
        private WhisperApiService whisperService;

        // ⭐ Usuario actual
        private string currentUsername = "";

        // Para grabación de audio
        private WaveInEvent waveIn;
        private MemoryStream recordedAudioStream;
        private WaveFormat recordingFormat;
        private bool isRecording = false;
        private DateTime recordingStartTime;

        // Para gestión de historial - ACTUALIZADO
        private ConversationService conversationService;
        private SavedConversation currentConversation;

        // Control de procesamiento
        private bool isProcessingImage = false;

        public ChatView()
        {
            InitializeComponent();
            Messages = new ObservableCollection<ChatMessage>();
            ChatMessagesList.ItemsSource = Messages;

            InitializeClaude();
            InitializeWhisper();
            InitializeHistoryManager();

            MessageTextBox.KeyDown += MessageTextBox_KeyDown;
            RecordAudioButton.PreviewMouseDown += RecordAudioButton_MouseDown;
            RecordAudioButton.PreviewMouseUp += RecordAudioButton_MouseUp;

            AddWelcomeMessage();
        }

        /// <summary>
        /// ⭐ Establece el usuario actual
        /// </summary>
        public void SetCurrentUser(string username)
        {
            currentUsername = username;

            if (currentConversation != null && string.IsNullOrEmpty(currentConversation.Username))
            {
                currentConversation.Username = currentUsername;
            }

            UpdateWelcomeMessage();
        }

        private void UpdateWelcomeMessage()
        {
            if (string.IsNullOrEmpty(currentUsername))
                return;

            string welcomeText = "🚨 HEALIA - Asistente de Emergencias Médicas 🚨\n\n";

            if (currentUsername.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                welcomeText += $"👨‍⚕️ Bienvenido Administrador\n\n" +
                              "Como administrador, puedes ver todas las conversaciones de todos los usuarios en el historial.\n\n";
            }
            else
            {
                welcomeText += $"👋 Bienvenido {currentUsername}\n\n" +
                              "Tus conversaciones se guardarán automáticamente y solo tú podrás verlas.\n\n";
            }

            welcomeText += "Estoy aquí para ayudarte con emergencias médicas usando Claude AI + Whisper AI.\n\n" +
                          "Puedes:\n" +
                          "📷 Subir foto de herida, quemadura, picadura, etc.\n" +
                          "🎤 Grabar audio describiendo la emergencia (se transcribe automáticamente)\n" +
                          "💬 Escribir tu situación médica\n\n" +
                          "⚠️ IMPORTANTE: Esto NO reemplaza atención médica profesional. " +
                          "Si es crítico, llama al 123 inmediatamente.\n\n" +
                          "¿En qué puedo ayudarte?";

            if (Messages.Count == 1 && Messages[0].Contenido.Contains("HEALIA"))
            {
                Messages[0].Contenido = welcomeText;
            }
        }

        private void InitializeClaude()
        {
            try
            {
                claudeService = new ClaudeApiService();
                StatusText.Text = "Conectado - Claude AI + Whisper AI";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error de conexión";
                MessageBox.Show($"Error al inicializar Claude: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeWhisper()
        {
            try
            {
                whisperService = new WhisperApiService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar Whisper: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeHistoryManager()
        {
            conversationService = new ConversationService();
            currentConversation = new SavedConversation();
        }

        private void AddWelcomeMessage()
        {
            Messages.Add(new ChatMessage
            {
                Contenido = "🚨 HEALIA - Asistente de Emergencias Médicas 🚨\n\n" +
                           "Estoy aquí para ayudarte con emergencias médicas usando Claude AI + Whisper AI.\n\n" +
                           "Puedes:\n" +
                           "📷 Subir foto de herida, quemadura, picadura, etc.\n" +
                           "🎤 Grabar audio describiendo la emergencia (se transcribe automáticamente)\n" +
                           "💬 Escribir tu situación médica\n\n" +
                           "⚠️ IMPORTANTE: Esto NO reemplaza atención médica profesional. " +
                           "Si es crítico, llama al 123 inmediatamente.\n\n" +
                           "¿En qué puedo ayudarte?",
                EsEnviado = false,
                Timestamp = DateTime.Now
            });
            ScrollToBottom();
        }

        // ==================== NUEVA CONVERSACIÓN ====================

        private void NewConversationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Messages.Count > 1)
            {
                var result = MessageBox.Show(
                    "¿Deseas iniciar una nueva conversación?\n\n" +
                    "La conversación actual se guardará automáticamente en el historial.",
                    "Nueva Conversación",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    StartNewConversation();
                }
            }
            else
            {
                StartNewConversation();
            }
        }

        // ==================== ANÁLISIS DE IMÁGENES ====================

        private async void AttachFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (isProcessingImage)
                return;

            try
            {
                isProcessingImage = true;

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Selecciona una imagen para analizar",
                    Filter = "Imagenes (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Todos los archivos (*.*)|*.*",
                    Multiselect = false
                };

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    string imagePath = openFileDialog.FileName;
                    await ProcessImageAnalysis(imagePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al seleccionar imagen: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                isProcessingImage = false;
            }
        }

        private async Task ProcessImageAnalysis(string imagePath)
        {
            try
            {
                Messages.Add(new ChatMessage
                {
                    Contenido = "📷 Imagen cargada. Analizando emergencia médica...",
                    EsEnviado = true,
                    Timestamp = DateTime.Now,
                    ImagePath = imagePath
                });
                ScrollToBottom();

                var typingIndicator = new ChatMessage
                {
                    Contenido = "",
                    EsEnviado = false,
                    IsTyping = true,
                    Timestamp = DateTime.Now
                };
                Messages.Add(typingIndicator);
                ScrollToBottom();

                StatusText.Text = "🚨 Analizando emergencia con Claude Vision AI...";

                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string extension = Path.GetExtension(imagePath);
                string analysis = await claudeService.AnalyzeEmergencyPhoto(imageBytes, extension);

                Messages.Remove(typingIndicator);

                Messages.Add(new ChatMessage
                {
                    Contenido = analysis,
                    EsEnviado = false,
                    Timestamp = DateTime.Now
                });

                StatusText.Text = "Análisis completo - Claude AI";
                SaveCurrentConversation();
            }
            catch (Exception ex)
            {
                Messages.Add(new ChatMessage
                {
                    Contenido = $"❌ Error al analizar: {ex.Message}",
                    EsEnviado = false,
                    Timestamp = DateTime.Now
                });
                StatusText.Text = "Error en análisis";
            }
            finally
            {
                ScrollToBottom();
            }
        }

        // ==================== AUDIO CON WHISPER ====================

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SendButton.IsEnabled = !string.IsNullOrWhiteSpace(MessageTextBox.Text);
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                e.Handled = true;
                if (SendButton.IsEnabled)
                {
                    SendButton_Click(sender, null);
                }
            }
        }

        private void RecordAudioButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isRecording)
            {
                StartRecording();
            }
        }

        private async void RecordAudioButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isRecording)
            {
                await StopRecordingAndProcess();
            }
        }

        private void StartRecording()
        {
            try
            {
                isRecording = true;
                recordingStartTime = DateTime.Now;
                recordedAudioStream = new MemoryStream();

                recordingFormat = new WaveFormat(16000, 16, 1);

                waveIn = new WaveInEvent
                {
                    WaveFormat = recordingFormat
                };

                waveIn.DataAvailable += (s, e) =>
                {
                    recordedAudioStream.Write(e.Buffer, 0, e.BytesRecorded);
                };

                waveIn.StartRecording();

                RecordAudioButton.Background = new SolidColorBrush(Color.FromRgb(220, 53, 69));
                StatusText.Text = "🎤 Grabando... (suelta para detener)";

                Messages.Add(new ChatMessage
                {
                    Contenido = "🎤 Grabando audio...",
                    EsEnviado = true,
                    Timestamp = DateTime.Now,
                    IsTemporary = true
                });
                ScrollToBottom();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar grabación: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isRecording = false;
            }
        }

        private async Task StopRecordingAndProcess()
        {
            try
            {
                isRecording = false;
                waveIn?.StopRecording();
                waveIn?.Dispose();

                TimeSpan duration = DateTime.Now - recordingStartTime;
                int audioDurationSeconds = (int)duration.TotalSeconds;

                RecordAudioButton.Background = new SolidColorBrush(Color.FromRgb(45, 45, 48));

                var tempMsg = Messages.FirstOrDefault(m => m.IsTemporary);
                if (tempMsg != null)
                    Messages.Remove(tempMsg);

                if (recordedAudioStream.Length > 0 && audioDurationSeconds >= 1)
                {
                    Messages.Add(new ChatMessage
                    {
                        Contenido = $"🎤 Audio grabado ({audioDurationSeconds}s)",
                        EsEnviado = true,
                        Timestamp = DateTime.Now,
                        IsAudioMessage = true,
                        AudioDurationSeconds = audioDurationSeconds
                    });
                    ScrollToBottom();

                    StatusText.Text = "🔄 Transcribiendo con Whisper AI...";

                    var typingIndicator = new ChatMessage
                    {
                        Contenido = "",
                        EsEnviado = false,
                        IsTyping = true,
                        Timestamp = DateTime.Now
                    };
                    Messages.Add(typingIndicator);
                    ScrollToBottom();

                    try
                    {
                        byte[] wavData = ConvertToWavFormat(
                            recordedAudioStream,
                            recordingFormat
                        );

                        string transcription = await whisperService.TranscribeAudio(wavData);

                        Messages.Remove(typingIndicator);

                        if (!string.IsNullOrWhiteSpace(transcription))
                        {
                            Messages.Add(new ChatMessage
                            {
                                Contenido = $"📝 **Transcripción:**\n\"{transcription}\"",
                                EsEnviado = true,
                                Timestamp = DateTime.Now
                            });
                            ScrollToBottom();

                            StatusText.Text = "🤖 Analizando con Claude AI...";

                            var claudeTyping = new ChatMessage
                            {
                                Contenido = "",
                                EsEnviado = false,
                                IsTyping = true,
                                Timestamp = DateTime.Now
                            };
                            Messages.Add(claudeTyping);
                            ScrollToBottom();

                            string claudeResponse = await claudeService.AnalyzeEmergencyText(transcription);

                            Messages.Remove(claudeTyping);

                            Messages.Add(new ChatMessage
                            {
                                Contenido = claudeResponse,
                                EsEnviado = false,
                                Timestamp = DateTime.Now
                            });

                            StatusText.Text = "✅ Análisis completo - Whisper + Claude";
                            SaveCurrentConversation();
                        }
                        else
                        {
                            Messages.Remove(typingIndicator);
                            Messages.Add(new ChatMessage
                            {
                                Contenido = "⚠️ No se pudo transcribir el audio. Intenta hablar más claro o más tiempo.",
                                EsEnviado = false,
                                Timestamp = DateTime.Now
                            });
                            StatusText.Text = "Error en transcripción";
                        }
                    }
                    catch (Exception ex)
                    {
                        Messages.Remove(typingIndicator);
                        Messages.Add(new ChatMessage
                        {
                            Contenido = $"❌ Error al procesar audio: {ex.Message}\n\n" +
                                       "Verifica que tengas configurada tu API Key de OpenAI.",
                            EsEnviado = false,
                            Timestamp = DateTime.Now
                        });
                        StatusText.Text = "Error en procesamiento";
                    }
                }
                else
                {
                    Messages.Add(new ChatMessage
                    {
                        Contenido = "⚠️ Audio muy corto. Mantén presionado al menos 1 segundo.",
                        EsEnviado = false,
                        Timestamp = DateTime.Now
                    });
                    StatusText.Text = "Conectado - Claude AI + Whisper AI";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar audio: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText.Text = "Error en audio";
            }
            finally
            {
                recordedAudioStream?.Dispose();
                ScrollToBottom();
            }
        }

        /// <summary>
        /// Convierte MemoryStream de audio a formato WAV
        /// </summary>
        private byte[] ConvertToWavFormat(MemoryStream audioStream, WaveFormat waveFormat)
        {
            try
            {
                audioStream.Position = 0;

                using (var outputStream = new MemoryStream())
                using (var waveFileWriter = new WaveFileWriter(outputStream, waveFormat))
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

        // ==================== ENVIAR MENSAJE DE TEXTO ====================

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = MessageTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            Messages.Add(new ChatMessage
            {
                Contenido = userMessage,
                EsEnviado = true,
                Timestamp = DateTime.Now
            });

            MessageTextBox.Clear();
            ScrollToBottom();

            var typingIndicator = new ChatMessage
            {
                Contenido = "",
                EsEnviado = false,
                IsTyping = true,
                Timestamp = DateTime.Now
            };
            Messages.Add(typingIndicator);
            ScrollToBottom();

            try
            {
                SendButton.IsEnabled = false;
                StatusText.Text = "Procesando con Claude...";

                string aiResponse = await claudeService.AnalyzeEmergencyText(userMessage);

                Messages.Remove(typingIndicator);

                Messages.Add(new ChatMessage
                {
                    Contenido = aiResponse,
                    EsEnviado = false,
                    Timestamp = DateTime.Now
                });

                StatusText.Text = "Conectado - Claude AI + Whisper AI";
                SaveCurrentConversation();
            }
            catch (Exception ex)
            {
                Messages.Remove(typingIndicator);

                Messages.Add(new ChatMessage
                {
                    Contenido = $"❌ Error: {ex.Message}\n\n¿Configuraste tu API Key de Claude?",
                    EsEnviado = false,
                    Timestamp = DateTime.Now
                });

                StatusText.Text = "Error de conexión";
            }
            finally
            {
                ScrollToBottom();
            }
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }

        // ==================== GESTIÓN DE HISTORIAL ====================

        private void SaveCurrentConversation()
        {
            try
            {
                if (Messages.Count <= 1)
                    return;

                currentConversation.Username = currentUsername;

                currentConversation.Messages = Messages
                    .Where(m => !m.IsTyping && !m.IsTemporary)
                    .Select(m => new SavedMessage
                    {
                        Contenido = m.Contenido,
                        EsEnviado = m.EsEnviado,
                        Timestamp = m.Timestamp,
                        IsAudioMessage = m.IsAudioMessage,
                        AudioDurationSeconds = m.AudioDurationSeconds
                    })
                    .ToList();

                if (string.IsNullOrEmpty(currentConversation.Title))
                {
                    currentConversation.Title = conversationService.GenerateConversationTitle(
                        currentConversation.Messages);
                }

                conversationService.SaveConversation(currentConversation);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al guardar conversación: {ex.Message}");
            }
        }

        public void LoadConversation(SavedConversation conversation)
        {
            try
            {
                Messages.Clear();

                foreach (var savedMsg in conversation.Messages)
                {
                    Messages.Add(new ChatMessage
                    {
                        Contenido = savedMsg.Contenido,
                        EsEnviado = savedMsg.EsEnviado,
                        Timestamp = savedMsg.Timestamp,
                        IsAudioMessage = savedMsg.IsAudioMessage,
                        AudioDurationSeconds = savedMsg.AudioDurationSeconds
                    });
                }

                currentConversation = conversation;
                ScrollToBottom();
                StatusText.Text = $"Conversación cargada: {conversation.MessageCount} mensajes";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar conversación: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void StartNewConversation()
        {
            if (Messages.Count > 1)
            {
                SaveCurrentConversation();
            }

            currentConversation = new SavedConversation();
            currentConversation.Username = currentUsername;

            Messages.Clear();
            AddWelcomeMessage();
            UpdateWelcomeMessage();

            StatusText.Text = "Nueva conversación iniciada";

            Task.Delay(2000).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    StatusText.Text = "Conectado - Claude AI + Whisper AI";
                });
            });
        }
    }

    // ==================== CLASES DE SOPORTE ====================

    public class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SentTemplate { get; set; }
        public DataTemplate ReceivedTemplate { get; set; }
        public DataTemplate TypingTemplate { get; set; }
        public DataTemplate AudioTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ChatMessage message)
            {
                if (message.IsTyping)
                    return TypingTemplate;

                if (message.IsAudioMessage && message.EsEnviado)
                    return AudioTemplate;

                return message.EsEnviado ? SentTemplate : ReceivedTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
}