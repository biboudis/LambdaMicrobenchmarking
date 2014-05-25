Lambda Microbenchmarking
=======================

This is an initial code for a microbenchmarking facility only for lambdas.
Currently it supports only lambdas returning a value.

```C#
Func<long> sumLinq = () => v.Sum();
Func<long> sumSqLinq = () => v.Select(x => x * x).Sum();
Func<long> sumSqEvenLinq = () => v.Where(x => x % 2 == 0).Select(x => x * x).Sum();
Func<long> cartLinq = () => (from x in vHi
                             from y in vLow
                             select x * y).Sum();

Script<long>.Of(
        Tuple.Create("sumLinq", sumLinq),
        Tuple.Create("sumSqLinq", sumSqLinq),
        Tuple.Create("sumSqEvensLinq", sumSqEvenLinq),
        Tuple.Create("cartLinq", cartLinq))
    .RunAll();
```