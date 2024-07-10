
using Bogus;
using Bogus.DataSets;
using LMS.api.Data;
using LMS.api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LMS.api.Seed
{
    public class SeedData
    {
        public static Faker faker = new Faker("sv");
        private static IEnumerable<Course>? courses;
        private static Random random = new Random();

        public static async Task InitAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            if (context.Course.Any()) // Check if there are any courses
            {
                return;
            }

            var roles = new List<ApplicationRole>
            {
                new ApplicationRole { Name = ApplicationRole.Admin },
                new ApplicationRole { Name = ApplicationRole.Teacher },
                new ApplicationRole { Name = ApplicationRole.Student }
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            courses = GenerateCourses(60);
            await context.Course.AddRangeAsync(courses); // Add generated courses to the context
            await context.SaveChangesAsync(); // Save changes to the database

            await GenerateStudentsAsync(1000, userManager);
        }

        private static IEnumerable<Course> GenerateCourses(int numberOfCourses)
        {
            var courses = new List<Course>();

            for (int i = 0; i < numberOfCourses; i++)
            {
                var title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(faker.Company.Bs());
                var course = new Course
                {
                    Title = title,
                    Description = faker.Lorem.Sentences(),
                    Modules = GenerateModules(random.Next(1, 4)).ToList(),
                    MaxCapcity = random.Next(20, 100),
                    Start = DateTime.Now.AddDays(random.Next(-100, -10)),
                    End = DateTime.Now.AddDays(random.Next(10, 100))
                };
                courses.Add(course);
            }
            return courses;
        }

        private static Course SelectACourse()
        {
            int next = random.Next(0, courses?.Count() ?? 0); // Use null-conditional operator
            return courses?.ElementAt(next); // Use null-conditional operator
        }

        // Other methods remain unchanged
        private static IEnumerable<Module> GenerateModules(int numberOfModules)
        {
            var modules = new List<Module>();

            for (int i = 0; i < numberOfModules; i++)
            {
                var module = new Module
                {
                    Title = faker.Lorem.Slug(3),
                    Description = faker.Lorem.Sentences(),
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(random.Next(0, 100)),
                    Activities = GenerateActivities(random.Next(1, 5)).ToList()
                };
                modules.Add(module);
            }

            return modules;
        }

        private static IEnumerable<Activity> GenerateActivities(int numberOfActivities)
        {
            var activities = new List<Activity>();

            for (int i = 0; i < numberOfActivities; i++)
            {
                var activity = new Activity
                {
                    Title = faker.Lorem.Slug(1),
                    Description = faker.Lorem.Sentence(),
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(random.Next(0, 100))
                };
                activities.Add(activity);
            }

            return activities;
        }

        private static async Task GenerateStudentsAsync(int numberOfStudents, UserManager<ApplicationUser> userManager)
        {
            for (int i = 0; i < numberOfStudents; i++)
            {
                var fName = faker.Name.FirstName();
                var lName = faker.Name.LastName();
                var email = faker.Internet.Email(fName, lName, "lexicon.se");
                var course = SelectACourse();

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = fName,
                    LastName = lName,
                    Course = course
                };

                var result = await userManager.CreateAsync(user, "DefaultPassword123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, ApplicationRole.Student);
                }
            }
        }
    }
}
