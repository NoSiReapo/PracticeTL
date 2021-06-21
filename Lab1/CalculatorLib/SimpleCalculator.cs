using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CalculatorLib
{
    public class SimpleCalculator : ICalculator
    {
        private readonly List<IOperation> _supportedOperations;
        
        public SimpleCalculator(List<IOperation> operations)
        {
            _supportedOperations = operations;
        }
            
        public int Calculate(string example)
        {
            string[] numbersAndOperations = Regex
                .Replace(example, @"([\d]+)", "'$1'")
                .Split(new[] { '\'' }, StringSplitOptions.RemoveEmptyEntries);

            int result = int.Parse(numbersAndOperations[0]);
            for (int i = 1; i < numbersAndOperations.Length; i +=2)
            {

                string operationCode = numbersAndOperations[i].Trim();
                IOperation operation = _supportedOperations.Find(x => x.OperatorCode == operationCode);
                int number = int.Parse(numbersAndOperations[i + 1]);
                result = operation.Apply(result, number);
            }
            return result;

        }
    }
}
