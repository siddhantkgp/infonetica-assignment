using WorkflowEngine.Models;
using WorkflowEngine.Services;
using Engine = WorkflowEngine.Services.WorkflowEngine;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IWorkflowRepository, InMemoryWorkflowRepository>();
builder.Services.AddSingleton<Engine>();

var app = builder.Build();
app.MapPost("/definitions", (WorkflowDefinition def, IWorkflowRepository repo) =>
{
    if (def.States.Count(s => s.IsInitial) != 1)
        return Results.BadRequest("Definition must have exactly one initial state.");

    if (def.States.Select(s => s.Id).Distinct().Count() != def.States.Count)
        return Results.BadRequest("Duplicate state IDs.");

    repo.AddDefinition(def);
    return Results.Created($"/definitions/{def.Id}", def);
});

app.MapGet("/definitions/{id}", (string id, IWorkflowRepository repo) =>
    repo.GetDefinition(id) is { } d ? Results.Ok(d) : Results.NotFound());

app.MapPost("/instances", (string defId, IWorkflowRepository repo) =>
{
    try
    {
        var inst = repo.CreateInstance(defId);
        return Results.Created($"/instances/{inst.Id}", inst);
    }
    catch (Exception ex) { return Results.BadRequest(ex.Message); }
});

app.MapGet("/instances/{id:guid}", (Guid id, IWorkflowRepository repo) =>
    repo.GetInstance(id) is { } inst ? Results.Ok(inst) : Results.NotFound());

app.MapPost("/instances/{id:guid}/actions/{actionId}",
    (Guid id, string actionId, Engine engine) =>
{
    try
    {
        var inst = engine.Execute(id, actionId);
        return Results.Ok(inst);
    }
    catch (Exception ex) { return Results.BadRequest(ex.Message); }
});

app.MapGet("/", () => "Workflow Engine is running ✅");

app.Run();
