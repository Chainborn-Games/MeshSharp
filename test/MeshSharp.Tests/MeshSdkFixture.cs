using JetBrains.Annotations;

namespace MeshSharp.Tests;

[UsedImplicitly]
public class MeshSdkFixture : IAsyncDisposable
{
    public MeshSdk Sdk { get; } = MeshSdk.Create();
    public async ValueTask DisposeAsync() => await Sdk.DisposeAsync();
}