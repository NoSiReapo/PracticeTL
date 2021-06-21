using System;

namespace CalculatorLib
{
    public class ExponentationOperaion : IOperation
    {
        public string OperatorCode
        {
            get { return "**"; }
        }

        public int Apply( int operand1, int operand2 )
        {
            return (int) Math.Pow( operand1, operand2 );
        }
    }
}
