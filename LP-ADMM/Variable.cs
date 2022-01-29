using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    public class Variable
    {
        public double UB;
        public double LB;
        public string Name;

        public double Value;

        public Variable(double lb, double ub, string name)
        {
            UB = ub;
            LB = lb;
            Name = name;
            Value = lb;
        }

        public static LinearExperssion operator* (double a, Variable b)
        {
            return new LinearExperssion( new List<(double, Variable)> { (a, b) } , 0 );
        }

        public static LinearExperssion operator +(Variable a, Variable b)
        {
            return new LinearExperssion(new List<(double, Variable)> { (1,a),(1, b) }, 0);
        }
        public static LinearExperssion operator +(LinearExperssion a, Variable b)
        {
            var newLE = a.Copy();
            newLE.Vars.Add((1, b));
            return newLE;
        }
        public static LinearExperssion operator +(Variable b, LinearExperssion a)
        {
            var newLE = a.Copy();
            newLE.Vars.Add((1, b));
            return newLE;
        }

        public static LinearExperssion operator -(Variable a, Variable b)
        {
            return new LinearExperssion(new List<(double, Variable)> { (1, a), (-1, b) }, 0);
        }
        public static LinearExperssion operator -(LinearExperssion a, Variable b)
        {
            var newLE = a.Copy();
            newLE.Vars.Add((-1, b));
            return newLE;
        }
        public static LinearExperssion operator -(Variable a, LinearExperssion b)
        {
            var newLE = b.Flip();
            newLE.Vars.Add((1, a));
            return newLE;
        }
    }
}
