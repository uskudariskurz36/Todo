using System.ComponentModel.DataAnnotations;

namespace Todo.API.Entities
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [StringLength(500)]
        public string Text { get; set; }

        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Role { get; set; }
    }
}
