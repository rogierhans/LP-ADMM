using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM;

public partial class LinearExperssion
{
    public List<(double, Variable)> Vars = new();
    public double Constant;

    public LinearExperssion(double constant)
    {
        Constant = constant;
    }
    public LinearExperssion(List<(double, Variable)> vars, double constant)
    {
        Vars = vars;
        Constant = constant;
    }


    public LinearExperssion AddConstant(double constant)
    {
        var newLE = Copy();
        newLE.Constant += constant;
        return newLE;
    }

    public LinearExperssion Copy()
    {
        return new LinearExperssion(new(Vars), Constant);
    }

    public double Eval()
    {
        return Constant + Vars.Select(x => x.Item1 * x.Item2.Value).Sum();

    }

    public LinearExperssion Flip()
    {
        var newLE = Copy();
        newLE.Constant = (-1) * Constant;
        newLE.Vars = newLE.Vars.Select(x => ((-1) * x.Item1, x.Item2)).ToList();
        return newLE;
    }
    internal double EvalLagrange(Dictionary<Variable, double> dictValues)
    {
        double sum = Constant;
        foreach (var (coeff, variable) in Vars)
        {
            sum += coeff * dictValues[variable];
          //  Console.WriteLine(sum);
        }
        return sum;
    }
}
