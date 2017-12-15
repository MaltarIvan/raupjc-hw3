using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Task1
{
    public class TodoSqlRepository : ITodoRepository
    {
        private TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public void Add(TodoItem todoItem)
        {
            if (_context.TodoItems.Any(t => t.Id == todoItem.Id))
            {
                throw new DuplicateTodoItemException("The item already exists in a database!");
            } else
            {
                // ensuring that the two labels with the same name wont be in the database
                List<TodoItemLabel> labelsList = new List<TodoItemLabel>();
                foreach (var label in todoItem.Labels)
                {
                    labelsList.Add(AddLabel(label));
                }
                todoItem.Labels = labelsList;
                _context.TodoItems.Add(todoItem);
                _context.SaveChanges();
            }
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.Include(t => t.Labels).SingleOrDefault(t => t.Id == todoId);
            if (todoItem != null)
            {
                if (todoItem.UserId != userId)
                {
                    throw new TodoAccessDeneidException("The User is not the owner of this TodoItem!");
                }
            }
            return todoItem;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Include(t => t.Labels).Where(t => !t.IsCompleted && t.UserId == userId).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Include(t => t.Labels).Where(t => t.UserId == userId).OrderByDescending(t => t.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Include(t => t.Labels).Where(t => t.IsCompleted && t.UserId == userId).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            IQueryable<TodoItem> q = _context.TodoItems.Include(t => t.Labels).Where(t => t.UserId == userId);
            return q.Where(filterFunction).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.SingleOrDefault(t => t.Id == todoId && t.UserId == userId);
            if (todoItem != null)
            {
                if (todoItem.MarkAsCompleted())
                {
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new TodoAccessDeneidException("The User does not have an access to this TodoItem");
            }
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem todoItem = _context.TodoItems.SingleOrDefault(t => t.Id == todoId);
            if (todoItem != null)
            {
                if (todoItem.UserId != userId)
                {
                    throw new TodoAccessDeneidException("The User is not the owner of this TodoItem!");
                }
                else
                {
                    _context.TodoItems.Remove(todoItem);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            if (todoItem.UserId != userId)
            {
                throw new TodoAccessDeneidException("The user is not the owner of this TodoItem!");
            }
            if (_context.TodoItems.Any(t => t.Id == todoItem.Id))
            {
                _context.Entry(todoItem).State = EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                Add(todoItem);
            }
        }

        public List<TodoItemLabel> GetAllLabels()
        {
            throw new NotImplementedException();
        }

        public List<TodoItemLabel> GetNotContainedLabels(Guid itemId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public List<TodoItemLabel> GetLabelsForItem(Guid itemId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public void AddLabelToItem(Guid itemId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public TodoItemLabel AddLabel(TodoItemLabel item)
        {
            // ensuring that the label won't be put to the database if the same label allready exists
            TodoItemLabel todoItemLabel = _context.TodoItemLabels.SingleOrDefault(l => l.Value == item.Value);
            if (todoItemLabel != null)
            {
                return todoItemLabel;
            }
            else
            {
                _context.TodoItemLabels.Add(item);
                _context.SaveChanges();
                return item;
            }
        }
    }
}
