namespace MeshSharp.Tests;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        await using var sdk = MeshSdk.Create();
        var key = await sdk.Wallet.BrewAsync();
        Assert.NotEmpty(key);
    }
}