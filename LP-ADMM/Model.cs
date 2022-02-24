using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    internal class Model
    {

        readonly List<Variable> Variables = new ();
        readonly List<EqualityConstraint> Constraints = new ();

        public Variable AddVar(double lb, double up, string name)
        {
            var Var = new Variable(lb, up, name);
            Variables.Add(Var);
            return Var;
        }

        private EqualityConstraint AddLinearExpression(LinearExperssion lhs, string name)
        {
            var constraint = new EqualityConstraint(lhs, name);
            Constraints.Add(constraint);
            return constraint;
        }


        public EqualityConstraint AddConstraint(Equality equality, string name)
        {
            if (equality is EQ eq)
            {
                return AddConstr(eq, name);
            }
            if (equality is GEQ geq)
            {
                return AddConstr(geq, name);
            }
            if (equality is LEQ leq)
            {
                return AddConstr(leq, name);
            }
            throw new Exception("");
        }

        public EqualityConstraint AddConstr(EQ geq, string name)
        {
            var newlhs = Combine(geq.LHS, geq.RHS);
            var constrait = AddLinearExpression(newlhs, name);
            return constrait;
        }

        public EqualityConstraint AddConstr(GEQ geq, string name)
        {
            var newlhs = Combine(geq.LHS, geq.RHS);
            var slack = AddVar(0, double.MaxValue, "slack_" + name);
            newlhs.Vars.Add((-1, slack));
            var constrait = AddLinearExpression(newlhs, name);
            return constrait;
        }
        public EqualityConstraint AddConstr(LEQ lhs, string name)
        {
            var newlhs = Combine(lhs.LHS, lhs.RHS);
            var slack = AddVar(0, double.MaxValue, "slack_" + name);
            newlhs.Vars.Add((1, slack));
            var constrait = AddLinearExpression(newlhs, name);
            return constrait;
        }

        private static LinearExperssion Combine(LinearExperssion lhs, LinearExperssion rhs)
        {
            var newlhs = lhs.Copy();
            return newlhs + rhs.Flip();
        }

        internal void Solve(LinearExperssion Objective, double rho)
        {
            for (int k = 0; k < 100000; k++)
            {
                if(k % 1000 == 0 )
                    Console.WriteLine("Objective: {0} {1} {2}", Objective.Eval(), LagrangeIteration(Objective), Constraints.Sum(x => Math.Abs(x.Residual())));
                Iteration(Objective, rho);
            }


        }

        private void Iteration(LinearExperssion Objective, double rho)
        {
            //  Console.WriteLine("--------------------Iteration {0}--------------------", k);
            // Variables.ForEach(x => Console.WriteLine("{0}: {1}", x.Name, x.Value));
            Dictionary<Variable, OptVar> dict = new();
            foreach (var variable in Variables)
            {
                dict[variable] = new OptVar(variable);

            }
            foreach (var (coef, variable) in Objective.Vars)
            {
                dict[variable].B = coef;
            }

            foreach (var constraint in Constraints)
            {

                constraint.UpdateMultiplier(rho);
              //   Console.WriteLine("{0}: {1}  \\:{2}", constraint.Name, constraint.LHS.Eval(), constraint.LagrangreMultiplier);
                constraint.AddToVarOpt(dict, rho);
            }

            Parallel.ForEach(dict, x => { x.Key.Value = x.Value.Optimum(); });
           // Console.WriteLine("Objective: {0} {1} {2}", Objective.Eval(), LagrangeIteration(Objective), Constraints.Sum(x => Math.Abs(x.Residual())));
            //  Console.ReadKey();
        }

        private double LagrangeIteration(LinearExperssion Objective)
        {
            Dictionary<Variable, OptVar> dict = new ();
            foreach (var variable in Variables)
            {
                dict[variable] = new OptVar(variable);

            }
            foreach (var (coef, variable) in Objective.Vars)
            {
                dict[variable].B = coef;
            }

            foreach (var constraint in Constraints)
            {
                constraint.AddToVarOptLagrange(dict);
            }
            Dictionary<Variable, double> DictValues = new ();
            foreach (var (variable,optvar) in dict)
            { var opt = optvar.Optimum();
                //Console.WriteLine("{0} {1} {2} {3}", optvar.A,optvar.B,optvar.C, opt);
                DictValues[variable] =opt ; 

            }
         //   Console.WriteLine("{0} {1}", Objective.EvalLagrange(DictValues), Constraints.Sum(x => x.Peneltylagrange(DictValues)));

            return Objective.EvalLagrange(DictValues) + Constraints.Sum(x => x.Peneltylagrange(DictValues));
        }
    }
}
