using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Homeworks.Factory
{
    public class BuisnessLogicFactory
    {
        private DbContext context;
        private string assemblyPath;

        public BuisnessLogicFactory(IConfiguration configuration)
        {
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

    public static class BLServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddLogic<T>(this IServiceCollection service)
            where T : class
        {
            return service.AddScoped(p => BuisnessLogicFactory.AddService<T>(p));
        }
    }
}
