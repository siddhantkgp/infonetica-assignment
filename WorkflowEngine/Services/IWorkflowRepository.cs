using WorkflowEngine.Models;
namespace WorkflowEngine.Services;    

public interface IWorkflowRepository
{
    void AddDefinition(WorkflowDefinition def);
    WorkflowDefinition? GetDefinition(string id);

    WorkflowInstance CreateInstance(string definitionId);
    WorkflowInstance? GetInstance(Guid id);
    void UpdateInstance(WorkflowInstance instance);
}