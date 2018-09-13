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
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            DbContextOptions options = null;
            if (type == ContextType.MEMORY) {
                options = GetMemoryConfig(builder);
            } else {
                options = GetSqlConfig(builder);
            }
            return new HomeworksContext(options);
        }

        private static DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder) 
        {
            builder.UseInMemoryDatabase("HomeworksDB");
            return builder.Options;
        }

        private static DbContextOptions GetSqlConfig(DbContextOptionsBuilder builder) 
        {
            builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=HomeworksDB;
                Trusted_Connection=True;MultipleActiveResultSets=True;");
            return builder.Options;
        }   
    }
}