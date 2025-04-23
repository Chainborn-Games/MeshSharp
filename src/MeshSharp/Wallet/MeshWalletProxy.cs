using System.Linq;
using System.Threading.Tasks;
using MeshSharp.Abstractions;
using Microsoft.JavaScript.NodeApi;

namespace MeshSharp.Wallet;

public class MeshWalletProxy : NodeObjectProxy
{
    private MeshWalletProxy(MeshSdk sdk, JSReference walletRef) : base(sdk)
    {
        WalletRef = walletRef;
    }

    public JSReference WalletRef { get; }
    private JSValue Wallet => WalletRef.GetValue();

    public static MeshWalletProxy Create(MeshSdk sdk, JSValue meshModule, MeshWalletConfig config)
    {
        var walletCtor = meshModule.GetProperty("MeshWallet");
        var configValue = CreateConfig(config);
        var wallet = (JSObject)walletCtor.CallAsConstructor(configValue);
        wallet.CallMethod("init");
        var walletReference = new JSReference(wallet);
        return new(sdk, walletReference);
    }
    
    public async Task<string> GetChangeAddressAsync()
    {
        return await RunAsync(async () =>
        {
            var promise = (JSPromise)Wallet.CallMethod("getChangeAddress");
            return (string)await promise.AsTask();
        });
    }

    public async Task<string[]> GetRewardAddressesAsync()
    {
        return await RunAsync(async () =>
        {
            var promise = (JSPromise)Wallet.CallMethod("getRewardAddresses");
            var jsArray = (JSArray)await promise.AsTask();
            return jsArray.Select(x => (string)x).ToArray();
        });
    }

    // Add more wallet methods here...
    public async Task<string[]> GetUnusedAddressesAsync()
    {
        return await RunAsync(async () =>
        {
            var promise = (JSPromise)Wallet.CallMethod("getUnusedAddresses");
            var jsValue = await promise.AsTask();
            var jsArray = (JSArray)jsValue;
            return jsArray.Select(x => (string)x).ToArray();
        });
    }

    private static JSValue CreateConfig(MeshWalletConfig options)
    {
        var fetcher = options.Fetcher;
        var submitter = options.Submitter;
        var key = JSValue.CreateObject();

        switch (options.KeyType)
        {
            case MeshKeyType.Mnemonic:
                key.SetProperty("type", "mnemonic");
                key.SetProperty("words", new JSArray(options.MnemonicWords.Cast<JSValue>().ToArray()));
                break;
            case MeshKeyType.Root:
                key.SetProperty("type", "root");
                key.SetProperty("bech32", options.RootPrivateKeyBech32);
                break;
            case MeshKeyType.Cli:
                key.SetProperty("type", "cli");
                key.SetProperty("payment", options.CliPaymentKey);
                if (!string.IsNullOrWhiteSpace(options.CliStakeKey))
                    key.SetProperty("stake", options.CliStakeKey);
                break;
            case MeshKeyType.Address:
                key.SetProperty("type", "address");
                key.SetProperty("address", options.ReadOnlyAddress);
                break;
        }

        var config = JSValue.CreateObject();
        config.SetProperty("networkId", options.NetworkId);
        config.SetProperty("key", key);

        if (fetcher != null) config.SetProperty("fetcher", fetcher.GetReference().GetValue());
        if (submitter != null) config.SetProperty("submitter", submitter.GetReference().GetValue());
        
        return config;
    }
}