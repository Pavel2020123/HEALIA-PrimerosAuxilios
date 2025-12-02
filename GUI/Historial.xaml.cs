using GUI;
using PrimerosAuxilios.BLL;
using PrimerosAuxilios.Entity;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI
{
    /// <summary>
    /// Control de usuario para el Historial
    /// SOLO maneja la interfaz - la lógica está en BLL
    /// </summary>
    public partial class Historial : UserControl
    {
        private ConversationService conversationService;
        private string currentUsername = "";

        public Historial()
        {
            InitializeComponent();
            conversationService = new ConversationService(); // Usa BLL
            this.Loaded += Historial_Loaded;
        }

        public void SetCurrentUser(string username)
        {
            currentUsername = username;
        }

        private void Historial_Loaded(object sender, RoutedEventArgs e)
        {
            LoadConversations();
        }

        /// <summary>
        /// Carga y muestra las conversaciones filtradas por usuario
        /// </summary>
        public void LoadConversations()
        {
            try
            {
                ConversationsPanel.Children.Clear();

                // Obtener conversaciones usando BLL
                var conversations = conversationService.GetConversationsForUser(currentUsername);

                if (conversations.Count == 0)
                {
                    ConversationsPanel.Children.Add(EmptyStatePanel);

                    if (currentUsername.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        ConversationCountText.Text = "0 conversaciones (de todos los usuarios)";
                    }
                    else
                    {
                        ConversationCountText.Text = "0 conversaciones propias";
                    }
                    return;
                }

                EmptyStatePanel.Visibility = Visibility.Collapsed;

                if (currentUsername.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    ConversationCountText.Text = $"{conversations.Count} conversación{(conversations.Count != 1 ? "es" : "")} (todos los usuarios)";
                }
                else
                {
                    ConversationCountText.Text = $"{conversations.Count} conversación{(conversations.Count != 1 ? "es" : "")} propia{(conversations.Count != 1 ? "s" : "")}";
                }

                foreach (var conversation in conversations)
                {
                    var conversationUI = CreateConversationUI(conversation);
                    ConversationsPanel.Children.Add(conversationUI);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar conversaciones: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Crea la UI para una conversación individual
        /// </summary>
        private Button CreateConversationUI(SavedConversation conversation)
        {
            var button = new Button
            {
                Style = (Style)FindResource("ConversationButtonStyle"),
                HorizontalContentAlignment = HorizontalAlignment.Stretch
            };

            var grid = new Grid
            {
                Margin = new Thickness(16, 12, 16, 12)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var infoPanel = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            var titleText = new TextBlock
            {
                Text = conversation.Title,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromRgb(33, 33, 33)),
                TextTrimming = TextTrimming.CharacterEllipsis,
                Margin = new Thickness(0, 0, 0, 6)
            };

            var detailsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            // Mostrar username si es admin
            if (currentUsername.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                var usernameText = new TextBlock
                {
                    Text = $"👤 {conversation.Username ?? "Usuario desconocido"}",
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 152, 0)),
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 0, 16, 0)
                };
                detailsPanel.Children.Add(usernameText);
            }

            var dateText = new TextBlock
            {
                Text = conversation.LastModified.ToString("dd/MM/yyyy HH:mm"),
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 181, 246)),
                Margin = new Thickness(0, 0, 16, 0)
            };

            var messageCountText = new TextBlock
            {
                Text = $"{conversation.MessageCount} mensaje{(conversation.MessageCount != 1 ? "s" : "")}",
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(100, 181, 246))
            };

            detailsPanel.Children.Add(dateText);
            detailsPanel.Children.Add(messageCountText);

            infoPanel.Children.Add(titleText);
            infoPanel.Children.Add(detailsPanel);

            Grid.SetColumn(infoPanel, 0);
            grid.Children.Add(infoPanel);

            var deleteButton = new Button
            {
                Content = "🗑️",
                FontSize = 16,
                Style = (Style)FindResource("DeleteButtonStyle"),
                ToolTip = "Eliminar conversación"
            };

            deleteButton.Click += (s, e) =>
            {
                e.Handled = true;
                DeleteConversation(conversation);
            };

            Grid.SetColumn(deleteButton, 1);
            grid.Children.Add(deleteButton);

            button.Content = grid;

            button.Click += (s, e) =>
            {
                LoadConversationIntoChat(conversation);
            };

            return button;
        }

        /// <summary>
        /// Elimina una conversación con confirmación
        /// </summary>
        private void DeleteConversation(SavedConversation conversation)
        {
            string confirmMessage = $"¿Estás seguro de que deseas eliminar esta conversación?\n\n\"{conversation.Title}\"";

            if (currentUsername.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                confirmMessage += $"\n\n⚠️ Esta conversación pertenece a: {conversation.Username ?? "Usuario desconocido"}";
            }

            var result = MessageBox.Show(
                confirmMessage,
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Eliminar usando BLL
                    conversationService.DeleteConversation(conversation.Id);
                    LoadConversations();
                    MessageBox.Show("Conversación eliminada correctamente",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Carga una conversación guardada en el ChatView
        /// </summary>
        private void LoadConversationIntoChat(SavedConversation conversation)
        {
            try
            {
                // Validar permisos usando BLL
                bool canAccess = conversationService.CanUserAccessConversation(currentUsername, conversation);

                if (!canAccess)
                {
                    MessageBox.Show("No tienes permisos para ver esta conversación.",
                        "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow == null)
                {
                    MessageBox.Show("No se pudo acceder a la ventana principal",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                mainWindow.Chat_UC.LoadConversation(conversation);
                mainWindow.HideHistorial();

                System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        mainWindow.ShowChat();
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar conversación: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewConversationFromHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow == null)
                {
                    MessageBox.Show("No se pudo acceder a la ventana principal",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                mainWindow.Chat_UC.StartNewConversation();
                mainWindow.HideHistorial();

                System.Threading.Tasks.Task.Delay(300).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        mainWindow.ShowChat();
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar nueva conversación: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}