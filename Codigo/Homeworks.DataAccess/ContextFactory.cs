using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Homeworks.DataAccess
{
    public enum ContextType 
    {
        MEMORY, SQL
    }

    public class ContextFactory : IDesignTimeDbContextFactory<HomeworksContext>
    {
        public HomeworksContext CreateDbContext(string[] args) 
        {
            return GetNewContext();
        }

        public static HomeworksContext GetNewContext(ContextType type = ContextType.MEMORY) 
        {
            if (type == ContextType.SQL) {
                return GetSqlContext(@"Server=.\SQLEXPRESS;Database=HomeworksDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
            return GetMemoryContext();
        }

        public static HomeworksContext GetMemoryContext() 
        {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            builder.UseInMemoryDatabase("HomeworksDB");
            return new HomeworksContext(builder.Options);
        }
        
        public static HomeworksContext GetSqlContext(string connection)
        {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            builder.UseSqlServer(connection);
            return new HomeworksContext(builder.Options);
        }
    }
}