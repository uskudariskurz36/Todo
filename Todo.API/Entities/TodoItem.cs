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
}
