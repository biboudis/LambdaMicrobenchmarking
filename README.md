Lambda Microbenchmarking
=======================

This is an initial code for a microbenchmarking facility only lambdas for C# and F#.
Currently it supports functions returning a value.

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

The corresponding script for F#:
```F#
let script = [|
   ("sumLinq", Func<int64> sumLinq);
   ("sumSqLinq", Func<int64> sumSqLinq);
   ("sumSqEvenLinq", Func<int64> sumSqEvenLinq);
   ("cartLinq", Func<int64> cartLinq)|] |> fun x -> Script.Of x
```
