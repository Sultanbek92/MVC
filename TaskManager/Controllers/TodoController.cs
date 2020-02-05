using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class TodoController : Controller
    {
        // GET: Todo
        public ActionResult Index()
        {
            TodoListViewModel viewModel = new TodoListViewModel();
            return View("Index", viewModel);
        }

        public ActionResult Edit(int id)
        {
            TodoListViewModel viewModel = new TodoListViewModel();
            viewModel.EditableItem = viewModel.TodoItems.FirstOrDefault(x => x.Id == id);
            return View("Index", viewModel);
        }

        public ActionResult Delete(int id)
        {
            using (var db = DbHelper.GetConnection())
            {
                TodoListItem item = db.Get<TodoListItem>(id);
                if (item != null)
                    db.Delete(item);
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateUpdate(TodoListViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using (var db = DbHelper.GetConnection())
                {
                    if (viewModel.EditableItem.Id <= 0)
                    {
                        viewModel.EditableItem.AddDate = DateTime.Now;
                        db.Insert<TodoListItem>(viewModel.EditableItem);
                    }
                    else
                    {
                        TodoListItem dbItem = db.Get<TodoListItem>(viewModel.EditableItem.Id);
                        // if (await TryUpdateModelAsync<TodoListItem>(dbItem, "EditableItem"))
                        // {
                        // db.Update<TodoListItem>(dbItem);
                        //                        }
                       
                            var sqlQuery = "UPDATE TodoListItems SET AddDate = @AddDate, Title = @Title, IsDone = @IsDone WHERE Id = @Id";
                            db.Execute(sqlQuery, dbItem);
                        
                    }
                }
                return RedirectToAction("Index");
            }
            else
                return View("Index", new TodoListViewModel());
        }

        public ActionResult ToggleIsDone(int id)
        {
            using (var db = DbHelper.GetConnection())
            {
                TodoListItem item = db.Get<TodoListItem>(id);
                if (item != null)
                {
                    item.IsDone = !item.IsDone;
                    db.Update<TodoListItem>(item);
                }
                return RedirectToAction("Index");
            }
        }
    }
}