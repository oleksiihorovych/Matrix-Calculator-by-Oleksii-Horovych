using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Principal;

namespace MatrixCalc
{
    public class Matrix
    {
        public double[,] _matrix { get; set; }
        public int rows { get; }
        public int cols { get; }

        public double this[int row, int col]
        {
            get { return _matrix[row, col]; }
        }

        public Matrix(double[,] matrix)
        {
            _matrix = matrix;
            rows = matrix.GetLength(0);
            cols = matrix.GetLength(1);
        }

        public Matrix Power(int power)
        {
            if (rows != cols)
            {
                throw new InvalidOperationException("Matrix must be square to raise to a power.");
            }

            if (power < 0)
            {
                throw new InvalidOperationException("Exponent must be non-negative.");
            }


            Matrix result = new Matrix(_matrix);

            if (power == 0)
            {
                double[,] identityMatrix = new double[rows, cols];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j ++)
                    {
                        if (i == j) { identityMatrix[i, j] = 1; }
                        else { identityMatrix[i, j] = 0; }
                    }
                }
                return new Matrix(identityMatrix);
            }

            for (int i = 1; i < power; i++)
            {
                result = TwoMatrixOperations.Multiply(result, new Matrix(_matrix));
            }

            return result;
        }
        public Matrix Transpose()
        {
            int rows = this.rows;
            int cols = this.cols;

            double[,] result = new double[cols, rows];

            Debug.WriteLine($"{rows} x {cols}");

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[j, i] = _matrix[i, j];
                }
            }

            return new Matrix(result);
        }
        public int Rank()
        {
            double[,] mat = GaussianElimination()._matrix;
            int rank = 0;
            int rows = this.rows;
            int cols = this.cols;

            for (int i = 0; i < rows; i++)
            {
                bool isZeroRow = true;
                for (int j = 0; j < cols; j++)
                {
                    if (Math.Abs(mat[i, j]) > double.Epsilon)
                    {
                        isZeroRow = false;
                        break;
                    }
                }
                if (!isZeroRow)
                {
                    rank++;
                }
            }

            return rank;
        }


        public Matrix Inverse()
        {
            if (rows != cols)
            {
                throw new InvalidOperationException("Inverse can only be calculated for square matrices.");
            }

            double[,] augmentedMatrix = new double[rows, cols * 2];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    augmentedMatrix[i, j] = _matrix[i, j];
                }
                augmentedMatrix[i, i + cols] = 1;
            }

            for (int i = 0; i < rows; i++)
            {
                // Find pivot
                double pivot = augmentedMatrix[i, i];
                if (Math.Abs(pivot) < double.Epsilon)
                {
                    throw new InvalidOperationException("Matrix is singular, cannot find inverse.");
                }

                for (int j = 0; j < cols * 2; j++)
                {
                    augmentedMatrix[i, j] /= pivot;
                }

                for (int k = 0; k < rows; k++)
                {
                    if (k != i)
                    {
                        double factor = augmentedMatrix[k, i];
                        for (int j = 0; j < cols * 2; j++)
                        {
                            augmentedMatrix[k, j] -= factor * augmentedMatrix[i, j];
                        }
                    }
                }
            }

            double[,] inverseMatrix = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    inverseMatrix[i, j] = augmentedMatrix[i, j + cols];
                }
            }

            return new Matrix(inverseMatrix);
        }


        public Matrix ScalarMultiply(double scalar)
        {
            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = _matrix[i, j] * scalar;
                }
            }
            return new Matrix(result);
        }

        public double Determinant()
        {
            if (rows != cols)
            {
                throw new InvalidOperationException("Determinant can only be calculated for square matrices.");
            }

            double[,] mat = GaussianElimination()._matrix;
            double determinant = 1.0;

            for (int i = 0; i < rows; i++)
            {
                determinant *= mat[i, i];
            }

            return determinant;
        }

        public Matrix GaussianElimination()
        {
            double[,] mat = new double[this.rows, this.cols];

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    mat[i, j] = _matrix[i, j];
                }
            }

            int rows = this.rows;
            int cols = this.cols;

            for (int c = 0; c < Math.Min(rows, cols); c++)
            {
                int pivot_row = c;
                for (int r = c + 1; r < rows; r++)
                {
                    if (Math.Abs(mat[r, c]) > Math.Abs(mat[pivot_row, c]))
                    {
                        pivot_row = r;
                    }
                }

                if (Math.Abs(mat[pivot_row, c]) < 1e-10)
                {
                    continue;
                }

                if (pivot_row != c)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        double temp = mat[c, k];
                        mat[c, k] = mat[pivot_row, k];
                        mat[pivot_row, k] = temp;
                    }
                }

                for (int r = c + 1; r < rows; r++)
                {
                    double factor = mat[r, c] / mat[c, c]; 
                    for (int k = c; k < cols; k++)
                    {
                        mat[r, k] -= factor * mat[c, k];
                    }
                }
            }

            Debug.WriteLine("Basdasdasd");

            return new Matrix(mat);  
        }


        public static class TwoMatrixOperations
        {
            public static Matrix Multiply(Matrix matrix1, Matrix matrix2)
            {
                if (matrix1.cols != matrix2.rows)
                {
                    throw new InvalidOperationException("Matrix dimensions are not valid for multiplication.");
                }

                int rows = matrix1.rows;
                int cols = matrix2.cols;

                double[,] result = new double[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        for (int k = 0; k < matrix1.cols; k++)
                        {
                            result[i, j] += matrix1[i, k] * matrix2[k, j];
                        }
                    }
                }

                return new Matrix(result);
            }

            public static Matrix MatrixAddition(Matrix matrix1, Matrix matrix2)
            {
                if (matrix1.rows != matrix2.rows || matrix1.cols != matrix2.cols)
                {
                    throw new InvalidOperationException("Matrix dimensions are not valid for addition.");
                }

                int rows = matrix1.rows;
                int cols = matrix1.cols;

                double[,] result = new double[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        result[i, j] = matrix1[i, j] + matrix2[i, j];
                    }
                }

                return new Matrix(result);
            }

            public static Matrix MatrixSubtraction(Matrix matrix1, Matrix matrix2)
            {
                if (matrix1.rows != matrix2.rows || matrix1.cols != matrix2.cols)
                {
                    throw new InvalidOperationException("Matrix dimensions are not valid for subtraction.");
                }

                int rows = matrix1.rows;
                int cols = matrix1.cols;

                double[,] result = new double[rows, cols];

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        result[i, j] = matrix1[i, j] - matrix2[i, j];
                    }
                }

                return new Matrix(result);
            }
        }
    }
}