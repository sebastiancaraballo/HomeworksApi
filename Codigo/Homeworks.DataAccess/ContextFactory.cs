using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Homeworks.DataAccess
{
    public class ContextFactory : IDesignTimeDbContextFactory<HomeworksContext>
    {
        public static HomeworksContext GetNewContext() {
            return GetNewMemoryContext();
        }

        public static HomeworksContext GetNewMemoryContext() {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            builder.UseInMemoryDatabase("HomeworksDB");
            var options = builder.Options;
            return new HomeworksContext(options);
        }

        public static HomeworksContext GetNewSqlContext() {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            builder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=HomeworksDB;
                Trusted_Connection=True;MultipleActiveResultSets=True;");
            var options = builder.Options;
            return new HomeworksContext(options);
        }

        public HomeworksContext CreateDbContext(string[] args)
        {
            return GetNewContext();
        }
    }
}