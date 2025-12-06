using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ScimTester.Core.Models;

namespace ScimTester.Core.Assertions;
public static class AssertExtensions
{
    public static async Task<TestResult> AssertStatus(this HttpResponseMessage rsp, int expected, string name)
    {
        var passed = (int)rsp.StatusCode == expected;
        var body = await SafeReadBody(rsp);
        return new TestResult(name, passed, passed ? null : $"Expected {expected} got {(int)rsp.StatusCode}", new { status = (int)rsp.StatusCode, body });
    }

    static async Task<string?> SafeReadBody(HttpResponseMessage rsp)
    {
        try { return await rsp.Content.ReadAsStringAsync(); }
        catch { return null; }
    }
}
