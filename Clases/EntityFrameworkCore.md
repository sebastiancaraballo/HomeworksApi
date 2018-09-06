# Clase 3 - Entity Framework Core - Code First

## Paquetes Necesarios

Paquete | Descripción
------------ | -------------
Microsoft.EntityFrameworkCore| EF Core
Microsoft.EntityFrameworkCore.Design| Contiene toda la lógica de design-time para EF Core. Contiene clases que nos serviran para indicarle a EF Tools por ejemplo como crear un contexto.
Microsoft.EntityFrameworkCore.SqlServer| Es el provider para la bd Microsoft SQL Server
Microsoft.EntityFrameworkCore.Tool| Este paquete permite la ejecución de comandos de entityframework (dotnet ef). Este permite hacer más fácil realizar varias tareas de EF Core, como: migraciones, scaffolding, etc
Microsoft.EntityFrameworkCore.InMemory| (Opcional) Es un provider para bd en Memoria, es sobretodo útil para testing.


## Commandos para creacion de proyeto HomeworkWebApi

### Creamos el sln para poder abrirlo en vs2017 (opcional)
```
dotnet new sln
```

### Creamos el proyecto webapi y lo agregamos al sln
```
dotnet new webapi -au none -n Homeworks.WebApi
dotnet sln add Homeworks.WebApi\Homeworks.WebApi.csproj
```

### Creamos la libreria businesslogic y la agregamos al sln
```
dotnet new classlib -n Homeworks.BusinessLogic
dotnet sln add Homeworks.BusinessLogic\Homeworks.BusinessLogic.csproj
```

### Creamos la libreria dataaccess y la agregamos al sln
```
dotnet new classlib -n Homeworks.DataAccess
dotnet sln add Homeworks.DataAccess\Homeworks.DataAccess.csproj
```

### Creamos la libreria domain y la agregamos al sln
```
dotnet new classlib -n Homeworks.Domain
dotnet sln add Homeworks.Domain\Homeworks.Domain.csproj
```

### Agregamos referencias de los proyectos a la webapi
```
dotnet add Homeworks.WebApi\Homeworks.WebApi.csproj reference Homeworks.DataAccess\Homeworks.DataAccess.csproj
dotnet add Homeworks.WebApi\Homeworks.WebApi.csproj reference Homeworks.Domain\Homeworks.Domain.csproj
dotnet add Homeworks.WebApi\Homeworks.WebApi.csproj reference Homeworks.BusinessLogic\Homeworks.BusinessLogic.csproj
```

### Agregamos la referencia del domain al dataaccess
```
dotnet add Homeworks.DataAccess\Homeworks.DataAccess.csproj reference Homeworks.Domain\Homeworks.Domain.csproj
```

### Agregamos las referencias de domain y dataaccess a businesslogic
```
dotnet add Homeworks.BusinessLogic\Homeworks.BusinessLogic.csproj reference Homeworks.Domain\Homeworks.Domain.csproj
dotnet add Homeworks.BusinessLogic\Homeworks.BusinessLogic.csproj reference Homeworks.DataAccess\Homeworks.DataAccess.csproj
```

### Descargamos Entity Framework Core
Nos movemos a la carpeta web api (cd Homeworks.WebApi)
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```
Nos movemos a la carpeta dataaccess (cd Homeworks.DataAccess)
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```
