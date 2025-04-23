using Microsoft.JavaScript.NodeApi;

namespace MeshSharp.Providers.Contracts;

public interface ISubmitter
{
    JSReference GetReference();
}