# Reflection

## Introducción a Reflection

Reflection es la habilidad de un programa de autoexaminarse con el objetivo de encontrar ensamblados (.dll), módulos, o información de tipos en tiempo de ejecución. En otras palabras, a nivel de código vamos a tener clases y objetos, que nos van a permetir referenciar a ensamblados, y a los tipos que se encuentran contenidos.

Se dice que un programa se refleja en sí mismo (de ahí el termino "reflección"), a partir de extraer metadata de sus assemblies y de usar esa metadata para ciertos fines. Ya sea para informarle al usuario o para modificar su comportamiento.

Al usar Reflection en C#, estamos pudiendo obtener la información detallada de un objeto, sus métodos, e incluso crear objetos e invocar sus métodos en tiempo de ejecución, sin haber tenido que realizar una referencia al ensamblado que contiene la clase y a su namespace.

Específicamente lo que nos permite usar Reflection es el namespace ```System.Reflecion```, que contiene clases e interfaces que nos permiten manejar todo lo mencionado anteriormente: ensamblados, tipos, métodos, campos, crear objetos, invocar métodos, etc.

## Estructura de un assembly/ensamblado

Los assemblies contienen módulos, los módulos contienen tipos y los tipos contienen miembros. Reflection provee clases para encapsular estos elementos. Entonces como dijimos posible utilizar reflection para crear dinámicamente instancias de un tipo, obtener el tipo de un objeto existente e invocarle métodos y acceder a sus atributos de manera dinámica. 
 
![alt text](http://www.codeproject.com/KB/cs/DLR/structure.JPG)

## ¿Para qué podría servir?

Supongamos por ejemplo, que necesitamos que nuestra aplicación soporte diferentes tipos de loggers (mecanismos para registrar datos/eventos que van ocurriendo en el flujo del programa). Además, supongamos que hay desarrolladores terceros que nos brindan una .dll externa que escribe información de logger y la envía a un servidor. En ese caso, tenemos dos opciones:

1) Podemos referenciar al ensamblado directamente y llamar a sus métodos (como hemos hecho siempre) 
2) Podemos usar Reflection para cargar el ensamblado y llamar a sus métodos a partir de sus interfaces.

En este caso, si quisieramos que nuestra aplicación sea lo más desacoplada posible, de manera que otros loggers puedan ser agregados (o 'plugged in' -de ahí el nombre plugin-) de forma sencilla y SIN RECOMPILAR la aplicación, es necesario elegir la segunda opción.

Por ejemplo podríamos hacer que el usuario elija (a medida que está usando la aplicación), y descargue la .dll de logger para elegir usarla en la aplicación. La única forma de hacer esto es a partir de Reflection. De esta forma, podemos cargar ensamblados externos a nuestra aplicación, y cargar sus tipos en tiempo de ejecución. 

## Favoreciendo el desacoplamiento

Lo que es importante para lograr el desacoplamiento de tipos externos, es que nuestro código referencie a una Interfaz, que es la que toda .dll externa va a tener que cumplir. Tiene que existir entonces ese contrato previo, de lo contrario, no sería posible saber de antemano qué metodos llamar de las librerías externas que poseen clases para usar loggers.

## Ejemplo en ```C#```
Ahora tomaremos el ensamblado (dll) Homeworks.Domain y lo moveremos a una carpeta que sepamos para poder inpecionarlo por ejemplo: ```c:\pruebas\```

Lo primero que probaremos será la capacidad de inspección que ofrece reflection sobre los
assemblies. Para ello, en el método Main agregaremos el siguiente códig. Nota: Se deberán agregar algunos imports.

Primero inspeccionamos el assembly:

```C#
using System;
using System.Reflection;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cargamos el assembly de ejemplo en memoria
            Assembly miAssembly = Assembly.LoadFile(@"c:\Pruebas\Homeworks.Domain.dll");
            // Podemos ver que Tipos hay dentro del assembly
            foreach (Type tipo in miAssembly.GetTypes())
            {
                Console.WriteLine(string.Format("Clase: {0}", tipo.Name));

                Console.WriteLine("Propiedades");
                foreach (PropertyInfo prop in tipo.GetProperties())
                {
                    Console.WriteLine(string.Format("\t{0} : {1}", prop.Name, prop.PropertyType.Name));
                }
                Console.WriteLine("Constructores");
                foreach (ConstructorInfo con in tipo.GetConstructors())
                {
                    Console.Write("\tConstructor: ");
                    foreach (ParameterInfo param in con.GetParameters())
                    {
                        Console.Write(string.Format("{0} : {1} ", param.Name, param.ParameterType.Name));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("Metodos");
                foreach (MethodInfo met in tipo.GetMethods())
                {
                    Console.Write(string.Format("\t{0} ", met.Name));
                    foreach (ParameterInfo param in met.GetParameters())
                    {
                        Console.Write(string.Format("{0} : {1} ", param.Name, param.ParameterType.Name));
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
```

Analizar detenidamente la salida.

Acabamos de ver como mediante reflection es posible investigar el contenido de un assembly, obtener su información, como conocer las propiedades, los constructores y los métodos de cada clase. 

![alt text](https://github.com/Sactos/HomeworksApi/blob/master/imgs/reflectionDomain.PNG)

## Instanciando tipos dinámicamente
Como ya hemos mencionado, otra de las pricipales ventajas de reflection es que además de poder conocer información sobre los tipos dentro de un assembly, permite trabajar con ellos de manera dinámica. Para ejemplificarlo, vamos a crear un objeto de la clase User utilizando un constructor con parámetros, le vamos a cambiar el valor de una de sus propiedades y luego le invocaremos un método. Todo esto desde nuestra aplicación de consola, que NO tiene una referencia a la dll con las clases, por lo que todo se hará de manera dinámica.

Cambiemos el contenido del Main por el siguiente:
```C#

static void Main(string[] args)
{
   // Cargamos el assembly en memoria
   Assembly testAssembly = Assembly.LoadFile(@"c:\Pruebas\Homeworks.Domain.dll"); 
   
   // Obtenemos el tipo que representa a User
   Type userType = testAssembly.GetType("Homeworks.Domain.User");
   
   // Creamos una instancia de User
   object userInstance = Activator.CreateInstance(userType);
   
   // O también podemos crearlo pasandole parámetros
   userInstance = Activator.CreateInstance(userInstance, new object[] { "Juan" });
   
   // Lo mostramos
   Console.WriteLine(userInstance.ToString());
   
   //Invocamos al método
   MethodInfo met = userType.GetMethod("IsValid");
   Console.WriteLine(string.Format("Es Valido: {0}", met.Invoke(userInstance, null)));
   
   //También podemos cambiar su nombre
   PropertyInfo prop = userType.GetProperty("Name");
   prop.SetValue(userInstance, "Manuel", null);
   
   Console.ReadLine();  
}

```

IMPORTANTE: aquí estamos asumiendo los nombres de los métodos y llamandolos directamente pasandole Strings como parámetros. Esto en un caso más real no sería correcto, ya que primero deberíamos asegurarnos de que el tipo que queremos instanciar cumple con la interfaz (es decir, tiene los métodos), que queremos usar.

Esto se puede hacer preguntando de la siguiente forma:

```C#

typeof(IMyInterface).IsAssignableFrom(typeof(MyType))
typeof(MyType).GetInterfaces().Contains(typeof(IMyInterface))

```

# Inyección de Dependencias (ID)

## ¿Qué es una dependencia?

En software, cuando hablamos de que dos piezas, componentes, librerías, módulos, clases, funciones (o lo que se nos pueda ocurrir relacionado al área), son dependientes entre sí, nos estamos refiriendo a que uno requiere del otro para funcionar. A nivel de clases, significa que una cierta **'Clase A'** tiene algún tipo de relación con una **'Clase B'**, delegándole el flujo de ejecución a la misma en cierta lógica.
Ej: **UserLogic** *depende de* **UserRepository**

##### Business Logic -> Repository
 
 ```c#
  public class UserLogic : IUserLogic
  {
        public IRepository<User> users;

        public UserLogic()
        {
            users = new new UserRepository();
        }
  }
 ```
 
**¿Notaron el problema (común entre ambas porciones de código) que existe?**
 
El problema reside en que ambas piezas de código tiene la responsabilidad de la instanciación de sus dependencias. Nuestras capas no deberían estar tan fuertemente acopladas y no deberíam ser tan dependientes entre sí. Si bien el acoplamiento es a nivel de interfaz (tenemos IUserLogic y IRepository), la tarea de creación/instanciación/"hacer el new" de los objetos debería ser asignada a alguien más. Nuestras capas no deberían preocuparse sobre la creación de sus dependencias.

**¿Por qué? ¿Qué tiene esto de malo?**:-1:

1. Si queremos **reemplazar** por ejemplo nuestro BreedsBusinessLogic **por una implementación diferente**, deberamos modificar nuestro controller. Si queremos reemplazar nuestro UserRepository por otro, tenemos que modificar nuestra clase UserLogic.

2. Si la UserLogic tiene sus propias dependencias, **debemos configurarlas dentro del controller**. Para un proyecto grande con muchos controllers, el código de configuración empieza a esparcirse a lo largo de toda la solución.

3. **Es muy difícil de testear, ya que las dependencias 'estan hardcodeadas'.** Nuestro controller siempre llama a la misma lógica de negocio, y nuestra lógica de negocio siempre llama al mismo repositorio para interactuar con la base de datos. En una prueba unitaria, se necesitaría realizar un mock/stub las clases dependientes, para evitar probar las dependencias. Por ejemplo: si queremos probar la lógica de UserLogic sin tener que depender de la lógica de la base de datos, podemos hacer un mock de UserRepository. Sin embargo, con nuestro diseño actual, al estar las dependencias 'hardcodeadas', esto no es posible.

Una forma de resolver esto es a partir de lo que se llama, **Inyeccion de Dependencias**. Vamos a inyectar la dependencia de la lógica de negocio en nuestro controller, y vamos a inyectar la dependencia del repositorio de datos en nuestra lógica de negocio. **Inyectar dependencias es entonces pasarle la referencia de un objeto a un cliente, al objeto dependiente (el que tiene la dependencia)**. Significa simplemente que la dependencia es encajada/empujada en la clase desde afuera. Esto significa que no debemos instanciar (hacer new), dependencias, dentro de la clase.

Esto lo haremos a partir de un parámetro en el constructor, o de un setter. Por ejemplo:

 ```c#
  public class UserLogic : IUserLogic
  {
        public IRepository<User> users;

        public UserLogic(IRepository<User> users)
        {
            this.users = users;
        }
  }
 ```
Esto es fácil lograrlo usando interfaces o clases abstractas en C#. Siempre que una clase satisfaga la interfaz,voy a poder sustituirla e inyectarla.

## Ventajas de ID
Logramos resolver lo que antes habíamos descrito como desventajas o problemas.

1. Código más limpio. El código es más fácil de leer y de usar.
2. Nuestro software termina siendo más fácil de Testear. 
3. Es más fácil de modificar. Nuestros módulos son flexibles a usar otras implementaciones. Desacoplamos nuestras capas.
4. Permite NO Violar SRP. Permite que sea más fácil romper la funcionalidad coherente en cada interfaz. Ahora nuestra lógica de creación de objetos no va a estar relacionada a la lógica de cada módulo. Cada módulo solo usa sus dependencias, no se encarga de inicializarlas ni conocer cada una de forma particular.
5. Permite NO Violar OCP. Por todo lo anterior, nuestro código es abierto a la extensión y cerrado a la modificación. El acoplamiento entre módulos o clases es  siempre a nivel de interfaz.

## Problema de la construcción de dependencias:

Vimos como inyectar dependencias a través del constructor. Sin embargo, ahora tenemos un problema, el cuál es dónde construir nuestras dependencias (dónde hacer el **new**).

Para resolver resolver este problema utilizaremos el ServiceProvaider (DependencyInjection) que nos brinda WebApi que nos permite inyectar dependencias.

## Preparando Nuestra WebApi

Lo primero que vamos a hacer es modificar nuestros constructores del las clases DataAcces, Controllers y Logic, para permitir la ID.

***En Nuestro StartUp***
En el metodo ConfigureServices vamos a decirle a nuestra api que implementacion tiene que usar para cada interfaz.
EJ: Aca le estamos diciendo a nuestra WebApi, que cada vez que se presente IUserLogic como dependencia que cree una instancia de UserLogic y la inyecte.
```c#
services.AddScoped<IUserLogic, UserLogic>();
```
Quedando de esta manera:
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    //services.AddDbContext<DbContext, HomeworksContext>(o => 
    //    o.UseSqlServer(Configuration.GetConnectionString("HomeworksDB")));
    services.AddDbContext<DbContext, HomeworksContext>(
        options => options.UseInMemoryDatabase("HomeworksDB"));
    services.AddScoped<IUserLogic, UserLogic>();
    services.AddScoped<IHomeworkLogic, HomeworkLogic>();
    services.AddScoped<IExerciseLogic, ExerciseLogic>();
    services.AddScoped<ISessionLogic, SessionLogic>();
    services.AddScoped<IRepository<User>, UserRepository>();
    services.AddScoped<IRepository<Homework>, HomeworkRepository>();
    services.AddScoped<IRepository<Exercise>, ExerciseRepository>();
}
```
***Para EF Core:*** Con esto le indicamos que cuando se necesite un DbContext se inyecte un HomeworksContext y le va a pasar por el constructor un DbContextOptions confiugrado para usar el SqlServer con un connection string que se encuentra en el archivo de configuracion.
```C#
services.AddDbContext<DbContext, HomeworksContext>(o => o.UseSqlServer(Configuration.GetConnectionString("HomeworksDB")));
```
***Archivo de Configuracion (appsettings.json)*** encontramos la siguiente linea:
```
"ConnectionStrings": {
    "HomeworksDB": "Server=.\\SQLEXPRESS;Database=HomeworksDB;Trusted_Connection=True;MultipleActiveResultSets=True;"
 },
```

Pero con todo esto realizado todavia nuestra WebApi conoce la implementaciones de nuestras interfaces. Para resolver esto vamos a crear un nuevo paquete (```Homeworks.Factory```) que se encargara de realizar estas inyeciones:

Y crearemos la siguiente clase:
```C#
public class BuisnessLogicFactory
{
    private DbContext context;
    private string assemblyPath;

    public BuisnessLogicFactory(IConfiguration configuration)
    {
// ESTE IF ES DE PRE-COMPILACION Y CON EL LE INDICAMOS AL COMPILADO QUE SI ESTA EL FLAG DEBUG ACTIVADO
// EJECUTE EL THEN SI NO EL ELSE
// EN assemblyPath SE ENCUENTRA LA RUTA DE DONDE ENCONTRAREMOS EL DLL DE BUSINESSLOGIC
#if DEBUG
        assemblyPath = AppDomain.CurrentDomain.BaseDirectory + @"Homeworks.BusinessLogic.dll";
#else
        assemblyPath = configuration.GetValue<string>("AssemblyPath");
#endif
        string type = configuration.GetValue<string>("ConnectionType");
        if (type == "MEMORY") {
            context = ContextFactory.GetMemoryContext();
        } else {
            string connection = configuration.GetConnectionString("HomeworksDB");
            context = ContextFactory.GetSqlContext(connection);
        }            
    }

    // ESTE METODO TIENE MUCHO ESPACIO PARA MEJORAR :)
    // CON ESTE METODO EL TIPO QUE NOS PASAN INSTANCIAMOS LA CLASE DESDE EL ASSEMBLY USANDO 
    // ESTE METODO: GetInstanceOfInterface
    // EJ: PODEMOS MEJORAR ESTE METODO INSTANCIANDO LOS REPOSITORIOS DE LA MISMA MANERA
    public object GetService(Type type)
    {
        if (typeof(IUserLogic).Equals(type))
            return GetInstanceOfInterface<IUserLogic>(new UserRepository(context));
        if (typeof(IExerciseLogic).Equals(type))
            return GetInstanceOfInterface<IExerciseLogic>(new ExerciseRepository(context));
        if (typeof(IHomeworkLogic).Equals(type))
            return GetInstanceOfInterface<IHomeworkLogic>(
                new HomeworkRepository(context),
                new ExerciseRepository(context)
            );
        if (typeof(ISessionLogic).Equals(type))
            return GetInstanceOfInterface<ISessionLogic>(new UserRepository(context));
        throw new ArgumentException();
    }

    public T GetService<T>() where T : class
    {
        return GetService(typeof(T)) as T;
    }

    public static T AddService<T>(IServiceProvider service) where T : class
    {
        var factorty = service.GetService(typeof(BuisnessLogicFactory)) as BuisnessLogicFactory;
        return factorty.GetService<T>();
    }

    // ESTE METODO BUSCA LA INTERFAZ EN EL DLL Y SI LA ENCUENTRA LA INSTANCIA SI NO LANZA UN NULL REFERENCE EXCEPTION
    private Interface GetInstanceOfInterface<Interface>(params object[] args)
    {
        try
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            IEnumerable<Type> implementations = GetTypesInAssembly<Interface>(assembly);
            if (implementations.Count() <= 0)
            {
                throw new NullReferenceException(assemblyPath + " don't contains Types that extend from " + nameof(Interface));
            }

            return (Interface)Activator.CreateInstance(implementations.First(), args);
        }
        catch (Exception e)
        {
            throw new Exception("Can't load assembly " + assemblyPath, e);
        }

    }

    private static IEnumerable<Type> GetTypesInAssembly<Interface>(Assembly assembly)
    {
        List<Type> types = new List<Type>();
        foreach (var type in assembly.GetTypes())
        {
            if (typeof(Interface).IsAssignableFrom(type))
                types.Add(type);
        }
        return types;
    }
}
```

A demas de esta clase crearemos el siguiente metodo de extension de ```IServiceCollection``` para que nos resulte mas comodo agregarlos a este.
```c#
public static class BLServiceCollectionServiceExtensions
{
    public static IServiceCollection AddLogic<T>(this IServiceCollection service)
        where T : class
    {
        return service.AddScoped(p => BuisnessLogicFactory.AddService<T>(p));
    }
}
```

Volviendo a nuestro ***StartUp***
Lo actualizaremos para usar nuestro metodo de extension.
Ahora inyectaremos nuestra Factory
Y usamos nuestro metodo de extension para inyectar las interfaces.
```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();

    services.AddScoped<BuisnessLogicFactory>();
    services.AddLogic<IUserLogic>();
    services.AddLogic<IHomeworkLogic>();
    services.AddLogic<IExerciseLogic>();
    services.AddLogic<ISessionLogic>();
}
```

## Obtener un servicio para un filtro
Para obtener un servicio dentro de un filtro se lo vamos a tener que pedir directamente al httpcontext, ya que no podemos inyectar servicio en los constructores de un filtro, si lo usamos como atributo.
Entonces si teniamos en nuestro filtro
```c#
public void OnActionExecuting(ActionExecutingContext context)
{
    //CODIGO ..
    using (var sessions = new SessionLogic()) {
        //CODIGO ...
    }
    //CODIGO ...
}
```
Ahora para para pedir un servicio invocamos el siguiente metodo ```context.HttpContext.RequestServices.GetService(TYPO_DEL_SERVICIO_QUE_BUSCAMOS_INYECTAR)``` que nos retorna un object que es del tipo del servicio. Entonces a tener:
```c#
public void OnActionExecuting(ActionExecutingContext context)
{U
    //CODIGO ..
    using (ISessionLogic sessions = (ISessionLogic)context.HttpContext.RequestServices.GetService(typeof(ISessionLogic))) {
        //CODIGO ...
    }
    //CODIGO ...
}
```

## Mas Info
* [StartUp](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.1)
* [Dependency Injection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1)
