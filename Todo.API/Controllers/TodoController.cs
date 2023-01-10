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

        /// <summary>
        /// Verileri sıralar ve beli adette çekmemizi sağlar.
        /// </summary>
        /// <param name="count">Adet. örn; 5</param>
        /// <param name="order">Sıralama. örn; "asc" ya da "desc"</param>
        /// <returns></returns>
        [HttpGet("{count}/{order}")]
        public IActionResult List(int count, string order)
        {
            List<TodoItem> items = null;

            if (order == "asc")
                items = _databaseContext.Todos.OrderBy(x => x.Id).Take(count).ToList();
            else if (order == "desc")
                items = _databaseContext.Todos.OrderByDescending(x => x.Id).Take(count).ToList();
            else
                items = _databaseContext.Todos.Take(count).ToList();

            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            TodoItem item = _databaseContext.Todos.Find(id);

            if (item == null)
            {
                return NotFound(id);
            }

            return Ok(item);
        }

        [HttpGet("{id}")]
        public IActionResult GetById2(int id)
        {
            TodoItem item = _databaseContext.Todos.Find(id);

            if (item == null)
            {
                return NotFound(id);
            }

            return Ok(item);
        }


        [HttpPost]
        public IActionResult Create(TodoCreateModel model)
        {
            if (model.Text == "test")
            {
                return BadRequest(model);   // status code : 400
            }

            TodoItem item = new TodoItem();
            item.Text = model.Text;
            item.DueDate = model.DueDate;
            item.Completed = false;

            _databaseContext.Todos.Add(item);
            _databaseContext.SaveChanges();

            return Created("", item);   // status code : 201
        }

        // PUT : /Todo/Edit/{id}
        [HttpPut("{id}")]
        public IActionResult Edit([FromRoute] int id, [FromBody] TodoUpdateModel model)
        {
            TodoItem item = _databaseContext.Todos.Find(id);

            if (item == null)
            {
                return NotFound();  // status code : 404
            }

            item.Text = model.Text;
            item.DueDate = model.DueDate;
            item.Completed = model.Completed;

            _databaseContext.SaveChanges();

            return Ok(item);    // status code : 200
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] int id)
        {
            TodoItem item = _databaseContext.Todos.Find(id);

            if (item == null)
            {
                return NotFound();  // status code : 404
            }

            _databaseContext.Todos.Remove(item);
            _databaseContext.SaveChanges();

            return Ok();    // status code : 200
        }
    }
}
