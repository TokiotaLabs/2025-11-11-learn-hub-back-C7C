IMAGINA que eres un experto desarrollador backend de software con especialización en .NET Core 8.0. Configura una base de datos SQL Server para un proyecto que utiliza Mediator con los siguientes requerimientos:

- El modelo debe responder SIEMPRE en el idioma en el que el usuario pregunta. Si el usuario usa inglés, responder en inglés. Si usa español, responder en español, etc.
- Asume que el proyecto .NET Core 8.0 ya ha sido creado y se han instalado todos los paquetes necesarios para hacer las migraciones.
- IMPORTANTE: Los comandos que vayas a ejecutar deberás ejecutarlos OBLIGATORIAMENTE de uno en uno y separados en varios pasos. No puedes usar los operadores '&&' para concatenar varios comandos, debes ejecutar obligatoriamente de uno en uno.
- Si algún comando falla deberás buscar una solución para conseguir el objetivo completo indicado.

### Requisitos de Estructura del Proyecto:

1 Crear los paquetes y dependencias necesarios de Entity Framework Core
2 Configurar el ApplicationDbContext
3 El archivo ApplicationDbContext deberás crearlo en la raíz del proyecto de Infrastructure. En este archivo se deben crear en el método 'OnModelCreating' todas las configuraciones de todos los atributos de todas las clases de dominio y todas sus relaciones.
4 Implementar gestión de cadenas de conexión
5 Configurar inyección de dependencias para el contexto de base de datos

### Detalles de Configuración de Base de Datos:

1 Usar SQL Server como proveedor de base de datos
2 Implementar enfoque code-first con migraciones
<!-- 3 Configurar almacenamiento seguro de cadenas de conexión en appsettings.json -->
3 Incluir configuración para entornos de desarrollo y producción

### Necesidades Específicas de Implementación:

1 Crear una clase base ApplicationDbContext
2 Configurar entidades usando Fluent API
3 Las migraciones SIEMPRE tienes que crearlas dentro de una carpeta 'Migrations' en el proyecto de LearnHub.Back.Infrastructure.
Deberás solicitar al usuario la siguiente información:
<migration_name>: es el nombre que le darás a la nueva migración. Sustituye este valor que te pasen por el atributo <migration_name> del comando. IMPORTANTE: Espera a que el usuario te de la información para continuar.

Antes de ejecutar cualquier comando sobre migraciones, SIEMPRE deberás posicionarte en el proyecto de Infrastructure.

```
dotnet ef migrations add "<migration_name>" --output-dir src\Infrastructure\Migrations
```
Una vez ejecutado el comando, asegúrate de que la migración se ha creado correctamente en la carpeta indicada.
SOLO si se ha creado correctamente, debes actualizar la base de datos para insertar la nueva migración. Para ello deberás ejecutar el siguiente comando:
```
dotnet ef database update
```
4 Configurar manejo de errores y logging para operaciones de base de datos

### Consideraciones de Seguridad:

1 Implementar almacenamiento seguro de cadenas de conexión
2 Configurar secrets para desarrollo
3 Configurar mecanismos de autenticación apropiados

### Requisitos Adicionales:

1 Incluir ejemplos de operaciones CRUD
2 Agregar funcionalidad de data seeding
3 Configurar estrategias de indexación de base de datos
4 Implementar mejores prácticas para optimización de rendimiento


Una vez hayas terminado todo el proceso, informa al usuario con un mensaje en el chat de que has terminado de ejecutar todo satisfactoriamente.