using System;
using Xunit;
using System.Collections.Generic;

namespace CalculatorLib.Tests
{
    public class SimpleCalculatorTest
    {
        [Fact]
        public void SimpleCalculator_Calculate_AdditionOperation_Calculated()
        {
            //Arrange
            List<IOperation> operations = new List<IOperation>
            {
                new AdditionOperation()
            };
            SimpleCalculator simpleCalculator = new SimpleCalculator(operations);

            //Act
            int result = simpleCalculator.Calculate("2+2");

            //Assert
            Assert.Equal(4, result);
        }
        [Fact]
        public void SimpleCalculator_Calculate_SubstractionOperation_Calculated()
        {
            //Arrange
            List<IOperation> operations = new List<IOperation>
            {
                new SubstractionOperation()
            };
            SimpleCalculator simpleCalculator = new SimpleCalculator(operations);
            //Act
            int result = simpleCalculator.Calculate("2-3");

            //Assert
            Assert.Equal(-1, result);
        }
    }
}
