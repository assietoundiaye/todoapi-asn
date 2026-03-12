public class TodoItemDTO
{
    public string? Id { get; set; }  // ← nullable, pas besoin de l'envoyer au POST
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}
