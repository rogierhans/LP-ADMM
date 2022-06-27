// See https://aka.ms/new-console-template for more information
using Gurobi;
using System.Diagnostics;
namespace LP_ADMM;

public class Program
{
    public static void Main()
    {
        while (true)
            Test();

        //var rogierVar = new GRBVar();
        //GRBLinExpr linear = rogierVar;
        //GRBQuadExpr fromlinear = new GRBQuadExpr(linear);
        //Console.WriteLine("{0} {1}", linear.Size, fromlinear.Size);
        //var rogierTest = new GRBQuadExpr();
        //var rogierLastLine = rogierTest + rogierVar;


        //var model = new Model();

        //var x1 = model.AddVar(0, 100, "x_1");
        //var x2 = model.AddVar(0, 100, "x_2");
        //var x3 = model.AddVar(0, 100, "x_3");
        //var x4 = model.AddVar(0, 100, "x_4");
        //var x5 = model.AddVar(0, 100, "x_5");
        //var x6 = model.AddVar(0, 100, "x_6");

        //model.AddConstraint(5 * x1 + 4 * x2 + 4 * x3 + 3 * x4 + 2 * x5 + 4 * x6 <= 120, "First Constraint");
        //model.AddConstraint(3 * x1 + 2 * x2 + 4 * x3 + 2 * x4 + 4 * x5 + 3 * x6 <= 180, "Second Constraint");
        //var objective = -4 * x1 + -3 * x2 + -6 * x3 + -3 * x4 + -4 * x5 + -4 * x6;
        //var sw = new Stopwatch();
        //sw.Start();
        //model.Solve(objective, 1);
        //Console.WriteLine(sw.Elapsed.TotalMilliseconds);
    }

    public static void Test()
    {
        List<(GRBVar, Variable)> list = new();
        Random rng = new Random();
        GRBEnv env = new GRBEnv();
        GRBModel grbModel = new GRBModel(env);
        var model = new Model();
        for (int i = 0; i < 100; i++)
        {
            var lb = 0;

            var up = lb + rng.NextDouble() * 100;
            //Console.WriteLine("{0} {1}", lb, up);
            var grbvar = grbModel.AddVar(lb, up, 0.0, GRB.CONTINUOUS, "");
            var variable = model.AddVar(lb, up, "");

            list.Add((grbvar, variable));
        }
        for (int i = 0; i < 100; i++)
        {
            var rhs = rng.NextDouble() * 100;
            var grblinearExperssion = new GRBLinExpr();
            var LE = new LinearExperssion(0);
            foreach (var (grbvar, variable) in list)
            {
                var coef = rng.NextDouble() * 10 - 3;
                grblinearExperssion += grbvar * coef;
                LE += coef * variable;
            }
            grbModel.AddConstr(grblinearExperssion >= rhs, "");
            model.AddConstraint(LE >= rhs, "");
        }
        var grbobjective = new GRBLinExpr();
        var objective = new LinearExperssion(0);
        foreach (var (grbvar, variable) in list)
        {
            var coef = rng.NextDouble() * -3;
            grbobjective += grbvar * coef;
            objective += coef * variable;
        }
        grbModel.SetObjective(grbobjective);
        grbModel.Optimize();
        model.Solve(objective, 1);
        foreach (var (grbvar, variable) in list)
        {
            Console.WriteLine("{0} {1}", grbvar.X, variable.Value);
        }
        Console.ReadLine();
    }
}
