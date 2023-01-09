using System.ComponentModel.DataAnnotations;

namespace Todo.API.Models
{
    public class TodoCreateModel
    {
        [StringLength(500)]
        public string Text { get; set; }

        public DateTime? DueDate { get; set; }
    }
}
