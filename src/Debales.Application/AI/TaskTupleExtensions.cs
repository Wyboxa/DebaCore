namespace Debales.Application.AI;

internal static class TaskTupleExtensions
{
    internal static async Task<(T1, T2)> WhenAll<T1, T2>(
        this (Task<T1> t1, Task<T2> t2) tasks)
    {
        await Task.WhenAll(tasks.t1, tasks.t2);
        return (tasks.t1.Result, tasks.t2.Result);
    }

    internal static async Task<(T1, T2, T3, T4)> WhenAll<T1, T2, T3, T4>(
        this (Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4) tasks)
    {
        await Task.WhenAll(tasks.t1, tasks.t2, tasks.t3, tasks.t4);
        return (tasks.t1.Result, tasks.t2.Result, tasks.t3.Result, tasks.t4.Result);
    }

    internal static async Task<(T1, T2, T3, T4, T5)> WhenAll<T1, T2, T3, T4, T5>(
        this (Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4, Task<T5> t5) tasks)
    {
        await Task.WhenAll(tasks.t1, tasks.t2, tasks.t3, tasks.t4, tasks.t5);
        return (tasks.t1.Result, tasks.t2.Result, tasks.t3.Result, tasks.t4.Result, tasks.t5.Result);
    }

    internal static async Task<(T1, T2, T3, T4, T5, T6, T7)> WhenAll<T1, T2, T3, T4, T5, T6, T7>(
        this (Task<T1> t1, Task<T2> t2, Task<T3> t3, Task<T4> t4, Task<T5> t5, Task<T6> t6, Task<T7> t7) tasks)
    {
        await Task.WhenAll(tasks.t1, tasks.t2, tasks.t3, tasks.t4, tasks.t5, tasks.t6, tasks.t7);
        return (tasks.t1.Result, tasks.t2.Result, tasks.t3.Result, tasks.t4.Result, tasks.t5.Result, tasks.t6.Result, tasks.t7.Result);
    }
}
