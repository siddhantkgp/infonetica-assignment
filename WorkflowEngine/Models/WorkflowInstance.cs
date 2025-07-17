namespace WorkflowEngine.Models;

public class WorkflowInstance
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string DefinitionId { get; init; } = default!;
    public string CurrentStateId { get; set; } = default!;
    public List<HistoryItem> History { get; } = new();

    public record HistoryItem(string ActionId, string From, string To, DateTimeOffset At);
}