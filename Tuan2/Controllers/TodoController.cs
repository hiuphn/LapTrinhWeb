using Microsoft.AspNetCore.Mvc;
using Tuan2.Models;

namespace Tuan2.Controllers
{
    public class TodoController : Controller
    {
        private static List<TodoItem> listTodo = new List<TodoItem>();
        
        public IActionResult Index()
        {
            return View(listTodo);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return View(item);
            }

            item.Id = listTodo.Count + 1;
            listTodo.Add(item);

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            foreach (TodoItem item in listTodo)
            {
                if(item.Id == id)
                {
                    return View(item);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(TodoItem todo)
        {
            foreach(TodoItem item in listTodo)
            {
                if(item.Id == todo.Id)
                {
                    item.Title = todo.Title;
                    item.IsCompleted = todo.IsCompleted;

                    return RedirectToAction("Index");
                }
            }

            return NotFound();  
        }
        
        public IActionResult Details(TodoItem todo)
        {
            foreach(TodoItem item in listTodo)
            {
                if(item.Id == todo.Id)
                {
                    return View(item);
                }
            }
            return NotFound();
        }
        public IActionResult Delete(int id)
        {
            foreach (TodoItem item in listTodo)
            {
                if (item.Id == id)
                {
                    return View(item);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Delete(TodoItem todo)
        {
            foreach (TodoItem item in listTodo)
            {
                if (item.Id == todo.Id)
                {
                    listTodo.Remove(item);
                    
                    return RedirectToAction("Index");

                }
                
            }
            
            return NotFound();
        }
    }


    
}
