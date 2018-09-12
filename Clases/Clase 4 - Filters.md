# Clase 4 - Filters

Los filtros en ASP.NET Core permiten ejecutar código antes o después de etapas específicas en la pipeline de una request.

## Algunos filtros ya construidos:

* Authorization (prevenir acceso a una ruta la cual el usuario no esta autorizado)
* Asegurarce que todas las request usen HTTPS
* Response caching (shot-circuiting de la request pipeline para retornar una respuesta cachada)

Filters pueden ser creados para manejar 'procupaciones' transversales. 
Ej: Manejo de excepciones la pueden realizar los filtros, si un metodo lanza una expcecion es atrapado por un filtro y este retorna un 404, entonces con los filtros consolidamos el manejo de este error.

## Como funcionan los filtros:
Filters corren entre la MVC Action invocation pipeline o fileter pipeliine. La filters pipeline corre despues de que la API 
selecciona una accion que ejecutar.

![FILTERS-PIPELINE](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters/_static/filter-pipeline-1.png?view=aspnetcore-2.1)

## Tipos de filtros:
Tipo | Descripcion
------------ | -------------
[Authorization filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#authorization-filters)| Se ejecutan primiero y son usados para determinar si el usuario actual es autorizado para acceder al recurso actual.
[Resource filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#resource-filters)| Se ejecutan luego de la authorizacion. Y sirven para ejecutar codigo antes y despues de que la pipeline termine. Son utiles para caching o shot-circuit la filter pipeline y asi mejorar la performance.
[Action filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#action-filters)| Sirven para ejecutar codigo antes y despues de una accion (metodo) es invocado. Son utiles para manipular argumentos pasados en la accion en particular.
[Exception filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#exception-filters)| Son usados para aplicar 'politicas' globales para manejar excepciones ocurridas antes de que cualquier cosa sea escrita en el body de la response.
[Result filters](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-2.1#result-filters)| Se ejecutan antes y despues de la ejecucion de un action results. Solo se ejecutan cuando un action method a sido ejecutado exitosamente. Son utiles para crear formateadores.

![FILTERS-PIPELINE2](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters/_static/filter-pipeline-2.png?view=aspnetcore-2.1)

## Implementacion de un Filtro
Vamos a crear ActionFilter para manejar la autehtificacion a nuestra api.
Esto no esta bien ya que se deberia de encargar un [Authorization filters](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/index?view=aspnetcore-2.1), para este ejemplo no lo utlizaremos ya que todo lo relacionado a la authorizacion de usuarios ya se encuentra todo muy digerido y implica otros temas como la generacion de tokens con jwt, que prefiero mostrar un poco como funciona un authorization filter por detras y simplificar los tokens, pero son bienvenidos a usarlo para el obl :smile:

## Creacion de SessionLogic
Esta clase se encargara de hacer ABMs de tokens (guids) que usaremos para identificar que usuario esta realizando la request.
```
public class SessionLogic : IDisposable
{
    // TENDRIA QUE SER UN SESSION REPOSITORY
    // SESSION = {
    //      token: Guid,  
    //      user: User
    // }
    private UserRepository repository;

    public SessionLogic() {
        repository = new UserRepository(ContextFactory.GetNewContext());
    }

    public bool IsValidToken(string token)
    {
        // SI EL TOKEN EXISTE EN BD RETORNA TRUE
        return true;
    }

    public Guid CreateToken(string userName, string password)
    {
        // SI EL USUARIO EXISTE Y LA PASS Y EL USERNAME SON EL MISMO
        // RETORANR GUID
        return Guid.NewGuid();
    }

    public bool HasLevel(string token, string role)
    {  
        // SI EL DUENIO DEL TOKEN TIENE EL ROLE REQUERIDO
        // RETORNA TRUE
        return true;
    }

    public User GetUser(string token) 
    {
        return repository.GetAll().ToList()[0];
    }

    public void Dispose()
    {
        repository.Dispose();
    }
}
```

## Login de usuario
Agregaremos un controller que se encarge de hacer el login de usuarios. 
Este tiene un post que si el username y la password nos genere un token.
```
[Route("api/[controller]")]
public class TokenController : Controller
{
    private SessionLogic sessions;

    public TokenController() 
    {
        sessions = new SessionLogic();
    }

    [HttpPost]
    public IActionResult Login([FromBody]LoginModel model) {
        var token = sessions.CreateToken(model.UserName, model.Password);
        if (token == null) 
        {
            return BadRequest("Invalid user/password");
        }
        return Ok(token);
    }

    protected override void Dispose(bool disposing) 
    {
        try {
            base.Dispose(disposing);
        } finally {
            sessions.Dispose();
        }
    }
}
```

## Creacion del Filtro
Nuestro ActionFilter va a implementar la interfaz **IActionFilter** que tiene los siguientes metodos **OnActionExecuting** (Se ejecuta antes de el action method y **OnActionExecuted** (Se ejecuta despues del action method), y tambien va heredar de **Attribute** que nos permitira usarlo como **tag** en C#

El constructor va a recivir el role del usuario que tiene permitido ejecutar el action mehtod.
Y solo implementaremos **OnActionExecuting** ya que solo nos interesa

```
public class ProtectFilter : Attribute, IActionFilter
{
    private readonly string _role;

    public ProtectFilter(string role) 
    {
        _role = role;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        string token = context.HttpContext.Request.Headers["Authorization"];
        if (token == null) {
            context.Result = new ContentResult()
            {
                Content = "Token is required",
            };
        }
        using (var sessions = new SessionLogic()) {
            if (!sessions.IsValidToken(token)) {
                context.Result = new ContentResult()
                {
                    Content = "Invalid Token",
                };
            }
            if (!sessions.HasLevel(token, _role)) {
                context.Result = new ContentResult()
                {
                    Content = "The user isen't " + _role,
                };   
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}
```
