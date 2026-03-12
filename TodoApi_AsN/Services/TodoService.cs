using MongoDB.Driver;
using TodoApi_AsN.Models;

namespace TodoApi_AsN.Services;

public class TodoService
{
    private readonly IMongoCollection<TodoItem> _todos;

    public TodoService(IConfiguration config)
    {
        var connectionString = config.GetValue<string>("MongoDb:ConnectionString")
                               ?? throw new InvalidOperationException("MongoDb:ConnectionString manquant.");

        var databaseName = config.GetValue<string>("MongoDb:DatabaseName") ?? "TodoDb";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _todos = database.GetCollection<TodoItem>("TodoItems");
    }

    public async Task<List<TodoItem>> GetAllAsync() =>
        await _todos.Find(_ => true).ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(string id) =>
        await _todos.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(TodoItem newTodo) =>
        await _todos.InsertOneAsync(newTodo);

    public async Task UpdateAsync(string id, TodoItem updatedTodo) =>
        await _todos.ReplaceOneAsync(x => x.Id == id, updatedTodo);

    public async Task RemoveAsync(string id) =>
        await _todos.DeleteOneAsync(x => x.Id == id);
}
