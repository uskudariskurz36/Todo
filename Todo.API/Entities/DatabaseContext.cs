using MFramework.Services.FakeData;
using Microsoft.EntityFrameworkCore;

namespace Todo.API.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
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
            }
        }

        public DbSet<TodoItem> Todos { get; set; }
    }
}
