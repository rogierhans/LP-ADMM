using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LP_ADMM
{
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    public partial class LinearExperssion
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
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


            return new Equality(one, two, Equality.EQType.LEQ);
        }

        public static Equality operator <=(LinearExperssion one, double two)
        {
            return new Equality(one, new LinearExperssion(two), Equality.EQType.LEQ);
        }
        public static Equality operator >=(LinearExperssion one, double two)
        {
            return new Equality(one, new LinearExperssion(two), Equality.EQType.GEQ);
        }
        public static Equality operator >=(LinearExperssion one, LinearExperssion two)
        {
            return new Equality(one, two, Equality.EQType.GEQ);
        }

        public static Equality operator ==(LinearExperssion one, LinearExperssion two)
        {
            return new Equality(one, two, Equality.EQType.EQ);
        }

        public static Equality operator !=(LinearExperssion one, LinearExperssion two)
        {

            throw new NotImplementedException("C# forces me to define this operator");
        }


    }
}
