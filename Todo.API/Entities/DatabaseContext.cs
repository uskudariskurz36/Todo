using MFramework.Services.FakeData;
using Microsoft.EntityFrameworkCore;

namespace Todo.API.Entities
{
    public class DatabaseContext : DbContext
    {
        private IConfiguration _configuration;

        public DatabaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;

            if (Database.CanConnect())
            {
                if (Todos.Any() == false)
                {

                    for (int i = 0; i < 10; i++)
                    {
                        TodoItem t = new TodoItem
                        {
                            DueDate = DateTimeData.GetDatetime(),
                            Text = TextData.GetSentence(),
                            Completed = BooleanData.GetBoolean()
                        };

                        Todos.Add(t);
                    }

                    SaveChanges();

                }

                if (Users.Any() == false)
                {
                    string adminUid = _configuration.GetValue<string>("Authentication:AdminUsername");
                    string adminPwd = _configuration.GetValue<string>("Authentication:AdminPassword");

                    User user = new User
                    {
                        Username = adminUid,
                        Password = adminPwd,
                        Role = "admin"
                    };

                    Users.Add(user);
                    SaveChanges();
                }
            }
            
        }

        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
