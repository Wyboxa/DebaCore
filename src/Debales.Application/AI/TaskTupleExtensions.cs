namespace Debales.Application.AI;

internal static class TaskTupleExtensions
{
    internal static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(
        this (Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4) tasks)
    {
        await Task.WhenAll(tasks.t1, tasks.t2, tasks.t3, tasks.t4);
        return (tasks.t1.Result, tasks.t2.Result, tasks.t3.Result, tasks.t4.Result);
    }
}
