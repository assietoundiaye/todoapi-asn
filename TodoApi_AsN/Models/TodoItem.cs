using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi_AsN.Models;

public class TodoItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }  // ← nullable, MongoDB génère l'Id automatiquement

    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
