namespace RestfulApi.Models;

public class Task
{

    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } 
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public int Priority { get; set; } // from 1 to 5
    public List<string> Tags { get; set; }

}
