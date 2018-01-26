using MaterialDesignTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignTest.ViewModel
{
    class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(Todo todo)
        {
            this.todo = todo;
        }

        public Todo todo { get; private set; }

        public List<TodoItem> TodoItems
        {
            get
            {
                return todo.TodoItems;
            }
            set
            {
                todo.TodoItems = value;
            }
        }
    }
}
