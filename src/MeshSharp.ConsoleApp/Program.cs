using MeshSharp;
using MeshSharp.Wallet;

await using var sdk = MeshSdk.Create();
var provider = await sdk.Providers.CreateBlockfrostProviderAsync("preprodjbBoLvimJoiSOrTTmsCclbEAxdVLIcZh");
var key = await sdk.Wallet.BrewAsync();
var wallet = await sdk.Wallet.CreateAsync(new()
{
    Fetcher = provider,
    Submitter = provider,
    NetworkId = 0,
    KeyType = MeshKeyType.Root,
    RootPrivateKeyBech32 = key
});

var unusedAddresses = await wallet.GetUnusedAddressesAsync();
var changeAddress = await wallet.GetChangeAddressAsync();
var rewardAddresses = await wallet.GetRewardAddressesAsync();
Console.WriteLine(key);
Console.WriteLine(changeAddress);
Console.WriteLine(rewardAddresses[0]);
Console.WriteLine(unusedAddresses[0]);

// var runtime = sdk.Runtime;
//
// await runtime.RunAsync(() =>
// {
//     var script = @"
//         const fs = require('fs');
//         fs.writeFileSync('/Users/sipke/Projects/Skywalker Digital/meshjs-dotnet/src/MeshSharp/bin/Debug/me.sk', 'secret_key');
//     ";
//     JSValue.RunScript(script);
//     return Task.CompletedTask;
// });