# Instrucciones para generar la estructura de carpetas en src

Por favor, genera la siguiente estructura de carpetas dentro de la carpeta `learn-hub-back` para organizar el proyecto de manera eficiente. Esta estructura incluye carpetas para el nivel de API, Application, Domain, Host, Infrastructure y Tests.

## Estructura de carpetas

Dentro de la carpeta `learn-hub-back`, crea las siguientes subcarpetas:

```
src/
├── Api/             	# Proyecto con la lógica del API
├── Application/        # Proyecto para los handlers
├── Domain/            	# Proyecto para las entidades de dominio
├── Host/          		# Proyecto que contiene el archivo host de la aplicación
├── Infrastructure/     # Proyecto para la infraestructura
├── Tests/          	# Proyecto para test
```

## Descripción de cada carpeta

1. **Api/**: Aquí se deben colocar los controladores que se usarán en toda la aplicación, además de todos los archivos de configuración referentes a estos controladores. También estarán los middleware que tuviera la aplicación o los archivos referentes a la autorización del API.

2. **Application/**: En esta carpeta se colocan los handlers y request de la aplicación, así como los archivos de respuesta y DTOs de entrada de los controladores y sus validaciones.

3. **Domain/**: Esta carpeta contiene todas las entidades de base de datos que tendremos en la aplicación. También contendrá los enumerados o constantes de la aplicación.

4. **Host/**: Aquí irá el propio proyecto de Host con la clase inicial 'Program' y todos los archivos de configuración de la aplicación. También estarán los archivos appsettings con la configuración de la aplicación.

5. **Infrastructure/**: En esta carpeta estarán las migraciones de la aplicación. También los posibles 'Commands', 'Queries' y servicios de acceso a la base de datos además de sus interfaces.

6. **Tests/**: Esta carpeta contendrá los archivos de tests, tanto unitarios como funcionales, de la aplicación.

## Cómo generar las carpetas

Para crear esta estructura de carpetas, simplemente usa el siguiente comando en la terminal, si estás en el directorio raíz del proyecto:

```
mkdir -p src/Api src/Application src/Domain src/Host src/Infrastructure src/Tests
```

SOLO cuando ya hayas creado la estructura de carpetas anterior, quiero que crees un proyecto en las siguientes carpetas:

### src/Api
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Api
```

### src/Application
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Application
```

### src/Domain
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Domain
```

### src/Host
Usa el siguiente comando en la terminal:
```
dotnet new webapp -n LearnHub.Back.Host
dotnet sln add src/Host/LearnHub.Back.Host.csproj
```

### src/Infrastructure
Usa el siguiente comando en la terminal:
```
dotnet new classlib -n LearnHub.Back.Infrastructure
```

### src/Tests
Usa el siguiente comando en la terminal:
```
dotnet new nunit -n LearnHub.Back.Tests
dotnet sln add src/Tests/LearnHub.Back.Tests.csproj
```

### Contenido Directory.Build.props
Usa el siguiente comando en la terminal para crear el archivo `Directory.Build.props` en la raíz del proyecto:
```powershell
$propsContent = @"
<Project>
  <PropertyGroup>
    <MicrosoftAspNetCoreAuthenticationJwtBearerVersion>8.0.0</MicrosoftAspNetCoreAuthenticationJwtBearerVersion>
    <MicrosoftAspNetCoreMvcNewtonsoftJsonVersion>8.0.0</MicrosoftAspNetCoreMvcNewtonsoftJsonVersion>
    <MicrosoftAspNetCoreMvcVersion>2.3.0</MicrosoftAspNetCoreMvcVersion>
    <MediatRVersion>12.4.1</MediatRVersion>
    <MicrosoftEntityFrameworkCoreVersion>9.0.2</MicrosoftEntityFrameworkCoreVersion>
    <MicrosoftEntityFrameworkCoreSqlServerVersion>9.0.2</MicrosoftEntityFrameworkCoreSqlServerVersion>
    <MicrosoftEntityFrameworkCoreToolsVersion>9.0.2</MicrosoftEntityFrameworkCoreToolsVersion>
    <NUnitVersion>4.3.2</NUnitVersion>
    <MoqVersion>4.20.72</MoqVersion>
    <FluentValidationVersion>10.3.6</FluentValidationVersion>
    <FluentValidationDependencyInjectionExtensionsVersion>10.3.0</FluentValidationDependencyInjectionExtensionsVersion>
    <AutoMapperVersion>10.1.1</AutoMapperVersion>
  </PropertyGroup>
</Project>
"@
```

### Crear Directory.Build.props
```powershell
$propsContent | Out-File -FilePath Directory.Build.props -Encoding utf8
```

#### Restaurar paquetes
```sh
dotnet restore
```

### Añadir referencias y paquetes
Usa los siguientes comandos en la terminal para añadir las referencias y paquetes necesarios para cada proyecto:

# Api
Usa los siguientes comandos en la terminal:
dotnet add src/Api/LearnHub.Back.Api.csproj reference src/Application/LearnHub.Back.Application.csproj src/Domain/LearnHub.Back.Domain.csproj src/Infrastructure/LearnHub.Back.Infrastructure.csproj
dotnet add src/Api/LearnHub.Back.Api.csproj package Microsoft.AspNetCore.Mvc --version 2.3.0

# Application
Usa los siguientes comandos en la terminal:
dotnet add src/Application/LearnHub.Back.Application.csproj reference src/Domain/LearnHub.Back.Domain.csproj src/Infrastructure/LearnHub.Back.Infrastructure.csproj
dotnet add src/Application/LearnHub.Back.Application.csproj package MediatR --version 12.4.1
dotnet add src/Application/LearnHub.Back.Application.csproj package FluentValidation --version 10.3.6
dotnet add src/Application/LearnHub.Back.Application.csproj package FluentValidation.DependencyInjectionExtensions --version 10.3.0
dotnet add src/Application/LearnHub.Back.Application.csproj package AutoMapper --version 10.1.1

# Domain
Usa los siguientes comandos en la terminal:
dotnet add src/Domain/LearnHub.Back.Domain.csproj package Microsoft.EntityFrameworkCore --version 9.0.2

# Host
Usa los siguientes comandos en la terminal:
dotnet add src/Host/LearnHub.Back.Host.csproj reference src/Api/LearnHub.Back.Api.csproj
dotnet add src/Host/LearnHub.Back.Host.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add src/Host/LearnHub.Back.Host.csproj package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 8.0.0

# Infrastructure
Usa los siguientes comandos en la terminal:
dotnet add src/Infrastructure/LearnHub.Back.Infrastructure.csproj reference src/Domain/LearnHub.Back.Domain.csproj
dotnet add src/Infrastructure/LearnHub.Back.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.2
dotnet add src/Infrastructure/LearnHub.Back.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools --version 9.0.2
dotnet add src/Infrastructure/LearnHub.Back.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design --version 9.0.2

# Test
Usa los siguientes comandos en la terminal:
dotnet add src/Tests/LearnHub.Back.Tests.csproj reference src/Api/LearnHub.Back.Api.csproj src/Application/LearnHub.Back.Application.csproj src/Domain/LearnHub.Back.Domain.csproj src/Infrastructure/LearnHub.Back.Infrastructure.csproj
dotnet add src/Tests/LearnHub.Back.Tests.csproj package NUnit --version 4.3.2
dotnet add src/Tests/LearnHub.Back.Tests.csproj package Moq --version 4.20.72