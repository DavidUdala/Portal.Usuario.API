using Bogus;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Utils;
using Portal.Usuario.Infrastructure.DatabaseServices.Context;

namespace Portal.Usuario.Infrastructure.DatabaseServices.Seeders
{
    public static class UserDbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.User.Any())
                return;

            var userAdmin = new User() { BirthDay = DateTime.Now, Email = "admin@admin.com", Name = "admin" ,Password =  HashHelper.ToSha256("admin"), DateCreated = DateTime.Now };

            var faker = new Faker<User>("pt_BR")
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(u => u.Password, f => HashHelper.ToSha256(f.Internet.Password(6)))
                .RuleFor(u => u.BirthDay, f => f.Date.Past(30))
                .RuleFor(u => u.DateCreated, f => f.Date.Past(1));

            var users = faker.Generate(10); // gerar 10 usuários

            context.User.Add(userAdmin);
            context.User.AddRange(users);
            context.SaveChanges();
        }
    }
}