using MeshSharp.Wallet;

namespace MeshSharp.Tests;

public class WalletBasicOperationsTests(MeshSdkFixture fixture) : IClassFixture<MeshSdkFixture>
{
    private readonly MeshSdk _sdk = fixture.Sdk;
    private const int PreprodNetworkId = 0;

    [Fact]
    public async Task Can_Brew_Secret_Key()
    {
        var key = await _sdk.Wallet.BrewAsync();
        Assert.False(string.IsNullOrWhiteSpace(key)); // Should generate a non-empty key
    }

    [Fact]
    public async Task Can_Create_Wallet_And_Get_Addresses()
    {
        var key = await _sdk.Wallet.BrewAsync();
        var provider = await _sdk.Providers.CreateOfflineProviderAsync();

        var wallet = await _sdk.Wallet.CreateAsync(new()
        {
            Fetcher = provider,
            NetworkId = PreprodNetworkId,
            KeyType = MeshKeyType.Root,
            RootPrivateKeyBech32 = key
        });

        var unusedAddresses = await wallet.GetUnusedAddressesAsync();
        var changeAddress = await wallet.GetChangeAddressAsync();
        var rewardAddresses = await wallet.GetRewardAddressesAsync();

        Assert.NotNull(wallet);
        Assert.NotEmpty(unusedAddresses);
        Assert.False(string.IsNullOrWhiteSpace(changeAddress));
        Assert.NotEmpty(rewardAddresses);
        Assert.False(string.IsNullOrWhiteSpace(rewardAddresses[0]));
        Assert.False(string.IsNullOrWhiteSpace(unusedAddresses[0]));
    }
}