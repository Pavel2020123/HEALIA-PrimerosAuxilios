# Manual de Usuario - HEALIA

## Sistema de Primeros Auxilios con Inteligencia Artificial

---

## Introducción

HEALIA es un sistema inteligente diseñado para proporcionar asistencia en situaciones de emergencia médica mediante el uso de inteligencia artificial. El sistema ofrece tres modalidades de interacción:

- Análisis de texto
- Análisis de imágenes
- Transcripción y análisis de audio

**AVISO IMPORTANTE:** Este sistema es una herramienta de apoyo educativo. No sustituye la atención médica profesional. En caso de emergencia grave, contacte inmediatamente a los servicios de emergencia (123).

---

## Requisitos del Sistema

- Sistema Operativo: Windows 10 o superior
- Conexión a Internet activa
- Micrófono (para funcionalidad de audio)
- Resolución de pantalla: 1280x720 mínimo

---

## Inicio de la Aplicación

### Primera Ejecución

1. Localizar el archivo ejecutable `GUI.exe`
2. Ejecutar con doble clic
3. Aparecerá la pantalla de autenticación

### Credenciales de Prueba

**Usuario Administrador:**
- Usuario: `admin`
- Contraseña: `admin123`

---

## Módulo de Autenticación

### Iniciar Sesión

**Procedimiento:**

1. Ingresar nombre de usuario en el campo correspondiente
2. Ingresar contraseña
3. Hacer clic en el botón "Iniciar Sesión"
4. El sistema validará las credenciales y permitirá el acceso

**Mensajes de Error:**
- "Campo requerido": Debe completar todos los campos
- "Usuario o contraseña incorrectos": Las credenciales no coinciden con ningún registro

### Registro de Nuevo Usuario

**Procedimiento:**

1. Hacer clic en el enlace "Registrarse"
2. Completar el formulario con la siguiente información:
   - Nombre completo
   - Nombre de usuario (mínimo 3 caracteres)
   - Correo electrónico válido
   - Contraseña (mínimo 6 caracteres)
   - Confirmación de contraseña
3. Hacer clic en "Registrar"
4. El sistema validará los datos y creará la cuenta

**Validaciones del Sistema:**
- El nombre de usuario debe ser único
- El correo electrónico debe ser único
- Las contraseñas deben coincidir
- Todos los campos son obligatorios

### Recuperación de Contraseña

**Procedimiento:**

1. Hacer clic en "¿Olvidaste tu contraseña?"
2. Ingresar nombre de usuario
3. El sistema verificará la existencia del usuario
4. Una vez verificado, habilitar campos para nueva contraseña
5. Ingresar nueva contraseña
6. Confirmar nueva contraseña
7. Hacer clic en "Cambiar Contraseña"

---

## Módulo de Chat con Inteligencia Artificial

### Interfaz Principal

Al acceder al sistema, se presenta la interfaz de chat con las siguientes opciones:

- Campo de texto para escribir mensajes
- Botón para adjuntar imágenes
- Botón para grabar audio
- Área de visualización de mensajes

### Análisis de Texto

**Procedimiento:**

1. Escribir la consulta o descripción de la emergencia en el campo de texto
2. Presionar Enter o hacer clic en el botón "Enviar"
3. El sistema procesará la consulta mediante Claude AI
4. Se mostrará la respuesta con recomendaciones

**Ejemplos de Consultas Válidas:**

- "Tengo una herida que sangra abundantemente en el brazo"
- "Presenta fiebre de 39 grados centígrados desde hace 6 horas"
- "Sufrió una picadura de insecto con inflamación"

**Mejores Prácticas:**

- Ser específico en la descripción
- Incluir síntomas relevantes
- Mencionar duración de los síntomas
- Indicar edad del paciente si es relevante

### Análisis de Imágenes

**Procedimiento:**

1. Hacer clic en el botón de adjuntar archivo
2. Seleccionar una imagen del sistema de archivos
3. Formatos aceptados: JPG, JPEG, PNG, BMP
4. Hacer clic en "Abrir"
5. El sistema procesará la imagen mediante Claude Vision AI
6. Se mostrará el análisis con recomendaciones

**Tipos de Imágenes Apropiadas:**

- Heridas abiertas o cerradas
- Quemaduras de diferentes grados
- Picaduras de insectos
- Erupciones cutáneas
- Lesiones visibles

**Recomendaciones para Captura:**

- Iluminación adecuada
- Imagen enfocada
- Mostrar la zona afectada completa
- Incluir referencia de escala si es posible

### Grabación y Transcripción de Audio

**Procedimiento:**

1. Mantener presionado el botón del micrófono
2. Describir la situación de emergencia de forma clara
3. Soltar el botón para finalizar la grabación
4. El sistema transcribirá automáticamente el audio mediante Whisper AI
5. Se mostrará la transcripción
6. Claude AI procesará la transcripción y proporcionará recomendaciones

**Requisitos de Grabación:**

- Duración mínima: 1 segundo
- Hablar de forma clara y pausada
- Minimizar ruido ambiental
- Utilizar terminología médica cuando sea posible

---

## Módulo de Historial de Conversaciones

### Acceso al Historial

**Procedimiento:**

1. Hacer clic en "Historial" en el menú lateral
2. Se mostrará la lista de conversaciones guardadas

### Visualización de Conversaciones

**Información Mostrada:**

- Título de la conversación
- Fecha y hora de última modificación
- Número de mensajes

**Usuarios Regulares:**
- Solo visualizan sus propias conversaciones

**Usuarios Administradores:**
- Visualizan todas las conversaciones del sistema
- Se indica el usuario propietario de cada conversación

### Gestión de Conversaciones

**Abrir Conversación:**
1. Hacer clic sobre la conversación deseada
2. Se cargará en la vista de chat

**Eliminar Conversación:**
1. Hacer clic en el icono de eliminación
2. Confirmar la acción
3. La conversación será eliminada permanentemente

**Iniciar Nueva Conversación:**
1. Hacer clic en "Nueva Conversación"
2. La conversación actual se guardará automáticamente
3. Se iniciará una nueva sesión de chat

---

## Módulo de Mapas

### Acceso y Funcionalidad

**Procedimiento:**

1. Hacer clic en "Mapa" en el menú lateral
2. Se desplegará la vista de mapas integrada
3. Permite visualizar ubicaciones geográficas
4. Útil para localizar centros médicos cercanos

---

## Gestión de Cuenta de Usuario

### Acceso a Configuración

**Procedimiento:**

1. Hacer clic en "Mi Cuenta" en el menú lateral
2. Acceder a opciones de configuración

### Cambio de Contraseña

**Procedimiento:**

1. Seleccionar opción de cambio de contraseña
2. Ingresar nueva contraseña
3. Confirmar nueva contraseña
4. Guardar cambios

### Cerrar Sesión

**Procedimiento:**

1. Hacer clic en "Mi Cuenta"
2. Seleccionar "Cerrar Sesión"
3. Confirmar acción
4. Se retornará a la pantalla de autenticación

---

## Funcionalidades Administrativas

### Requisitos

- Rol de administrador
- Credenciales: admin/admin123

### Capacidades Adicionales

**Gestión de Conversaciones:**
- Visualización de todas las conversaciones del sistema
- Identificación de usuario propietario
- Capacidad de eliminación de cualquier conversación

**Gestión de Usuarios:**
- Visualización de lista completa de usuarios registrados
- Acceso a información de cuentas

---

## Resolución de Problemas

### Problemas de Autenticación

**Error: "Usuario o contraseña incorrectos"**
- Verificar que las credenciales sean correctas
- Asegurar que no haya espacios adicionales
- Verificar que Bloq Mayús no esté activado

**Error: "El usuario ya existe"**
- El nombre de usuario ya está registrado
- Seleccionar un nombre de usuario diferente

### Problemas de Conectividad

**Error: "Error de conexión"**
- Verificar conexión a Internet
- Comprobar que las API keys estén configuradas
- Reiniciar la aplicación

### Problemas con Audio

**Error: "Audio muy corto"**
- Mantener presionado el botón al menos 1 segundo
- Verificar que el micrófono esté funcionando

**Error: "No se pudo transcribir el audio"**
- Hablar más claramente
- Reducir ruido ambiental
- Verificar configuración del micrófono

---

## Preguntas Frecuentes

### ¿El sistema almacena mis conversaciones?

Sí, todas las conversaciones se guardan automáticamente en el sistema de archivos local de su computadora.

### ¿Son privadas mis conversaciones?

- Usuarios regulares: Solo usted puede ver sus conversaciones
- Usuario administrador: Tiene acceso a todas las conversaciones

### ¿Funciona sin conexión a Internet?

No, el sistema requiere conexión a Internet para acceder a los servicios de inteligencia artificial.

### ¿Puedo confiar en las recomendaciones del sistema?

Las recomendaciones son de carácter orientativo. Siempre consulte con un profesional médico para diagnóstico y tratamiento.

### ¿Cómo elimino mi cuenta?

Contacte al administrador del sistema para gestionar la eliminación de cuenta.

---

## Limitaciones del Sistema

1. **Alcance:** El sistema proporciona información general, no diagnósticos médicos definitivos
2. **Conectividad:** Requiere conexión permanente a Internet
3. **Idioma:** Optimizado para español
4. **Precisión:** Las recomendaciones dependen de la calidad de la información proporcionada

---

## Casos de Uso Apropiados

### Situaciones Adecuadas

- Primeros auxilios básicos
- Orientación sobre síntomas menores
- Evaluación preliminar de lesiones
- Información sobre procedimientos básicos

### Situaciones que Requieren Atención Médica Inmediata

- Dolor intenso en el pecho
- Dificultad respiratoria severa
- Sangrado abundante que no se detiene
- Pérdida de consciencia
- Trauma craneal
- Quemaduras extensas o de tercer grado
- Fracturas evidentes
- Intoxicaciones

**En estos casos, contacte inmediatamente al 123 (servicios de emergencia)**

---

## Soporte Técnico

### Problemas Técnicos

Para problemas relacionados con el funcionamiento del sistema:

1. Reiniciar la aplicación
2. Verificar conexión a Internet
3. Comprobar configuración del sistema
4. Consultar con el administrador o docente

### Contacto Académico

**Docente Responsable:**  
John Patiño  
Correo: johnpatino@unicesar.edu.co

---

## Aviso Legal

Este sistema fue desarrollado con fines académicos y educativos. La información proporcionada tiene carácter orientativo y no sustituye el criterio profesional de personal médico calificado.

El usuario reconoce que:

1. Las recomendaciones son generadas por inteligencia artificial
2. No constituyen diagnóstico médico profesional
3. En emergencias reales debe contactar servicios de salud
4. El sistema no se responsabiliza por decisiones tomadas basándose únicamente en sus recomendaciones

---

## Conclusión

HEALIA es una herramienta de apoyo educativo que combina tecnología de inteligencia artificial con una interfaz intuitiva para proporcionar orientación en situaciones de primeros auxilios. 

Utilice el sistema de manera responsable y siempre priorice la atención médica profesional en casos de emergencia.

---

**Versión del Manual:** 1.0  
**Fecha:** Diciembre 2025  
**Sistema:** HEALIA - Primeros Auxilios con IA
