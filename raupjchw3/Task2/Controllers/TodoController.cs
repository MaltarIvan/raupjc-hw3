﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Task1;
using Microsoft.AspNetCore.Authorization;
using Task2.Data;
using Microsoft.AspNetCore.Identity;
using Task2.Models.Todo;
using AutoMapper;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Task2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            List<TodoItem> items = _repository.GetActive(new Guid(applicationUser.Id));
            List<TodoViewModel> todoViewModels = Mapper.Map<List<TodoItem>, List<TodoViewModel>>(items);
            IndexViewModel indexViewModel = new IndexViewModel(todoViewModels);
            return View(indexViewModel);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> MarkAsCompleted(Guid Id)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(Id, new Guid(applicationUser.Id));
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser applicationUser = await _userManager.GetUserAsync(HttpContext.User);
                TodoItem todo = new TodoItem(item.Text, new Guid(applicationUser.Id))
                {
                    DateDue = item.DateDue
                };
                _repository.Add(todo);
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult SeeCompletedTodos()
        {
            return View();
        }
    }
}