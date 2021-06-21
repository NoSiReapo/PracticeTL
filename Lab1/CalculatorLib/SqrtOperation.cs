using System;

namespace CalculatorLib
{
    public class SqrtOperation : IOperation
    {
        public string OperatorCode
        {
            get { return "^"; }
        }

        public int Apply(int operand1, int operand2)
        {
            return (int) Math.Pow(operand1 ,(double) 1/operand2);
        }
    }
}
