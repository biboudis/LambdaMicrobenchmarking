## Lambda Microbenchmarking

This is an initial code for a microbenchmarking facility* for testing C# and F# lambdas.
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
   
script.RunAll() |> ignore
```
Sample output:
```
Benchmark          Mean      Mean-Error  Sdev  Unit
sumLinq          98.324           0.543 0.359 ms/op
sumSqLinq       224.831           1.804 1.193 ms/op
sumSqEvenLinq    82,789           5,729 3,789 ms/op
cartLinq        200,775           6,477 4,284 ms/op
```


\* the statistics part is inspired by [JMH](http://openjdk.java.net/projects/code-tools/jmh/) ([AbstractStatistics](http://hg.openjdk.java.net/code-tools/jmh/file/75f8b23444f6/jmh-core/src/main/java/org/openjdk/jmh/util/internal/AbstractStatistics.java), [ListStatistics](http://hg.openjdk.java.net/code-tools/jmh/file/75f8b23444f6/jmh-core/src/main/java/org/openjdk/jmh/util/internal/ListStatistics.java)).
 
### References
* [Clash of the Lambdas](http://biboudis.github.io/clashofthelambdas/)
* [Microbenchmarks in Java and C#](http://www.itu.dk/people/sestoft/papers/benchmarking.pdf)
