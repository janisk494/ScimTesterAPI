using System.Text.Json;

namespace ScimTester.Core.Models;
public record RunRequest(string BaseUrl, string? BearerToken, string Suite = "default", JsonElement? CustomSchema = null);
