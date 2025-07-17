using WorkflowEngine.Models;
using WorkflowEngine.Services;

public class InMemoryWorkflowRepository : IWorkflowRepository
{
    private readonly Dictionary<string, WorkflowDefinition> _defs = new();
    private readonly Dictionary<Guid, WorkflowInstance> _instances = new();

    public void AddDefinition(WorkflowDefinition def) =>
        _defs[def.Id] = def;

    public WorkflowDefinition? GetDefinition(string id) =>
        _defs.TryGetValue(id, out var d) ? d : null;

    public WorkflowInstance CreateInstance(string defId)
    {
        var def = GetDefinition(defId)
                  ?? throw new ArgumentException($"No definition {defId}");

        var initial = def.States.Single(s => s.IsInitial);
        var inst = new WorkflowInstance
        {
            DefinitionId = def.Id,
            CurrentStateId = initial.Id
        };
        _instances[inst.Id] = inst;
        return inst;
    }

    public WorkflowInstance? GetInstance(Guid id) =>
        _instances.TryGetValue(id, out var i) ? i : null;

    public void UpdateInstance(WorkflowInstance instance) =>
        _instances[instance.Id] = instance;
}