namespace CalculatorLib
{
    public class RemainderOperation : IOperation
    {
        public string OperatorCode
        {
            get { return "%"; }
        }

        public int Apply(int operand1, int operand2)
        {
            return operand1 % operand2;
        }
    }
}
