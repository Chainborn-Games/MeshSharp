using MeshSharp.Providers.Contracts;

namespace MeshSharp.Wallet;

public class MeshWalletConfig
{
    public int NetworkId { get; set; } = 0;
    public MeshKeyType KeyType { get; set; }
    
    public string[]? MnemonicWords { get; set; }
    public string? RootPrivateKeyBech32 { get; set; }
    public string? CliPaymentKey { get; set; }
    public string? CliStakeKey { get; set; }
    public string? ReadOnlyAddress { get; set; }
    public IFetcher? Fetcher { get; set; }
    public ISubmitter? Submitter { get; set; }
}