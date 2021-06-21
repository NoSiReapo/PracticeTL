using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorLib
{
    public interface IOperation
    {
        string OperatorCode { get; }
        int Apply(int operand1, int operand2);
    }
}
