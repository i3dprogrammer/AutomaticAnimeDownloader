using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.Models
{
    public class Todo
    {
        public List<TodoItem> TodoItems { get; set; }

        public Todo()
        {
            TodoItems = new List<TodoItem>();
            TodoItems.Add(new TodoItem() { Done = false, Description = "Downloading time schedule." });
            TodoItems.Add(new TodoItem() { Done = false, Description = "Support different users other than HorribleSubs." });
            TodoItems.Add(new TodoItem() { Done = false, Description = "Get downloadable anime titles from your MAL list." });
        }
    }

    public class TodoItem
    {
        public bool Done { get; set; }
        public string Description { get; set; }
    }
}
