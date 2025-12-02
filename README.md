# HEALIA - Sistema de Primeros Auxilios con Inteligencia Artificial

## Descripción del Proyecto

HEALIA es un sistema inteligente de asistencia médica de emergencias desarrollado como proyecto académico. El sistema integra tecnologías de inteligencia artificial de última generación para proporcionar análisis y recomendaciones en situaciones de emergencia médica.

### Tecnologías Integradas

- **Claude AI** (Anthropic): Modelo de lenguaje avanzado para análisis de emergencias médicas
- **Whisper AI** (Azure OpenAI): Motor de transcripción automática de audio a texto
- **Arquitectura por Capas**: Implementación de patrón arquitectónico multicapa (Entity, DAL, BLL, GUI)

---

## Arquitectura del Sistema

El proyecto implementa una arquitectura por capas que garantiza separación de responsabilidades, mantenibilidad y escalabilidad.

### Estructura de Capas

```
PrimerosAuxilios/
│
├── PrimerosAuxilios.Entity/        (Capa de Entidades)
│   ├── User.cs
│   ├── ChatMessage.cs
│   ├── SavedConversation.cs
│   ├── SavedMessage.cs
│   └── Enums.cs
│
├── PrimerosAuxilios.DAL/           (Capa de Acceso a Datos)
│   ├── UserRepository.cs
│   └── ConversationRepository.cs
│
├── PrimerosAuxilios.BLL/           (Capa de Lógica de Negocio)
│   ├── UserService.cs
│   ├── ConversationService.cs
│   ├── ClaudeApiService.cs
│   └── WhisperApiService.cs
│
└── GUI/                            (Capa de Presentación)
    ├── LoginUserControl.xaml
    ├── RegisterUserControl.xaml
    ├── ChatView.xaml
    ├── Historial.xaml
    └── MainWindow.xaml
```

### Descripción de Capas

#### Capa Entity (Entidades)
Contiene los modelos de datos del sistema. Define la estructura de las entidades de negocio sin implementar lógica.

**Responsabilidades:**
- Definición de estructuras de datos
- Propiedades y atributos de entidades
- Enumeraciones del sistema

#### Capa DAL (Data Access Layer)
Implementa el patrón Repository para el acceso a datos. Maneja la persistencia mediante archivos JSON.

**Responsabilidades:**
- Operaciones CRUD (Create, Read, Update, Delete)
- Persistencia de datos en formato JSON
- Consultas y filtrado de información

#### Capa BLL (Business Logic Layer)
Contiene toda la lógica de negocio del sistema, incluyendo validaciones, reglas de negocio e integración con APIs externas.

**Responsabilidades:**
- Validación de datos de negocio
- Implementación de reglas de negocio
- Coordinación entre capas
- Integración con servicios externos (Claude AI, Whisper AI)

#### Capa GUI (Graphical User Interface)
Implementa la interfaz de usuario mediante Windows Presentation Foundation (WPF).

**Responsabilidades:**
- Presentación de información
- Captura de eventos de usuario
- Navegación entre vistas
- Invocación de servicios de la capa BLL

---

## Tecnologías y Herramientas

### Framework y Lenguaje
- .NET Framework 4.7.2+
- C# 7.0+
- Windows Presentation Foundation (WPF)

### Persistencia de Datos
- System.Text.Json (serialización)
- Archivos JSON (almacenamiento)

### APIs Externas
- Claude AI API (Anthropic) - Análisis de emergencias médicas
- Whisper AI API (Azure OpenAI) - Transcripción de audio

### Bibliotecas de Terceros
- NAudio - Procesamiento y grabación de audio
- Microsoft.Web.WebView2 - Visualización de contenido web

---

## Requisitos del Sistema

### Requisitos de Software
- Windows 10 o superior
- .NET Framework 4.7.2 o superior
- Visual Studio 2019 o superior (para desarrollo)

### Requisitos de Hardware
- Procesador: 2.0 GHz o superior
- Memoria RAM: 4 GB mínimo (8 GB recomendado)
- Espacio en disco: 500 MB
- Micrófono (para funcionalidad de grabación de audio)
- Conexión a Internet (para servicios de IA)

---

## Instalación y Configuración

### Clonar el Repositorio

```bash
git clone [URL_DEL_REPOSITORIO]
cd PrimerosAuxilios
```

### Restaurar Dependencias

Abrir la solución en Visual Studio:
```
PrimerosAuxilios.sln
```

Visual Studio restaurará automáticamente los paquetes NuGet necesarios.

### Configuración de API Keys

#### Claude AI
Editar archivo: `PrimerosAuxilios.BLL/ClaudeApiService.cs`
```csharp
private const string API_KEY = "SU_CLAUDE_API_KEY";
```

#### Whisper AI (Azure OpenAI)
Editar archivo: `PrimerosAuxilios.BLL/WhisperApiService.cs`
```csharp
private const string AZURE_API_KEY = "SU_AZURE_API_KEY";
private const string WHISPER_API_URL = "SU_AZURE_ENDPOINT";
```

### Compilación

Desde Visual Studio:
```
Compilar > Recompilar Solución
```

Desde línea de comandos:
```bash
msbuild PrimerosAuxilios.sln /t:Rebuild /p:Configuration=Release
```

### Ejecución

Presionar F5 en Visual Studio o ejecutar el archivo:
```
GUI\bin\Release\GUI.exe
```

---

## Funcionalidades del Sistema

### Módulo de Autenticación

**Inicio de Sesión**
- Validación de credenciales de usuario
- Actualización de última sesión
- Control de acceso basado en roles

**Registro de Usuarios**
- Creación de nuevas cuentas
- Validación de datos de entrada
- Verificación de unicidad de usuario y correo electrónico

**Recuperación de Contraseña**
- Validación de identidad del usuario
- Actualización segura de credenciales

### Módulo de Chat Inteligente

**Análisis de Texto**
- Procesamiento de consultas en lenguaje natural
- Análisis contextual de emergencias médicas
- Generación de recomendaciones personalizadas

**Análisis de Imágenes**
- Procesamiento de imágenes médicas (heridas, quemaduras, lesiones)
- Identificación de características visuales
- Evaluación de gravedad

**Transcripción de Audio**
- Captura de audio en tiempo real
- Transcripción automática mediante Whisper AI
- Procesamiento de texto transcrito

### Módulo de Historial

**Gestión de Conversaciones**
- Almacenamiento automático de interacciones
- Búsqueda y filtrado por usuario
- Control de acceso basado en permisos

**Funcionalidades Administrativas**
- Visualización de todas las conversaciones (rol administrador)
- Gestión de conversaciones por usuario
- Eliminación de registros

### Módulo de Mapas

**Visualización Geográfica**
- Integración con Google Maps
- Visualización de ubicaciones de emergencia
- Navegación interactiva

---

## Usuarios del Sistema

### Usuario Administrador

**Credenciales por Defecto:**
- Usuario: `admin`
- Contraseña: `admin123`

**Permisos:**
- Acceso completo al sistema
- Visualización de todas las conversaciones
- Gestión de usuarios

### Usuario Regular

**Registro:**
Los usuarios pueden crear cuentas nuevas mediante el formulario de registro.

**Permisos:**
- Acceso a funcionalidades de chat
- Visualización de conversaciones propias
- Gestión de perfil personal

---

## Persistencia de Datos

### Ubicación de Archivos

**Sistema de Archivos Windows:**
```
%AppData%\PrimerosAuxilios\users.json
%AppData%\HEALIA\Conversations\conversations.json
```

### Estructura de Datos

#### users.json
```json
[
  {
    "Username": "string",
    "Password": "string",
    "Email": "string",
    "FullName": "string",
    "CreatedAt": "datetime",
    "LastLogin": "datetime"
  }
]
```

#### conversations.json
```json
[
  {
    "Id": "guid",
    "Title": "string",
    "Username": "string",
    "CreatedAt": "datetime",
    "LastModified": "datetime",
    "MessageCount": "integer",
    "Messages": [
      {
        "Contenido": "string",
        "EsEnviado": "boolean",
        "Timestamp": "datetime",
        "IsAudioMessage": "boolean",
        "AudioDurationSeconds": "integer"
      }
    ]
  }
]
```

---

## Flujo de Datos del Sistema

### Ejemplo: Proceso de Autenticación

```
1. Usuario ingresa credenciales (GUI)
   └─> LoginUserControl.xaml.cs

2. Validación de campos no vacíos (GUI)
   └─> Verificación de formato básico

3. Invocación de servicio de autenticación (BLL)
   └─> UserService.Login(username, password)

4. Validación de reglas de negocio (BLL)
   └─> Verificación de formato y longitud

5. Consulta de datos de usuario (DAL)
   └─> UserRepository.GetByUsername(username)

6. Lectura de archivo JSON (DAL)
   └─> Deserialización de users.json

7. Retorno de entidad User (Entity)
   └─> Objeto con datos del usuario

8. Verificación de contraseña (BLL)
   └─> Comparación de credenciales

9. Actualización de última sesión (BLL)
   └─> user.LastLogin = DateTime.Now

10. Persistencia de cambios (DAL)
    └─> UserRepository.Update(user)

11. Escritura en archivo JSON (DAL)
    └─> Serialización y guardado

12. Retorno de resultado (BLL → GUI)
    └─> true/false

13. Actualización de interfaz (GUI)
    └─> Navegación a vista principal o mensaje de error
```

---

## Consideraciones de Seguridad

### Limitaciones Conocidas

- Las contraseñas se almacenan en texto plano en el archivo JSON
- No se implementa encriptación de datos en reposo
- No se utiliza HTTPS para comunicación con APIs (manejado por bibliotecas)
- Sistema diseñado para demostración académica

### Recomendaciones para Producción

- Implementar hash de contraseñas (BCrypt, SHA256)
- Migrar a sistema de base de datos relacional
- Implementar sistema de tokens de autenticación
- Añadir auditoría de acciones del sistema

---

## Limitaciones del Sistema

1. **Persistencia**: Utiliza archivos JSON en lugar de base de datos relacional
2. **Concurrencia**: No maneja múltiples usuarios simultáneos
3. **Seguridad**: Almacenamiento de contraseñas sin encriptación
4. **Conectividad**: Requiere conexión a Internet para funcionalidades de IA
5. **Plataforma**: Limitado a sistemas operativos Windows

---

## Información del Proyecto

### Contexto Académico

**Institución:** Universidad Popular del Cesar  
**Curso:** Programación III  
**Profesor:** John Patiño  
**Período Académico:** 2025-2

### Equipo de Desarrollo

**Integrantes:**
- Pavel Cañas
- Luis Castrillo
- Moisés Franco

### Contacto

**Docente:** johnpatino@unicesar.edu.co

---

## Licencia

Este proyecto fue desarrollado con fines académicos y educativos.

---

## Referencias

- Anthropic. (2025). Claude AI API Documentation. https://docs.anthropic.com
- Microsoft. (2025). Azure OpenAI Service Documentation. https://learn.microsoft.com/azure/ai-services/openai/
- Microsoft. (2025). Windows Presentation Foundation Documentation. https://learn.microsoft.com/dotnet/desktop/wpf/
- NAudio. (2025). Audio and MIDI library for .NET. https://github.com/naudio/NAudio

---

**Nota:** Este sistema proporciona información de primeros auxilios con fines educativos y de orientación. No sustituye el diagnóstico, tratamiento o consejo médico profesional. En caso de emergencia médica, contacte inmediatamente a los servicios de emergencia locales.
