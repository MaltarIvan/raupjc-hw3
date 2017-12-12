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
        public string getDateCompletedShort()
        {
            if (DateCompleted.HasValue)
            {
                return ((DateTime)DateCompleted).ToShortDateString();
            }
            else
            {
                return "";
            }
        }
        public DateTime DateCreated { get; set; }
        public string getDateCreatedShort()
        {
            return DateCreated.ToShortDateString();
        }
        public DateTime? DateDue { get; set; }
        public string getDateDueShort()
        {
            if (DateDue.HasValue)
            {
                return ((DateTime)DateDue).ToShortDateString();
            }
            else
            {
                return "";
            }
        }
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
        public string DaysUntilDue{
            get
            {
                if (DateDue.HasValue)
                {
                    int days = ((DateTime)DateDue - DateTime.Now).Days;
                    if (days >= 0)
                    {
                        return "(za " + days.ToString() + " dana!)";
                    }
                    else
                    {
                        return "(The deadline has passed!)";
                    }
                } else
                {
                    return "";
                }
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
