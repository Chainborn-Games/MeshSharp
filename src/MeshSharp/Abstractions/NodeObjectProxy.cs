using System;
using System.Threading.Tasks;
using Microsoft.JavaScript.NodeApi;
using Microsoft.JavaScript.NodeApi.Runtime;

namespace MeshSharp.Abstractions;

public abstract class NodeObjectProxy(MeshSdk sdk)
{
    public MeshSdk Sdk { get; } = sdk;
    public NodeEmbeddingThreadRuntime Runtime => Sdk.Runtime;

    protected async Task<T> RunAsync<T>(Func<Task<T>> action)
    {
        return await Runtime.RunAsync(action);
    }
    
    protected async Task RunAsync(Func<Task> action)
    {
        await Runtime.RunAsync(action);
    }

    protected Task<JSValue> ImportMeshSdkCoreModule()
    {
        return Sdk.ImportMeshSdkCoreModule();
    }
}