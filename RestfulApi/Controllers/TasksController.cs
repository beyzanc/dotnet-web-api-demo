using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sipay.Bootcamp.Beyza_Cabuk.FluentValidationDemo.Validators;
using Task = RestfulApi.Models.Task;

namespace RestfulApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TasksValidator _validator;
        private readonly List<Task> _tasks;

        public TasksController()
        {
            _validator = new TasksValidator();

            _tasks = new List<Task>()
            {
                new Task { Id = 1, Title = "Pay bills", Description = "Pay electricity, water, and internet bills", Deadline = DateTime.Now.AddDays(5), IsCompleted = false, Priority = 4, Tags = new List<string> { "finance", "bills" } },
                new Task { Id = 2, Title = "Write API documentation", Description = "Write detailed documentation for the RESTful API endpoints", Deadline = DateTime.Now.AddDays(2), IsCompleted = true, Priority = 2, Tags = new List<string> { "documentation", "API", "business" } },
                new Task { Id = 3, Title = "Buy groceries", Description = "Buy vegan milk, cucumber and cat food.", Deadline = DateTime.Now.AddDays(1), IsCompleted = false, Priority = 1, Tags = new List<string> { "groceries", "home" } },
                new Task { Id = 4, Title = "Clean the house", Description = "Vacuum, dust and mop.", Deadline = DateTime.Now.AddDays(4), IsCompleted = false, Priority = 4, Tags = new List<string> { "cleaning", "home" } },
                new Task { Id = 5, Title= "Take the cat to the vet", Description= "Take the cat to the vet and tell her about her recent condition and ask her to check her kidneys.", Deadline = DateTime.Now.AddDays(2), IsCompleted = true, Priority = 5, Tags = new List<string> { "vet","health","home","family"} },
                new Task { Id = 6, Title = "Call mom", Description = "Call mom to catch up and see how she's doing.", Deadline = DateTime.Now.AddDays(1), IsCompleted = true, Priority = 1, Tags = new List<string> { "family", "communication" } },
                new Task { Id = 7, Title = "Have a meeting with the advisor", Description = "Have a meeting with the advisor and ask her what you need to do and the documents you need to submit in order for your graduation to be finalized.", Deadline = DateTime.Now.AddDays(8), IsCompleted = false, Priority = 5, Tags = new List<string> { "graduation", "education" } },
                new Task { Id = 8, Title = "Volunteer at shelter", Description = "Help out at a local animal shelter for a few hours.", Deadline = DateTime.Now.AddDays(3), IsCompleted = true, Priority = 2, Tags = new List<string> { "volunteering", "animals" } },
                new Task { Id = 9, Title = "Add unit tests", Description = "Write comprehensive unit tests for the core functionality", Deadline = DateTime.Now.AddDays(4), IsCompleted = false, Priority = 3, Tags = new List<string> { "testing", "unit tests", "business" } },
                new Task { Id = 10, Title = "Create monthly budget", Description = "Review income and expenses to create a detailed monthly budget for better financial planning.", Deadline = DateTime.Now.AddDays(6), IsCompleted = false, Priority = 2, Tags = new List<string> { "finance", "budgeting" } }
            };
        }

        [HttpGet]
        public IActionResult GetAllTasks() {

            if (_tasks.Count == 0)
            {
                return Ok(new List<Task>());    
            }
            return Ok(_tasks);

        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _tasks.SingleOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NoContent();
            }
            return Ok(task);
        }

        [HttpGet("isCompleted/{isCompleted}")]
        public IActionResult GetTasksByIsCompleted(bool isCompleted)
        {
            var tasks = _tasks.Where(task => task.IsCompleted == isCompleted);
            if (!tasks.Any())
            {
              return Ok(new List<Task>());
            }
            return Ok(tasks);
        }


        [HttpGet("priority/{priority}")]
        public IActionResult GetTasksByPriority(int priority)
        {
            var tasks = _tasks.Where(task => task.Priority == priority);
            if (!tasks.Any())
            {
              return Ok(new List<Task>());
            }
            return Ok(tasks);
        }

        [HttpGet("sort")]
        public IActionResult GetSort(string sortBy, string sortOrder) // asc or desc
        {
            if (string.IsNullOrWhiteSpace(sortBy) || string.IsNullOrWhiteSpace(sortOrder))
            {
                return Ok(_tasks);
            }

            var sortedTasks = new List<Task>();

            switch (sortBy)
            {
                case "deadline":
                    sortedTasks = sortOrder.ToLower() == "desc"
                        ? _tasks.OrderByDescending(task => task.Deadline).ToList()
                        : _tasks.OrderBy(task => task.Deadline).ToList();
                    break;

                case "priority":
                    sortedTasks = sortOrder.ToLower() == "desc"
                        ? _tasks.OrderByDescending(task => task.Priority).ToList()
                        : _tasks.OrderBy(task => task.Priority).ToList();
                    break;
                default:
                    return BadRequest("Invalid sort parameter.");
            }

            if (!sortedTasks.Any())
            {
                return Ok(new List<Task>());
            }

            return Ok(sortedTasks);

        }


        [HttpGet("filter")]
        public IActionResult GetFilter([FromQuery] bool? isCompleted, [FromQuery] int? priority, [FromQuery] DateTime? deadline, [FromQuery] string[] tags)
        {

            var filteredTasks = _tasks;

            if (isCompleted.HasValue)
            {
                filteredTasks = filteredTasks.Where(task => task.IsCompleted == isCompleted.Value).ToList();
            }

            if (priority.HasValue)
            {
                filteredTasks = filteredTasks.Where(task => task.Priority == priority.Value).ToList();
            }

            if (deadline.HasValue)
            {
                filteredTasks = filteredTasks.Where(task => task.Deadline.Date == deadline.Value.Date).ToList();
            }

            if (tags != null && tags.Length > 0)
            {
                filteredTasks = filteredTasks.Where(task => tags.All(tag => task.Tags.Contains(tag))).ToList();
            }

            if (!filteredTasks.Any())
            {
                return Ok(new List<Task>());
            }

            return Ok(filteredTasks);
        }


        [HttpPost]
        public IActionResult CreateTask(Task task)
        {

            var validation = _validator.Validate(task);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }
           

            _tasks.Add(task);
            if (_tasks.Count == 0)
            {
                return NoContent();
            }
            return Created("", _tasks);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTaskById(int id) {
            var task = _tasks.SingleOrDefault(task => task.Id == id);
            if (task == null)
            {
                return NoContent();
            }
            _tasks.Remove(task);

            if (_tasks.Count == 0)
            {
                return NoContent();
            }

            return Ok(_tasks);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, Task task) {
            var existTask = _tasks.SingleOrDefault(task => task.Id == id);
            if (existTask == null)
            {
                return NoContent();
            }

            var validation = _validator.Validate(task);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors) ;
            }

            existTask.Title = task.Title;
            existTask.Description = task.Description;
            existTask.Deadline = task.Deadline;
            existTask.IsCompleted = task.IsCompleted;
            existTask.Priority = task.Priority;
            existTask.Tags = task.Tags;

            return Ok(existTask);

        }

        [HttpPatch("{id}/Title")]
        public IActionResult UpdateTaskTitle(int id, [FromBody] string newTitle) { 
            var task = _tasks.FirstOrDefault(task => task.Id == id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }
            task.Title = newTitle;
            return Ok(task);

        }
    }
}
