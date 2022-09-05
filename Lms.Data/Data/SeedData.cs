using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static LmsapiContext db = default!;
        public static async Task initAsync(LmsapiContext context, IServiceProvider services)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            db = context;
            if (await db.Course.AnyAsync()) return;

            var faker = new Faker();
            var courses = new List<Course>();

            for (int i = 0; i < 20; i++)
            {
                courses.Add(new Course
                {
                    Title = faker.Company.CompanySuffix() + faker.Random.Word(),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20)),
                    Modules = new Module[]
                    {
                        new Module
                        {
                            Title = faker.Commerce.ProductName(),
                            StartDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20))
                        },
                        new Module
                        {
                            Title = faker.Commerce.ProductName(),
                            StartDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20))
                        },
                        new Module
                        {
                            Title = faker.Commerce.ProductName(),
                            StartDate = DateTime.Now.AddDays(faker.Random.Int(-20, 20))
                        }
                    }
                });





            }
            db.AddRange(courses);
            await db.SaveChangesAsync();

           
        }
    }

}