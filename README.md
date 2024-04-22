# SmartDoc 📄

**SmartDoc** es un sistema avanzado diseñado para la clasificación y análisis de documentos, utilizando tecnologías de inteligencia artificial para analizar facturas y realizar análisis de sentimientos. Este sistema aprovecha los servicios cognitivos de Azure para proporcionar análisis detallados y precisos.

## Tecnologías Utilizadas 🛠️

- **.Net 8**: Usado para la construcción de la API y la lógica de aplicación, proporcionando una base robusta para servicios web.
- **Entity Framework Core**: Maneja las operaciones de base de datos y las migraciones, asegurando un acceso eficiente y escalable a los datos.
- **Razor**: Empleado en la creación de componentes interactivos para la interfaz de usuario en la aplicación web.
- **Azure Cognitive Services**: Clave para los servicios de análisis de documentos y análisis de sentimientos.
- **Serilog**: Utilizado para el manejo avanzado de registros, facilitando la depuración y el monitoreo del rendimiento de la aplicación.
- **MediatR**: Utilizado para implementar notificaciones que gestionan y registran los LOGS del análisis de archivos, facilitando un flujo de trabajo basado en eventos dentro de la aplicación.
- **Bootstrap**: Proporciona estilos y componentes de interfaz de usuario listos para usar, garantizando una experiencia de usuario coherente y profesional.
- **LINQ (Language Integrated Query)**: Facilita la manipulación y consulta de datos, permitiendo un acceso más intuitivo y eficiente a la información almacenada.
- **SQL Server**: Sistema de gestión de bases de datos que almacena y recupera los datos requeridos por otras aplicaciones de software, sea cual sea el modo en que estén implementadas.
- **GitHub**: Utilizado para el control de versiones y la colaboración, permitiendo que múltiples desarrolladores trabajen juntos en el proyecto de forma eficaz.

## Estructura del Proyecto 🏗️

El proyecto está organizado en N capas y servicios:

- **SmartDoc.Api**: Actúa como la capa de presentación, exponiendo la funcionalidad del sistema a través de una API web.
- **SmartDoc.BL**: Contiene toda la lógica de negocios, incluyendo los servicios de clasificación de documentos, análisis de facturas y análisis de sentimientos.
- **SmartDoc.Data y SmartDoc.DataAccess**: Administran la configuración y las operaciones relacionadas con la base de datos.
- **SmartDoc.WebApp**: Ofrece una interfaz de usuario basada en la web para interactuar con el sistema a través de componentes Razor.

## Configuración y Despliegue 🔧

- **Configuración**: Los servicios y configuraciones esenciales están definidos en el archivo `appsettings.json` de SmartDoc.Api, incluyendo conexiones a bases de datos y configuraciones de servicios externos.
- **Inyección de Dependencias**: Establecida en `DIContainer.cs`, facilita la gestión y extensión de los servicios utilizados a lo largo de la aplicación.
- **Aplicación Web**: Configurada para utilizar componentes Razor y manejar la interactividad del usuario, asegurando una experiencia de usuario fluida y responsiva.
