using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{

    public class Equality
    {
        public enum EQType { EQ , LEQ, GEQ };
        public LinearExperssion LHS;
        public LinearExperssion RHS;
        public EQType Type;

        public Equality(LinearExperssion lHS, LinearExperssion rHS, EQType type  )
        {
            LHS = lHS;
            RHS = rHS;
            Type = type;
        }
    }
}

