using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2.Models.Todo
{
    public class IndexViewModel
    {
        public List<TodoViewModel> TodoViewModels { get; set; }

        public IndexViewModel(List<TodoViewModel> todoViewModels)
        {
            TodoViewModels = todoViewModels;
        }
    }
}
