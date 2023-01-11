using MFramework.Services.FakeData;
using Microsoft.EntityFrameworkCore;

namespace Todo.API.Entities
{
    public class DatabaseContext : DbContext
    {
        private IConfiguration _configuration;

        public DatabaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
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

                }
            }
            _configuration = configuration;
        }

        public DbSet<TodoItem> Todos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
