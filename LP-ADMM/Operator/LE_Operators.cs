using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
    public partial class LinearExperssion
    {
        public static LinearExperssion operator +(LinearExperssion a, LinearExperssion b)
        {
            var newLE = a.Copy();
            newLE.Constant += b.Constant;
            newLE.Vars.AddRange(b.Vars);
            return newLE;
        }

        public static LinearExperssion operator +(LinearExperssion a, double b)
        {
            return a.AddConstant(b);
        }

        public static LinearExperssion operator +(double b, LinearExperssion a)
        {
            return a.AddConstant(b);
        }


        public static LinearExperssion operator -(LinearExperssion a, LinearExperssion b)
        {
            return a + b.Flip();
        }

        public static LinearExperssion operator -(LinearExperssion a, double b)
        {
            return a.AddConstant(-b);
        }

        public static LinearExperssion operator -(double b, LinearExperssion a)
        {
            var newLE = a.Flip();
            return newLE.AddConstant(b);
        }

        public static Equality operator <=(LinearExperssion one, LinearExperssion two)
        {
            return new LEQ(one, two);
        }

        public static Equality operator <=(LinearExperssion one, double two)
        {
            return new LEQ(one, new LinearExperssion( two));
        }
        public static Equality operator >=(LinearExperssion one, double two)
        {
            return new GEQ(one, new LinearExperssion( two));
        }
        public static Equality operator >=(LinearExperssion one, LinearExperssion two)
        {
            return new GEQ(one, two);
        }

        public static Equality operator ==(LinearExperssion one, LinearExperssion two)
        {
            return new EQ(one, two);
        }

        public static Equality operator !=(LinearExperssion one, LinearExperssion two)
        {

            throw new NotImplementedException("C# forces me to define this operator");
        }


    }
}
