using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GitlabClient;

class GitlabClient
{
    private readonly HttpClient _client;

    public GitlabClient(HttpClient client, string token)
    {
        _client = client;
        _client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", token);
    }

    public async Task<List<Label>> GetProjectLabels(int projectId, CancellationToken ct = default)
    {
        using var rs = await _client.GetAsync(
            $"/api/v4/projects/{projectId}/labels?per_page=100&include_ancestor_groups=false",
            ct
        );
        rs.EnsureSuccessStatusCode();
        var rsBody = await rs.Content.ReadAsStringAsync(ct);
        var obj = JsonSerializer.Deserialize<List<Label>>(rsBody);
        return obj ?? throw new InvalidOperationException();
    }

    public async Task<List<Label>> GetGroupLabels(string group, CancellationToken ct = default)
    {
        using var rs = await _client.GetAsync($"/api/v4/groups/{group}/labels?per_page=100", ct);
        rs.EnsureSuccessStatusCode();
        var rsBody = await rs.Content.ReadAsStringAsync(ct);
        var obj = JsonSerializer.Deserialize<List<Label>>(rsBody);
        return obj ?? throw new InvalidOperationException();
    }

    public async Task<string> CreateGroupLabel(string group, Label label, CancellationToken ct = default)
    {
        var content = JsonContent.Create(label);
        using var rs = await _client.PostAsync($"/api/v4/groups/{group}/labels", content, ct);
        var sb = new StringBuilder();
        sb.Append($"Status {rs.StatusCode}");

        var rsBody = await rs.Content.ReadAsStringAsync(ct);
        sb.AppendLine("Content:");
        sb.AppendLine(rsBody);

        return sb.ToString();
    }
}

class Label
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("color")]
    public string? Color { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }
}