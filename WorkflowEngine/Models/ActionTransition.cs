namespace WorkflowEngine.Models;

public record ActionTransition(
    string Id,
    string Name,
    IReadOnlyCollection<string> FromStates,
    string ToState,
    bool Enabled = true,
    string? Description = null);