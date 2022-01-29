using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{

    public class Equality
    {

    }
    public class GEQ : Equality
    {
        public LinearExperssion LHS;
        public LinearExperssion RHS;

        public GEQ(LinearExperssion lHS, LinearExperssion rHS)
        {
            LHS = lHS;
            RHS = rHS;
        }
    }
    public class LEQ : Equality
    {
        public LinearExperssion LHS;
        public LinearExperssion RHS;

        public LEQ(LinearExperssion lHS, LinearExperssion rHS)
        {
            LHS = lHS;
            RHS = rHS;
        }


    }
    public class EQ : Equality
    {
        public LinearExperssion LHS;
        public LinearExperssion RHS;

        public EQ(LinearExperssion lHS, LinearExperssion rHS)
        {
            LHS = lHS;
            RHS = rHS;
        }


    }

}

