# Configurable Workflow Engine

> **Stack**: .NET 8 • ASP.NET Core minimal-API • In-memory storage

## Project Structure
```
WorkflowEngine/ 
├── Program.cs
├── Models/
│   ├── State.cs
│   ├── ActionTransition.cs
│   ├── WorkflowDefinition.cs
│   └── WorkflowInstance.cs
├── Services/
│   ├── IWorkflowRepository.cs
│   ├── InMemoryWorkflowRepository.cs
│   └── WorkflowExecutor.cs
└── README.md
```
## Quick Start
```bash
git clone https://github.com/siddhantkgp/infonetica-assignment.git
cd WorkflowEngine
dotnet build
dotnet run # http://localhost:5267
```
## Sample Demo
# 1. Create a definition
```bash
curl -X POST http://localhost:5267/definitions \
     -H "Content-Type: application/json" \
     -d '{
           "id":"doc-approval",
           "name":"Document Approval",
           "states":[
               {"id":"draft","name":"Draft","isInitial":true},
               {"id":"review","name":"Review"},
               {"id":"approved","name":"Approved","isFinal":true}
           ],
           "actions":[
               {"id":"submit","name":"Submit","fromStates":["draft"],"toState":"review"},
               {"id":"approve","name":"Approve","fromStates":["review"],"toState":"approved"}
           ]
         }'
```
# 2. Start an instance
```bash
INSTANCE=$(curl -s -X POST "http://localhost:5267/instances?defId=doc-approval" | jq -r .id)
```
# 3. Execute a transition
```bash
curl -X POST http://localhost:5267/instances/$INSTANCE/actions/submit
```
