using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

public class DataAccess
{
    private string connectionString = "Data Source=todolist.db;Version=3;";

    public DataAccess()
    {
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        if (!File.Exists("todolist.db"))
        {
            SQLiteConnection.CreateFile("todolist.db");
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Todos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Task TEXT NOT NULL,
                        IsCompleted INTEGER NOT NULL
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public List<TodoItem> GetTodos()
    {
        List<TodoItem> todos = new List<TodoItem>();
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string selectQuery = "SELECT Id, Task, IsCompleted FROM Todos;";
            using (var command = new SQLiteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        todos.Add(new TodoItem
                        {
                            Id = reader.GetInt32(0),
                            Task = reader.GetString(1),
                            IsCompleted = reader.GetInt32(2) == 1
                        });
                    }
                }
            }
        }
        return todos;
    }

    public void AddTodo(TodoItem todo)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string insertQuery = "INSERT INTO Todos (Task, IsCompleted) VALUES (@Task, @IsCompleted);";
            using (var command = new SQLiteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Task", todo.Task);
                command.Parameters.AddWithValue("@IsCompleted", todo.IsCompleted ? 1 : 0);
                command.ExecuteNonQuery();
            }
        }
    }

    public void UpdateTodo(TodoItem todo)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string updateQuery = "UPDATE Todos SET Task = @Task, IsCompleted = @IsCompleted WHERE Id = @Id;";
            using (var command = new SQLiteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Task", todo.Task);
                command.Parameters.AddWithValue("@IsCompleted", todo.IsCompleted ? 1 : 0);
                command.Parameters.AddWithValue("@Id", todo.Id);
                command.ExecuteNonQuery();
            }
        }
    }

    public void DeleteTodo(int id)
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string deleteQuery = "DELETE FROM Todos WHERE Id = @Id;";
            using (var command = new SQLiteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }
}

public class TodoItem
{
    public int Id { get; set; }
    public string Task { get; set; }
    public bool IsCompleted { get; set; }
public string TaskDisplay
    {
        get { return IsCompleted ? "[完了] " + Task : Task; }
    }
}