using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Models;
using VendingMachine.Services;

namespace VendingMachine.Test
{
    [TestClass]
    public class CoinChangerServiceTest
    {
        [TestMethod]
        public void MakeChange_SmallAmount_Success()
        {
            // Arrange
            var service = new CoinChangerService();

            // Act
            var change = service.CalculateChange(new List<Coin>
            {
                new Coin { Id = 1, Quantity = 10, Value = 0.10M },
                new Coin { Id = 2, Quantity = 10, Value = 0.20M },
                new Coin { Id = 3, Quantity = 10, Value = 0.50M },
                new Coin { Id = 4, Quantity = 10, Value = 1.00M },
            },
            4.20M);

            // Assert
            Assert.IsTrue(change.SequenceEqual(new List<Coin>
            {
                new Coin { Value = 1.00M, Quantity = 4 },
                new Coin { Value = 0.20M, Quantity = 1 },

            }, new CoinChangeComparer()));
        }


        [TestMethod]
        public void MakeChange_QuantityDepleted_Success()
        {
            // Arrange
            var service = new CoinChangerService();

            // Act
            var change = service.CalculateChange(new List<Coin>
            {
                new Coin { Id = 1,  Quantity = 10, Value = 0.10M },
                new Coin { Id = 2,  Quantity = 10, Value = 0.20M },
                new Coin { Id = 3,  Quantity = 3, Value = 0.50M },
                new Coin { Id = 4,  Quantity = 3, Value = 1.00M },
            },
            6.60M);

            // Assert
            Assert.IsTrue(change.SequenceEqual(new List<Coin>
            {
                new Coin { Value = 1.00M, Quantity = 3 }, // 3
                new Coin { Value = 0.50M, Quantity = 3 }, // 4.5          
                new Coin { Value = 0.20M, Quantity = 10 },  // 6.5
                new Coin { Value = 0.10M, Quantity = 1 }, // 6.6

            }, new CoinChangeComparer()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MakeChange_InsufficientCoinsPositiveRemain_Success()
        {
            // Arrange
            var service = new CoinChangerService();

            // Act
            var change = service.CalculateChange(new List<Coin>
            {
                new Coin { Id = 1,  Quantity = 1, Value = 0.10M },
                new Coin { Id = 2,  Quantity = 0, Value = 0.20M },
                new Coin { Id = 3,  Quantity = 2, Value = 0.50M },
                new Coin { Id = 4,  Quantity = 1, Value = 1.00M },
            },
            6.60M);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MakeChange_InsufficientCoinsNegativeRemain_Success()
        {
            // Arrange
            var service = new CoinChangerService();

            // Act
            var change = service.CalculateChange(new List<Coin>
            {
                new Coin { Id = 1,  Quantity = 0, Value = 0.10M },
                new Coin { Id = 2,  Quantity = 0, Value = 0.20M },
                new Coin { Id = 3,  Quantity = 2, Value = 0.50M },
                new Coin { Id = 4,  Quantity = 10, Value = 1.00M },
            },
            6.60M);
        }
    }

    public class CoinChangeComparer : IEqualityComparer<Coin>
    {

        /// <summary>
        /// Whether the two strings are equal
        /// </summary>
        public bool Equals(Coin x, Coin y)
        {
            return x.Value == x.Value;
        }

        /// <summary>
        /// Return the hash code for this string.
        /// </summary>
        public int GetHashCode(Coin obj)
        {
            return obj.Value.GetHashCode();
        }
    }
}
