using System;

namespace FHW.Services;

public sealed class FactorioUpdateService
{
    public static FactorioUpdateService? Instance { get; private set; }
    private readonly HttpClient _client;

    public FactorioUpdateService(HttpClient client)
    {
        _client = client;
        if (Instance is null) Instance = this;
    }

    public async Task<Core.Game.Classes.UpdatePackages.Packages?> GetAvailablePackages()
    {
        var content = await _client.GetFromJsonAsync<Core.Game.Classes.UpdatePackages.Packages>($"get-available-versions");
        return content;
    }
}
