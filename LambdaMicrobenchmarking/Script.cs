using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaMicrobenchmarking
{

    public static class Script
    {
        public static Script<T> Of<T>(params Tuple<String, Func<T>>[] actions)
        {
            return Script<T>.Of(actions);
        }

        public static Script<T> Of<T>(String name, Func<T> action)
        {
            return Of(Tuple.Create(name, action));
        }
    }

    public class Script<T>
    {
        static public int Iterations
        {
            get { return Run<T>.iterations; }
            set { Run<T>.iterations = value; }
        }

        static public int WarmupIterations
        {
            get { return Run<T>.warmups; }
            set { Run<T>.warmups = value; }
        }

        public static double MinRunningSecs
        {
            get { return Run<T>.minimumSecs; }
            set { Run<T>.minimumSecs = value; }
        }

        private List<Tuple<String, Func<T>>> actions { get; set; }

        private Script(params Tuple<String, Func<T>>[] actions)
        {
            this.actions = actions.ToList();
        }

        public static Script<T> Of(params Tuple<String, Func<T>>[] actions)
        {
            return new Script<T>(actions);
        }

        public Script<T> Of(String name, Func<T> action)
        {
            actions.Add(Tuple.Create(name,action));
            return this;
        }

        public Script<T> WithHead()
        {
            Console.WriteLine(Run<T>.FORMAT, "Benchmark", "Mean", "Mean-Error", "Sdev", "Unit", "Count");
            return this;
        }

        public Script<T> RunAll()
        {
            actions.Select(action => new Run<T>(action)).ToList().ForEach(run => run.Measure());
            return this;
        }
    }
}
