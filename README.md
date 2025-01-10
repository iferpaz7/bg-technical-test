# BG.API

Este proyecto es una API desarrollada en .NET 9, diseñada para manejar la lógica de negocio del sistema. A continuación, se explican los pasos para configurar y ejecutar el proyecto, incluyendo la aplicación de migraciones y la documentación generada con Swagger.

---

## Requisitos Previos

- **.NET 9 SDK**: Asegúrate de tener instalado el SDK de .NET 9.
- **SQL Server**: Necesitarás una instancia de SQL Server para la base de datos.
- **Visual Studio o VS Code**: Editor recomendado para ejecutar y depurar la API.

---

## Configuración Inicial

1. **Clonar el repositorio**:
   ```bash
   git clone <url-del-repositorio>
   cd BG.API
   ```

2. **Configurar la cadena de conexión**:
   - Abre el archivo `appsettings.json`.
   - Actualiza la clave `ConnectionStrings:DefaultConnection` con los datos de tu servidor SQL Server.
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=TU_SERVIDOR;Database=TU_BASE_DE_DATOS;User Id=USUARIO;Password=CONTRASEÑA;"
     }
     ```

---

## Aplicar Migraciones

1. **Aplicar las migraciones**:
   Para actualizar la base de datos con las migraciones existentes, ejecuta el siguiente comando desde la carpeta de `BG.Infrastructure`:
   ```bash
   dotnet ef --startup-project ../../BG/BG.API database update
   ```

---

## Documentación con OpenAPI y Swagger

1. **Habilitar Swagger**:
   La documentación de la API está habilitada utilizando Swagger. No se requiere configuración adicional.

2. **Ejecutar la API**:
   Inicia la API desde Visual Studio, VS Code o con el siguiente comando:
   ```bash
   dotnet run
   ```

3. **Acceder a la documentación**:
   Abre tu navegador y navega a:
   ```
   https://localhost:7088/swagger
   ```
   Aquí encontrarás una interfaz interactiva para probar los endpoints de la API y consultar su documentación.

---

## Estructura del Proyecto

El sistema está organizado en una arquitectura por capas para mantener una separación clara de responsabilidades. Las carpetas principales son:

- **BG.API**: Proyecto principal que expone la API.
- **BG.Application**: Contiene la lógica de aplicación, como casos de uso.
- **BG.Core**: Define las entidades y las interfaces del dominio.
- **BG.Infrastructure**: Implementa las dependencias de infraestructura, como acceso a datos.
- **Common**: Provee utilidades compartidas entre los proyectos.

---

## Notas

- Si encuentras problemas durante la configuración o ejecución, asegúrate de revisar los logs generados por la API.
- Siempre valida que las migraciones se apliquen en un entorno de desarrollo antes de pasarlas a producción.

