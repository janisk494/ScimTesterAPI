using ScimTester.Core;
using ScimTester.Core.Models;
using ScimTester.Core.Assertions;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ScimTester.Api.Services;
public class ScimRunner : IScimRunner
{
    private readonly IHttpClientFactory _http;
    public ScimRunner(IHttpClientFactory http) => _http = http;

    public async Task<RunReport> RunSuiteAsync(RunRequest request, CancellationToken ct = default)
    {
        var client = _http.CreateClient();
        client.BaseAddress = new Uri(request.BaseUrl);
        if (!string.IsNullOrEmpty(request.BearerToken))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", request.BearerToken);

        var results = new List<ScimTester.Core.Models.TestResult>();

        try
        {
            if (request.Suite == "default")
            {
                // Run all test collections
                results.AddRange(await RunUsersTests(client, request.BaseUrl, ct));
                results.AddRange(await RunGroupsTests(client, request.BaseUrl, ct));
                results.AddRange(await RunSchemaTests(client, request.BaseUrl, ct));
            }
            else if (request.Suite == "users")
            {
                results.AddRange(await RunUsersTests(client, request.BaseUrl, ct));
            }
            else if (request.Suite == "groups")
            {
                results.AddRange(await RunGroupsTests(client, request.BaseUrl, ct));
            }
            else if (request.Suite == "schema")
            {
                results.AddRange(await RunSchemaTests(client, request.BaseUrl, ct));
            }
        }
        catch (Exception ex)
        {
            results.Add(new ScimTester.Core.Models.TestResult("RunnerError", false, ex.Message));
        }

        var report = new RunReport { RunId = Guid.NewGuid().ToString(), Timestamp = DateTime.UtcNow, Results = results };
        return report;
    }

    private async Task<List<ScimTester.Core.Models.TestResult>> RunUsersTests(HttpClient client, string baseUrl, CancellationToken ct)
    {
        var results = new List<ScimTester.Core.Models.TestResult>();
        
        // GET /Users
        var resp = await client.GetAsync($"{baseUrl}/Users", ct);
        results.Add(await resp.AssertStatus(200, "GET /Users returns 200"));

        // Create user
        var payload = new {
            schemas = new[]{ "urn:ietf:params:scim:schemas:core:2.0:User" },
            userName = "scimtester-" + Guid.NewGuid().ToString("N")[..6],
            name = new { givenName = "Test", familyName = "User" },
            emails = new[]{ new{ value = "test@example.com", primary = true } },
            active = true
        };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/scim+json");
        var createRsp = await client.PostAsync($"{baseUrl}/Users", content, ct);
        results.Add(await createRsp.AssertStatus(201, "POST /Users creates user"));

        return results;
    }

    private async Task<List<ScimTester.Core.Models.TestResult>> RunGroupsTests(HttpClient client, string baseUrl, CancellationToken ct)
    {
        var results = new List<ScimTester.Core.Models.TestResult>();
        
        // GET /Groups
        var resp = await client.GetAsync($"{baseUrl}/Groups", ct);
        results.Add(await resp.AssertStatus(200, "GET /Groups returns 200"));

        return results;
    }

    private async Task<List<ScimTester.Core.Models.TestResult>> RunSchemaTests(HttpClient client, string baseUrl, CancellationToken ct)
    {
        var results = new List<ScimTester.Core.Models.TestResult>();
        
        // GET /Schemas
        var resp = await client.GetAsync($"{baseUrl}/Schemas", ct);
        results.Add(await resp.AssertStatus(200, "GET /Schemas returns 200"));

        return results;
    }
}
