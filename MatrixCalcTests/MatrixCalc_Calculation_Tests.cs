using System;
using Xunit;
using MatrixCalc;

namespace MatrixCalc.Tests
{
    public class MatrixTests
    {
        // Test for Power method
        [Fact]
        public void Power_WhenCalledWithZeroPower_ReturnsIdentityMatrix()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Power(0);

            double[,] expected = { { 1, 0 }, { 0, 1 } };
            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Power_WhenCalledWithPositivePower_ReturnsCorrectMatrix()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Power(2);

            double[,] expected = { { 7, 10 }, { 15, 22 } };
            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Power_WhenCalledWithNegativePower_ThrowsException()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            Assert.Throws<InvalidOperationException>(() => matrix.Power(-1));
        }

        [Fact]
        public void Power_WhenCalledWithNonSquareMatrix_ThrowsException()
        {
            double[,] matrixData = { { 1, 2, 3 }, { 4, 5, 6 } };
            var matrix = new Matrix(matrixData);

            Assert.Throws<InvalidOperationException>(() => matrix.Power(2));
        }

        // Test for Transpose method
        [Fact]
        public void Transpose_WhenCalled_ReturnsTransposedMatrix()
        {
            double[,] matrixData = { { 1, 2, 3 }, { 4, 5, 6 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Transpose();

            double[,] expected = { { 1, 4 }, { 2, 5 }, { 3, 6 } };
            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Transpose_WhenCalledOnSingleElementMatrix_ReturnsSameMatrix()
        {
            double[,] matrixData = { { 1 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Transpose();

            Assert.Equal(matrixData, result._matrix);
        }

        [Fact]
        public void Transpose_WhenCalledOnSquareMatrix_ReturnsMatrixWithTransposedValues()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Transpose();

            double[,] expected = { { 1, 3 }, { 2, 4 } };
            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Transpose_WhenCalledOnMatrixWithDifferentRowAndColumnCounts_ReturnsTransposedMatrix()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Transpose();

            double[,] expected = { { 1, 3, 5 }, { 2, 4, 6 } };
            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Transpose_WhenCalledOnEmptyMatrix_ReturnsEmptyMatrix()
        {
            double[,] matrixData = { };
            var matrix = new Matrix(matrixData);

            var result = matrix.Transpose();

            Assert.Empty(result._matrix);
        }

        // Test for Rank method
        [Fact]
        public void Rank_WhenCalledOnIdentityMatrix_ReturnsMatrixSize()
        {
            double[,] matrixData = { { 1, 0 }, { 0, 1 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Rank();

            Assert.Equal(2, result);
        }

        [Fact]
        public void Rank_WhenCalledOnMatrixWithFullRank_ReturnsFullRank()
        {
            double[,] matrixData = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Rank();

            Assert.Equal(3, result);
        }

        [Fact]
        public void Rank_WhenCalledOnZeroMatrix_ReturnsZero()
        {
            double[,] matrixData = { { 0, 0 }, { 0, 0 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Rank();

            Assert.Equal(0, result);
        }

        [Fact]
        public void Rank_WhenCalledOnMatrixWithDifferentRowAndColumnCounts_ReturnsRankOfTheMatrix()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Rank();

            Assert.Equal(2, result);
        }

        [Fact]
        public void Rank_WhenCalledOnMatrixWithRepeatedRows_ReturnsCorrectRank()
        {
            double[,] matrixData = { { 1, 2 }, { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Rank();
             
            Assert.Equal(2, result);
        }

        // Test for Inverse method
        [Fact]
        public void Inverse_WhenCalledOnIdentityMatrix_ReturnsIdentityMatrix()
        {
            double[,] matrixData = { { 1, 0 }, { 0, 1 } };
            var matrix = new Matrix(matrixData);

            var result = matrix.Inverse();

            Assert.Equal(matrixData, result._matrix);
        }

        [Fact]
        public void Inverse_WhenCalledOnNonInvertibleMatrix_ThrowsException()
        {
            double[,] matrixData = { { 1, 2 }, { 2, 4 } };
            var matrix = new Matrix(matrixData);

            Assert.Throws<InvalidOperationException>(() => matrix.Inverse());
        }

        [Fact]
        public void Inverse_WhenCalledOn2x2Matrix_ReturnsCorrectInverse()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            double[,] expected = { { -2, 1 }, { 1.5, -0.5 } };
            var result = matrix.Inverse();

            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Inverse_WhenCalledOnSingleElementMatrix_ReturnsInverse()
        {
            double[,] matrixData = { { 5 } };
            var matrix = new Matrix(matrixData);

            double[,] expected = { { 0.2 } };
            var result = matrix.Inverse();

            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void Inverse_WhenCalledOnMatrixWithZeroElement_ThrowsException()
        {
            double[,] matrixData = { { 0, 1 }, { 1, 0 } };
            var matrix = new Matrix(matrixData);

            Assert.Throws<InvalidOperationException>(() => matrix.Inverse());
        }

        // Test for ScalarMultiply method
        [Fact]
        public void ScalarMultiply_WhenCalledWithScalar_ReturnsCorrectMatrix()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            double[,] expected = { { 2, 4 }, { 6, 8 } };
            var result = matrix.ScalarMultiply(2);

            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void ScalarMultiply_WhenCalledWithZero_ReturnsZeroMatrix()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            double[,] expected = { { 0, 0 }, { 0, 0 } };
            var result = matrix.ScalarMultiply(0);

            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void ScalarMultiply_WhenCalledWithNegativeScalar_ReturnsMatrixWithNegatedValues()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            double[,] expected = { { -1, -2 }, { -3, -4 } };
            var result = matrix.ScalarMultiply(-1);

            Assert.Equal(expected, result._matrix);
        }

        [Fact]
        public void ScalarMultiply_WhenCalledWithFractionalScalar_ReturnsMatrixWithScaledValues()
        {
            double[,] matrixData = { { 1, 2 }, { 3, 4 } };
            var matrix = new Matrix(matrixData);

            double[,] expected = { { 0.5, 1 }, { 1.5, 2 } };
            var result = matrix.ScalarMultiply(0.5);

            Assert.Equal(expected, result._matrix);
        }
    }
}
