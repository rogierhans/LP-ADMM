using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    internal class EqualityConstraint
    {
        public string Name;
        public LinearExperssion LHS;
        public double LagrangreMultiplier = 0;

        public EqualityConstraint(LinearExperssion lHS, string name)
        {
            LHS = lHS;
            Name = name;
            LagrangreMultiplier = 0;
        }

        public void UpdateMultiplier(double rho)
        {
            LagrangreMultiplier += rho * ResidualAVG();
        }


        public void AddToVarOpt(Dictionary<Variable, OptVar> dict, double rho)
        {
            var residual = ResidualAVG();
            foreach (var (coef, varible) in LHS.Vars)
            {
                var optvar = dict[varible];
                var restResidual = varible.Value * coef - residual;
                optvar.B += LagrangreMultiplier * coef - restResidual * rho * coef;
                optvar.C += (coef * coef) * rho / 2;
            }
        }

        public void AddToVarOptLagrange(Dictionary<Variable, OptVar> dict)
        {
            foreach (var (coef, varible) in LHS.Vars)
            {
                var optvar = dict[varible];
                optvar.B += LagrangreMultiplier * coef;
            }
        }

        public double ResidualAVG()
        {
            return Residual() / LHS.Vars.Count;
        }
        public double Residual()
        {
            double sum = LHS.Constant;
            foreach (var (coef, varible) in LHS.Vars)
            {
                sum += (coef * varible.Value);
            }
            return sum / LHS.Vars.Count;
        }

        public double ResidualLagrange(Dictionary<Variable, double> dictValues)
        {
            double sum = LHS.Constant;
            foreach (var (coef, varible) in LHS.Vars)
            {
                sum += (coef * dictValues[varible]);
            }
            return sum ;
        }
        public double Penelty()
        {
            return Residual() * LagrangreMultiplier;
        }

        internal double Peneltylagrange(Dictionary<Variable, double> dictValues)
        {
            return ResidualLagrange(dictValues) * LagrangreMultiplier;
        }
    }
}
