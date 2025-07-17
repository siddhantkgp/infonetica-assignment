using WorkflowEngine.Models;
namespace WorkflowEngine.Services;

public class WorkflowEngine(IWorkflowRepository repo)
{
    private readonly IWorkflowRepository _repo = repo;

    public WorkflowInstance Execute(Guid instanceId, string actionId)
    {
        var inst = _repo.GetInstance(instanceId)
                  ?? throw new KeyNotFoundException("Instance not found");

        var def = _repo.GetDefinition(inst.DefinitionId)!;

        var action = def.Actions.SingleOrDefault(a => a.Id == actionId)
                     ?? throw new InvalidOperationException("Action not in definition");

        if (!action.Enabled)
            throw new InvalidOperationException("Action disabled");

        if (inst.CurrentStateId == action.ToState)
            throw new InvalidOperationException("Already in target state");

        if (!action.FromStates.Contains(inst.CurrentStateId))
            throw new InvalidOperationException("Illegal source state");

        var currentState = def.States.Single(s => s.Id == inst.CurrentStateId);
        if (currentState.IsFinal)
            throw new InvalidOperationException("Instance in final state");

        var targetState = def.States.SingleOrDefault(s => s.Id == action.ToState)
                          ?? throw new InvalidOperationException("Target state missing");

        if (!targetState.Enabled)
            throw new InvalidOperationException("Target state disabled");

        // mutate + history
        inst.History.Add(new(inst.CurrentStateId, inst.CurrentStateId, targetState.Id,
                             DateTimeOffset.UtcNow));
        inst.CurrentStateId = targetState.Id;
        _repo.UpdateInstance(inst);
        return inst;
    }
}
