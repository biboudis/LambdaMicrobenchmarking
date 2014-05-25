using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdaMicrobenchmarking
{
    public class Script<T>
    {
        static public int Iterations { get; set; }
        static public int WarmupIterations { get; set; }
        private Tuple<String, Func<T>>[] actions { get; set; }

        private Script(params Tuple<String, Func<T>>[] actions)
        {
            this.actions = actions;
        }

        public static Script<T> Of(Tuple<String, Func<T>>[] actions)
        {
            return new Script<T>(actions);
        }

        public Script<T> RunAll()
        {
            Console.WriteLine("{0,-25} \t{1,10} {2,6:0.00} {3,6:0.00} {4,5}", "Benchmark", "Mean", "Mean-Error", "Sdev", "Unit");

            actions.Select(action => new Run<T>(action)).ToList().ForEach(run => run.Measure());
            return this;
        }
    }
}
