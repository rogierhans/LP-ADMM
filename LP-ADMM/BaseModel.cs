using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    internal class BaseModel
    {

        readonly List<Variable> Variables = new();
        readonly List<Equality> Constraints = new();

        public Variable AddVar(double lb, double up, string name)
        {
            var Var = new Variable(lb, up, name);
            Variables.Add(Var);
            return Var;
        }


        public void AddConstraint(Equality equality)
        {

        }

    }
}
