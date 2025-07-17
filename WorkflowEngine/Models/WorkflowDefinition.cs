namespace WorkflowEngine.Models;

public class WorkflowDefinition
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
    public List<State> States { get; init; } = new();
    public List<ActionTransition> Actions { get; init; } = new();
}