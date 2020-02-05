using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace TaskManager.Models
{
    public class TodoListViewModel
    {
        public TodoListViewModel()
        {
            using (var db = DbHelper.GetConnection())
            {
                this.EditableItem = new TodoListItem();
                this.TodoItems = db.Query<TodoListItem>("SELECT * FROM TodoListItems ORDER BY AddDate DESC").ToList();

            }
        }

        public List<TodoListItem> TodoItems { get; set; }

        public TodoListItem EditableItem { get; set; }
    }
    public static class DbHelper
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["IdentityDb"].ConnectionString);
        }
    }
}