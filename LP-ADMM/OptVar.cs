using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    internal class OptVar
    {
        public Variable Var;
        public double A=0, B=0, C=0;

        public OptVar(Variable var)
        {
            Var = var;
        }

        internal double Optimum()
        {
            if (C == 0)
            {
                if (B > 0)
                {
                    return Var.LB;
                }
                else
                {
                    return Var.UB;
                }
            }
            double minimum = (-B / (2 * C));
            if (minimum < Var.LB)
            {
                return Var.LB;
            }
            else if (minimum > Var.UB)
            {
                return Var.UB;
            }
            else
            {
                return minimum;
            }
        }
    }
}
