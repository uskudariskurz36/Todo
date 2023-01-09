using Microsoft.AspNetCore.Mvc;
using Todo.API.Entities;
using Todo.API.Models;

namespace Todo.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private DatabaseContext _databaseContext;

        public TodoController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_databaseContext.Todos.ToList());
        }


        [HttpPost]
        public IActionResult Create(TodoCreateModel model)
        {
            if (model.Text == "test")
            {
                return BadRequest(model);
            }

            TodoItem item = new TodoItem();
            item.Text = model.Text;
            item.DueDate = model.DueDate;
            item.Completed = false;

            _databaseContext.Todos.Add(item);
            _databaseContext.SaveChanges();

            return Created("", item);
        }
    }
}
