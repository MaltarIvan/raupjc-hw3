using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task1;

namespace Task2.Models.Todo
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public String Text { set; get; }
        public bool IsCompleted { get
            {
                return DateCompleted.HasValue;
            }
        }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDue { get; set; }
        public List<TodoItemLabel> Labels { get; set; }
        public bool MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                DateCompleted = DateTime.Now;
                return true;
            }
            return false;
        }
        public TimeSpan? DaysUntilDue{
            get
            {
                return DateDue - DateTime.UtcNow;
            }
        }

        public TodoViewModel(string text, Guid userId)
        {
            Id = Guid.NewGuid();
            Text = text;
            DateCreated = DateTime.UtcNow;
            UserId = userId;
            Labels = new List<TodoItemLabel>();
        }

        public TodoViewModel(string text)
        {
            // Generates new unique identifier
            Id = Guid.NewGuid();
            // DateTime .Now returns local time , it wont always be what you expect (depending where the server is).
            // We want to use universal (UTC ) time which we can easily convert to local when needed.
            // ( usually done in browser on the client side )
            DateCreated = DateTime.UtcNow;
            Text = text;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            TodoItem todoItem = (TodoItem)obj;
            return Id == todoItem.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
