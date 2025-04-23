using Microsoft.JavaScript.NodeApi;

namespace MeshSharp.Providers.Contracts;

public interface IFetcher
{
    JSReference GetReference();
}