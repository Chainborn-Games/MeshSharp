using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MeshSharp.Providers;
using MeshSharp.Wallet;
using Microsoft.JavaScript.NodeApi;
using Microsoft.JavaScript.NodeApi.Runtime;

namespace MeshSharp;

public class MeshSdk : IAsyncDisposable, IDisposable
{
    private static MeshSdk? _instance;
    private readonly NodeEmbeddingPlatform _platform;
    private readonly NodeEmbeddingThreadRuntime _runtime;
    private readonly MeshWalletFactory _walletFactory;
    private readonly ProvidersFactory _providersFactory;

    private MeshSdk(NodeEmbeddingPlatform platform, NodeEmbeddingThreadRuntime runtime)
    {
        _platform = platform;
        _runtime = runtime;
        _walletFactory = new(this);
        _providersFactory = new(this);
    }
    
    public NodeEmbeddingThreadRuntime Runtime => _runtime ?? throw new InvalidOperationException("Runtime not initialized.");
    public MeshWalletFactory Wallet => _walletFactory;
    public ProvidersFactory Providers => _providersFactory;

    public static MeshSdk Create()
    {
        if (_instance != null)
            return _instance;
        
        // Find the path to the libnode binary for the current platform.
        var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var libNodePath = Path.Combine(baseDir, "runtimes", "osx-arm64", "native", "libnode.dylib");
        var platform = new NodeEmbeddingPlatform(new()
        {
            LibNodePath = libNodePath
        });
        var runtime = platform.CreateThreadRuntime(baseDir,
            new()
            {
                MainScript = "globalThis.require = require('module').createRequire(process.execPath);\n"
            });

        return _instance = new(platform, runtime);
    }

    public async Task<JSValue> ImportMeshSdkCoreModule()
    {
        //return await Runtime.ImportAsync("@meshsdk/core");
        // var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        // var bundlePath = Path.Combine(baseDir, "node_modules", "mesh-sharp-interop", "mesh-sharp-interop.js");
        //return await Runtime.ImportAsync(bundlePath);
        return await Runtime.ImportAsync("@meshsdk/core");
    }
    
    public void Dispose()
    {
        _runtime.Dispose();
        _platform.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return default;
    }
}