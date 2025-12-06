using System.Threading;
using System.Threading.Tasks;
using ScimTester.Core.Models;

namespace ScimTester.Core;
public interface IScimRunner
{
    Task<RunReport> RunSuiteAsync(RunRequest request, CancellationToken ct = default);
}
