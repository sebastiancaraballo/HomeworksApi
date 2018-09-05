using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;
using Microsoft.EntityFrameworkCore;

namespace Homeworks.DataAccess
{
    public abstract class ContextFactory
    {
        public static HomeworksContext GetNewContext() {
            var builder = new DbContextOptionsBuilder<HomeworksContext>();
            builder.UseInMemoryDatabase("HomeworksDB");
            var options = builder.Options;
            return new HomeworksContext(options);
        }
    }
}