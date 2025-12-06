namespace ScimTester.Core.Models;
public record TestResult(string Name, bool Passed, string? Message = null, object? Details = null);
