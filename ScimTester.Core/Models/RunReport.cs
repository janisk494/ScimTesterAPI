using System.Collections.Generic;

namespace ScimTester.Core.Models;
public record RunReport
{
    public string RunId { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    public List<TestResult> Results { get; init; } = new();
    public bool Passed => Results != null && Results.Count > 0 ? Results.TrueForAll(r => r.Passed) : false;
}
