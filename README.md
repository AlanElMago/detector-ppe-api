# Detector PPE Web API
El Detector PPE Web API es una herramienta diseñada para facilitar la notificación de incumplimientos en el uso de equipo de protección personal (PPE) en entornos industriales. Esta aplicación web funciona en conjunto con el Detector PPE, un sistema de detección EPP que utiliza tecnología de visión artificial para identificar a los empleados que no llevan equipo de protección adecuado.

## Objetivo General
El objetivo principal del Detector PPE Web API es establecer un canal de comunicación entre el Detector PPE y un supervisor (ej. un inspector) designado dentro del entorno industrial. Al detectar que un empleado no lleva el EPP requerido, el Detector PPE captura una imagen como evidencia y utiliza la aplicación web para enviar un mensaje de alerta al supervisor a través de WhatsApp.

## Funcionamiento
El Detector PPE Web API se basa en una arquitectura cliente-servidor, donde el Detector PPE actúa como cliente y el servidor web aloja el API. El proceso de comunicación se desarrolla de la siguiente maner:
1. **Detección de Incumplimiento:** El detector PPE identifica a un empleado que no lleva el EPP requerido.
2. **Captura de Evidencia:** El detector PPE captura una imagen del empleado como evidencia del incumplimiento.
3. **Generación de Mensaje de Alerta:** El Detector PPE genera un mensaje de alerta, junto con la imagen de evidencia, que informa sobre el incumplimiento.
4. **Envío de Alerta a través de WhatsApp:** El Detector PPE se comunica con el Detector PPE Web API, que utiliza la API de Meta (conocida como Graph API), para enviar el mensaje de alerta y la imagen capturada al número de WhatsApp del supervisor designado.

# Tecnologías Empleadas
- **ASP.NET Core (versión 8.0):** Un marco de trabajo de código abierto desarrollado por Microsoft para la elaboración de aplicaciones web modernas.
- **Docker:** Una plataforma para la virtualización de aplicaciones, lo que permite ejecutar el web API en cualquier entorno de servidor de manera consistente.
- **Microsoft Azure:** Una suite de servicios en la nube desarrollado por Microsoft que proporciona una infraestructura para implementar, escalar y administrar la aplicación web.
- **Graph API (versión 18.0):** La API de Meta para interactuar con la plataforma de WhatsApp, permitiendo enviar mensajes de texto e imágenes a través de la aplicación.
