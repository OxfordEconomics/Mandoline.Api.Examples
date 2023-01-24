using System.Threading;
using System.Threading.Tasks;

namespace Core.Client.Utilities;

public static class TaskExtensions
{
    public static void ExecuteSynchronously(this Task task, CancellationToken cancel = default(CancellationToken))
    {
        task.Wait(cancel);
    }

    public static T ExecuteSynchronously<T>(this Task<T> task, CancellationToken cancel = default(CancellationToken))
    {
        task.Wait(cancel);
        return task.Result;
    }
}
