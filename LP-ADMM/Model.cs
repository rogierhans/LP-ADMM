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
        readonly List<ADMM_EQ_CONSTRAINT> Constraints = new ();

        public Variable AddVar(double lb, double up, string name)
        {
            var Var = new Variable(lb, up, name);
            Variables.Add(Var);
            return Var;
        }

        private ADMM_EQ_CONSTRAINT AddLinearExpression(LinearExperssion lhs, string name)
        {
            var constraint = new ADMM_EQ_CONSTRAINT(lhs, name);
            Constraints.Add(constraint);
            return constraint;
        }


        public ADMM_EQ_CONSTRAINT AddConstraint(Equality equality, string name)
        {
            if (equality.Type == Equality.EQType.EQ)
            {
                var newlhs = ShiftRHSToLHS(equality.LHS, equality.RHS);
                var constrait = AddLinearExpression(newlhs, name);
                return constrait;
            }
            if (equality.Type == Equality.EQType.GEQ)
            {
                var newlhs = ShiftRHSToLHS(equality.LHS, equality.RHS);
                var slack = AddVar(0, double.MaxValue, "slack_" + name);
                newlhs.Vars.Add((-1, slack));
                var constrait = AddLinearExpression(newlhs, name);
                return constrait;
            }
            if (equality.Type == Equality.EQType.LEQ)
            {
                var newlhs = ShiftRHSToLHS(equality.LHS, equality.RHS);
                var slack = AddVar(0, double.MaxValue, "slack_" + name);
                newlhs.Vars.Add((1, slack));
                var constrait = AddLinearExpression(newlhs, name);
                return constrait;
            }
            throw new Exception("");
        }

        private static LinearExperssion ShiftRHSToLHS(LinearExperssion lhs, LinearExperssion rhs)
        {
            var newlhs = lhs.Copy();
            return newlhs + rhs.Flip();
        }

        internal void Solve(LinearExperssion Objective, double rho)
        {
            for (int k = 0; k < 100000; k++)
            {
                if(k % 1000 == 0 )
                    Console.WriteLine("Objective: {0} {1}", Objective.Eval(), Constraints.Sum(x => Math.Abs(x.Residual())));
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
               //  Console.WriteLine("{0}: {1}  \\:{2}", constraint.Name, constraint.LHS.Eval(), constraint.LagrangreMultiplier);
                constraint.AddToVarOpt(dict, rho);
            }

            Parallel.ForEach(dict, x => { x.Key.Value = x.Value.Optimum(); });
           // Console.WriteLine("Objective: {0} {1} {2}", Objective.Eval(), LagrangeIteration(Objective), Constraints.Sum(x => Math.Abs(x.Residual())));
            //  Console.ReadKey();
        }

    }
}
