// See https://aka.ms/new-console-template for more information
using Gurobi;
using System.Diagnostics;
namespace LP_ADMM;

public class Program
{
    public static void Main(string[] args)
    {
        var model = new Model();

        var x1 = model.AddVar(0, 100, "x_1");
        var x2 = model.AddVar(0, 100, "x_2");
        var x3 = model.AddVar(0, 100, "x_3");
        var x4 = model.AddVar(0, 100, "x_4");
        var x5 = model.AddVar(0, 100, "x_5");
        var x6 = model.AddVar(0, 100, "x_6");

        model.AddConstraint( 5 * x1 + 4 * x2 + 4 * x3 + 3 * x4 + 2 * x5 + 4 * x6 <= 120, "First Constraint");
        model.AddConstraint(3 * x1 + 2 * x2 + 4 * x3 + 2 * x4 + 4 * x5 + 3 * x6 <= 180, "Second Constraint");
        var objective = -4 * x1 + -3 * x2 + -6 * x3 + -3 * x4 + -4 * x5 + -4 * x6;
        var sw = new Stopwatch();
        sw.Start();
        model.Solve(objective,0.1);
        Console.WriteLine(sw.Elapsed.TotalMilliseconds);
    }
}
