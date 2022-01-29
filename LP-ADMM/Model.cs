using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    internal class Model
    {

        List<Variable> Variables = new List<Variable>();
        List<EqualityConstraint> Constraints = new List<EqualityConstraint>();

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


        public EqualityConstraint AddConstraint(Equality eq, string name)
        {
            if (eq is EQ dumb) {
                return AddConstr(dumb, name);
            }
            if (eq is GEQ lol)
            {
                return AddConstr(lol, name);
            }
            if (eq is LEQ waat)
            {
                return AddConstr(waat, name);
            }
            throw new Exception("");
        }

        public EqualityConstraint AddConstr(EQ geq, string name)
        {
            var newlhs = Combine(geq.LHS, geq.RHS);
            var constrait = AddLinearExpression(newlhs, name);
            return constrait;
        }

        public  EqualityConstraint AddConstr(GEQ geq, string name)
        {
            var newlhs = Combine(geq.LHS, geq.RHS);
            var slack = AddVar(0, double.MaxValue, "slack_" + name);
            newlhs.Vars.Add((-1, slack));
            var constrait = AddLinearExpression(newlhs, name);
            return constrait;
        }
        public  EqualityConstraint AddConstr(LEQ lhs, string name)
        {
            var newlhs = Combine(lhs.LHS, lhs.RHS);
            var slack = AddVar(0, double.MaxValue, "slack_" + name);
            newlhs.Vars.Add((1, slack));
            var constrait = AddLinearExpression(newlhs, name);
            return  constrait;
        }

        private LinearExperssion Combine(LinearExperssion lhs, LinearExperssion rhs)
        {
            var newlhs = lhs.Copy();
            return newlhs + rhs.Flip();
        }

        internal void Solve(LinearExperssion Objective, double rho)
        {
            for (int k = 0; k < 10000; k++)
            {
              //  Console.WriteLine("--------------------Iteration {0}--------------------", k);
               // Variables.ForEach(x => Console.WriteLine("{0}: {1}", x.Name, x.Value));
                Dictionary<Variable, OptVar> dict = new Dictionary<Variable, OptVar>();
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
                   // Console.WriteLine("{0}: {1}  \\:{2}", constraint.Name, constraint.LHS.Eval(), constraint.LagrangreMultiplier);
                    constraint.AddToVarOpt(dict, rho);
                }
                Parallel.ForEach(dict, x => { x.Key.Value = x.Value.Optimum(); });
              //  Console.ReadKey();
            }
             Console.WriteLine("Objective: {0} {1} {2}", Objective.Eval(), Objective.Eval() + Constraints.Sum( x => x.Penelty()), Constraints.Sum(x => Math.Abs(x.Residual())));

        }
    }
}
