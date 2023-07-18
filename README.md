# Patika.dev & Sipay .NET Bootcamp #Week 1: RESTful API Task Management System

This repository contains a RESTful API for a task management system. The API allows users to perform various operations related to tasks, such as creating tasks, updating task details, retrieving tasks based on different criteria, and more. The API is built using ASP.NET Core and utilizes FluentValidation for input validation.

## Project Structure

- **Task.cs**: Defines the **Task model class** with properties such as Id, Title, Description, Deadline, IsCompleted, Priority and list of Tags.
- **TasksController.cs**: Contains the **TasksController class**, which handles various **HTTP requests** related to tasks. It includes methods for retrieving tasks, creating a new task, updating task details, deleting a task, sorting and filtering tasks. The controller is responsible for processing the requests and generating appropriate responses.
- **TasksValidator.cs**: Defines the **TasksValidator class**, which extends **AbstractValidator<Task>**. It contains validation rules for each property of the Task model using **FluentValidation's fluent API**. The TasksValidator class ensures that the input data for tasks meets the specified criteria and is valid before further processing.

## Usage

1.  Clone the project from Github by running the following command in a terminal:
   
    `https://github.com/beyzanc/dotnet-web-api-demo.git`
    
2. Install the .NET Core SDK by following the instructions on the [.NET Core website](https://dotnet.microsoft.com/en-us/download/dotnet-core)
3. Install the FluentValidation NuGet package by running the following command in a terminal:
   
   `dotnet add package FluentValidation`
4. Run the project in Visual Studio by pressing F5 or by using the this command in a terminal:
   
   `dotnet run`
   
   Then, the application will launch and start listening on a local development server.

5. Open your web browser or a tool like Postman.

   Navigate to `http://localhost:<port>/api/Tasks`, where <port> is the port number assigned by the development server.

   Use the Swagger UI to interact with the API's and test the TasksController's HTTP method.

7. Enjoy!


## Additional Notes

## Fluent Validation Rules with Same Examples

| Property    | Validation Rule                                        | Success Examples                   | Fail Examples             |
|-------------|-------------------------------------------------------|----------------------------------------------|----------------------------------------|
| Id          | NotEmpty(), GreaterThan(0)                            | 1, 10, 100                                  | 0, -5, -10                            |
| Title       | NotEmpty(), MaximumLength(100)                        | "Pay Bills", "Write Documentation", "Buy Groceries" | "", "Lorem ipsum dolor sit amet, consectetur adipiscing elit." |
| Description | MaximumLength(500)                                     | "Lorem ipsum dolor sit amet", "Nulla vitae elit libero" | "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus." |
| Deadline    | GreaterThanOrEqualTo(DateTime.Today)                   | Today, Future date                          | Past date                              |
| IsCompleted | Must(x => x == true or x == false)                     | true, false                                 | null, 1, "true"                       |
| Priority    | InclusiveBetween(1, 5)                                 | 1, 3, 5                                     | 0, 6, 10                               |
| Tags        | ForEach(tag => tag.MaximumLength(30))                 | ["finance", "bills"], ["documentation", "API"], ["groceries", "home"] | ["Lorem ipsum dolor sit amet, consectetur adipiscing elit."] |



### **A successful example of task filtering with HTTP GET:**
![](https://github.com/beyzanc/dotnet-web-api-demo/blob/main/200.png)

### **An unsuccessful example of a POST operation with invalid inputs in Fluent Validation:**
![](https://github.com/beyzanc/dotnet-web-api-demo/blob/main/400.png)


## Object Oriented Programming in Task Management System

Here are the uses of OOP in this project:

- **Encapsulation**: The Task.cs class is an example of encapsulation. It groups together the properties of a task into a single class.
```  
public class Task
{
    public int Id { get; set; }
    // ...
}
```

- **Abstraction**: The TasksController class provides an abstraction layer for handling HTTP requests related to the Task object.

```
public class TasksController : ControllerBase
{
    // ...
    [HttpPost]
    public IActionResult Post([FromBody] Task task)
    {
        // ...
    }
}
```
- **Inheritance**: The TasksValidator class inherits from the AbstractValidator<Task> class provided by FluentValidation.
```
public class TasksValidator : AbstractValidator<Task>
{
    // ...
}
```

- **Polymorphism**: The IValidator<Task> interface is used to define a common interface for validating Task objects. In this code, the specific implementation of the IValidator<Task> interface is provided to the TasksController class with dependency injection.

```
public TasksController(IValidator<Task> validator)
{
    _validator = validator;
}
```

## SOLID Principles in Task Management System

- **Single Responsibility Principle**: Each class has a clear and well-defined one responsibility.

- **Open-Closed Principle**: The TasksController class uses an interface to validate Task objects, allowing new validation rules to be added without modifying existing code.

- **Liskov Substitution Principle**: The TasksValidator class is a subclass of the AbstractValidator<Task> class and can be used wherever an instance of the AbstractValidator<Task> class is expected.

- **Interface Segregation Principle**: The TasksController class depends only on the minimal IValidator<Task> interface, avoiding unnecessary dependencies.

- **Dependency Inversion Principle**: The TasksController class depends on an abstraction (the IValidator<Task> interface) rather than on a specific implementation.


## Idempotence in Task Management System

Here is a brief overview of how idempotence applies to different API methods in this systems:

- GET /api/tasks: This endpoint retrieves all tasks and is inherently *idempotent*. Repeating the GET request multiple times will consistently return the same set of tasks.

- GET /api/tasks/{id}: Retrieving a specific task by its ID is also an *idempotent* operation. Multiple requests for the same task will consistently return the same task information.

- POST /api/tasks: Creating a new task with a POST request is *not idempotent*. Each request will create a new task, and repeating the same request multiple times will result in the creation of multiple tasks with different IDs.

- DELETE /api/tasks/{id}: Deleting a specific task by its ID is an *idempotent* operation. If the same delete request is sent multiple times, it will delete the task only once, and subsequent requests will have no effect.

- PUT /api/tasks/{id}: Updating a task with a PUT request is generally considered *idempotent*. Repeating the same update request multiple times will result in the same final state of the task.

- PATCH /api/tasks/{id}: Similarly, updating a task with a PATCH request is typically *idempotent*. Repeating the same patch request multiple times will not alter the final state beyond the intended outcome.

## Built With

- ASP.NET 6.0
- Fluent Validation Library
- Swagger
- .NET 6.0 SDK
- Visual Studio 2022
