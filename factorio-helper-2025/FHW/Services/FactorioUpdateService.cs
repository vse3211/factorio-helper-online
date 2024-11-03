using System;

namespace FHW.Services;

public sealed class FactorioUpdateService
{
    private readonly HttpClient _client;

    public FactorioUpdateService(HttpClient client)
    {
        _client = client;
    }

    public async Task<Classes.Game.UpdatePackages?> GetAvailablePackages()
    {
        var content = await _client.GetFromJsonAsync<Classes.Game.UpdatePackages>($"get-available-versions");
        return content;
    }
}
