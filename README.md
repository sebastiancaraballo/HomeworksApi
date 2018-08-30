# Clase 2

## Instalacion de ambiente

* [.NET Core 2.1 downloads](https://www.microsoft.com/net/download/dotnet-core/2.1)
* [Visual Code](https://code.visualstudio.com/Download)
* [Postman] (https://www.getpostman.com/apps)

Commando | Resultado
------------ | -------------
dotnet new sln| Creamos solucion (solo útil para VisualStudio, cuando queremos abrir la solución y levantar los proyectos asociados)
dotnet new webapi -au none -n "Nombre del Proyecto"| Crear un nuevo Proyecto del template WebApi (-ua none implica que el proyecto sin autentificación)
dotnet sln add | Asociamos el proyecto creado al .sln
dotnet new classlib -n "Nombre del Proyecto"| Crear un nueva libreria (standard)
dotnet add "Nombre del Proyecto 1".csproj reference "Nombre del Proyecto 2".csproj| Agrega una referencia al Proyecto 1 del Proyecto 2
dotnet add package "Nombre del Package" | Instala la Package al proyecto actual


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
