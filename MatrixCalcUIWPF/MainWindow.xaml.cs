using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using MatrixCalc;
using Microsoft.VisualBasic;
using Matrix = MatrixCalc.Matrix;

namespace MatrixCalcUIWPF
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateButtonStates(MatrixA);
            UpdateButtonStates(MatrixB);
        }

        //--------- Helper Methods --------- //
        private List<TextBox> GetTextBoxes(Grid matrixGrid)
        {
            var textBoxes = new List<TextBox>();
            foreach (var child in matrixGrid.Children)
            {
                if (child is TextBox textBox)
                {
                    textBoxes.Add(textBox);
                }
            }
            return textBoxes;
        }

        private string MatrixToString(Matrix matrix)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < matrix.rows; i++)
            {
                for (int j = 0; j < matrix.cols; j++)
                {
                    double value = matrix[i, j];
                    if (value % 1 == 0)
                    {
                        sb.Append($"{value,8:F0} ");
                    }
                    else
                    {
                        sb.Append($"{value,8:F2} ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        private void DisplayResult(string nameOfOperation, string result, bool isMatrix)
        {

            Grid resultsGrid = new Grid();
            resultsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            resultsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            StackPanel resultStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };

            if (isMatrix == true)
            {
                StackPanel buttonStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Button pasteToMatrixA = new Button
                {
                    Content = "Paste to A",
                    Width = 190,
                    Height = 25,
                    Margin = new Thickness(0, 0, 0, 0),
                    Style = (Style)FindResource("RoundedButtonStyle"),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                pasteToMatrixA.Click += PasteToMatrixA_Click;

                Button pasteToMatrixB = new Button
                {
                    Content = "Paste to B",
                    Width = 190,
                    Height = 25,
                    Margin = new Thickness(0, 0, 0, 0),
                    Style = (Style)FindResource("RoundedButtonStyle"),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };
                pasteToMatrixB.Click += PasteToMatrixB_Click;

                buttonStackPanel.Children.Add(pasteToMatrixA);
                buttonStackPanel.Children.Add(pasteToMatrixB);

                resultsGrid.Children.Add(buttonStackPanel);
                Grid.SetColumn(buttonStackPanel, 1);
            }
                TextBlock operationTextBlock = new TextBlock
                {
                    Text = nameOfOperation + " = ",
                    Margin = new Thickness(5),
                    FontSize = 16,
                    FontFamily = new FontFamily("Consolas"),
                    VerticalAlignment = VerticalAlignment.Center
                };

                TextBlock resultTextBlock = new TextBlock
                {
                    Text = result,
                    Margin = new Thickness(5),
                    FontSize = 16,
                    FontFamily = new FontFamily("Consolas"),
                    VerticalAlignment = VerticalAlignment.Center
                };

                resultStackPanel.Children.Add(operationTextBlock);
                resultStackPanel.Children.Add(resultTextBlock);

                resultsGrid.Children.Add(resultStackPanel);
                Grid.SetColumn(resultStackPanel, 0);

                Border border = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(0, 0, 0, 1),
                    Margin = new Thickness(0, 0, 0, 10),
                    Child = resultsGrid
                };

                ResultsStackPanel.Children.Insert(0, border);
            }

        private void PasteToMatrix(Grid targetMatrix)
        {
            string resultString = null;
            foreach (UIElement element in ResultsStackPanel.Children)
            {
                if (element is Border border)
                {
                    var resultsGrid = border.Child as Grid;
                    if (resultsGrid != null)
                    {
                        var resultTextBlock = resultsGrid.Children.OfType<StackPanel>()
                            .SelectMany(sp => sp.Children.OfType<TextBlock>())
                            .LastOrDefault();
                        if (resultTextBlock != null)
                        {
                            resultString = resultTextBlock.Text;
                            break;
                        }
                    }
                }
            }

            var lines = resultString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var matrixData = new List<List<double>>();
            foreach (var line in lines)
            {
                var values = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => double.TryParse(v, out double num) ? num : 0)
                    .ToList();
                if (values.Any())
                {
                    matrixData.Add(values);
                }
            }

            int rows = matrixData.Count;
            int cols = matrixData[0].Count;
            double[,] matrixArray = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixArray[i, j] = matrixData[i][j];
                }
            }

            UpdateMatrixGrid(targetMatrix, matrixArray);
        }
        private void PasteToMatrixA_Click(object sender, RoutedEventArgs e)
        {
            PasteToMatrix(MatrixA);
        }

        private void PasteToMatrixB_Click(object sender, RoutedEventArgs e)
        {
            PasteToMatrix(MatrixB);
        }
        private bool ReadMatrix(Grid grid, out Matrix matrix)
        {
            var values = new Dictionary<(int row, int col), double>();
            var errorMessages = new List<string>();
            int maxRow = -1;
            int maxCol = -1;

            bool isEmpty = true;

            foreach (UIElement element in grid.Children)
            {
                if (element is TextBox textBox)
                {
                    int row = Grid.GetRow(textBox);
                    int column = Grid.GetColumn(textBox);

                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        values[(row, column)] = 0; 
                    }
                    else if (double.TryParse(textBox.Text, out double value))
                    {
                        values[(row, column)] = value;
                        if (row > maxRow) maxRow = row;
                        if (column > maxCol) maxCol = column;
                        isEmpty = false;
                    }
                    else
                    {
                        errorMessages.Add($"({row + 1}, {column + 1}): '{textBox.Text}'. Please, input number");
                    }
                }
            }

            if (isEmpty)
            {
                MessageBox.Show($"Input values please", "Matrix is empty", MessageBoxButton.OK, MessageBoxImage.Error);
                matrix = null;
                return false;
            }

            if (errorMessages.Any())
            {
                string allErrors = string.Join("\n", errorMessages);
                MessageBox.Show($"Incorrect data in cell: \n{allErrors}", "Errors", MessageBoxButton.OK, MessageBoxImage.Error);
                matrix = null;
                return false;
            }

            for (int row = 0; row <= maxRow; row++)
            {
                for (int col = 0; col <= maxCol; col++)
                {
                    if (values.ContainsKey((row, col)) && values[(row, col)] != 0)
                    {
                        if (row > maxRow) maxRow = row;
                        if (col > maxCol) maxCol = col;
                    }
                }
            }

            double[,] matrixData = new double[maxRow + 1, maxCol + 1];

            for (int row = 0; row <= maxRow; row++)
            {
                for (int col = 0; col <= maxCol; col++)
                {
                    if (values.TryGetValue((row, col), out double value))
                    {
                        matrixData[row, col] = value;
                    }
                    else
                    {
                        matrixData[row, col] = 0;
                    }
                }
            }
            UpdateMatrixGrid(grid, matrixData);

            matrix = new Matrix(matrixData);
            return true;
        }

        private void UpdateMatrixGrid(Grid grid, double[,] matrixData)
        {
            for (int row = 0; row < matrixData.GetLength(0); row++)
            {
                for (int col = 0; col < matrixData.GetLength(1); col++)
                {
                    foreach (UIElement element in grid.Children)
                    {
                        if (element is TextBox textBox && Grid.GetRow(textBox) == row && Grid.GetColumn(textBox) == col)
                        {
                            textBox.Text = matrixData[row, col].ToString();
                        }
                    }
                }
            }
        }

        // --------- Calculation Methods --------- //

        private void CalculateDeterminant(Grid matrixGrid)
        {
            try
            {
                if (ReadMatrix(matrixGrid, out Matrix matrix))
                {
                    if (matrix.rows != matrix.cols)
                    {
                        throw new InvalidOperationException("Matrix must be square to calculate the determinant.");
                    }

                    double determinant = matrix.Determinant();
                    DisplayResult("Determinant", determinant.ToString(),false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateInverse(Grid matrixGrid)
        {
            try
            {
                if (ReadMatrix(matrixGrid, out Matrix matrix))
                {
                    if (matrix.rows != matrix.cols)
                    {
                        throw new InvalidOperationException("Matrix must be square to calculate inverse matrix");
                    }

                    Matrix inverseMatrix = matrix.Inverse();
                    DisplayResult($"Inverse matrix: ", MatrixToString(inverseMatrix),true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateRank(Grid matrixGrid)
        {
            try
            {
                if (ReadMatrix(matrixGrid, out Matrix matrix))
                {
                    double rank = matrix.Rank();
                    DisplayResult("Rank", rank.ToString(),false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CalculateTranspose(Grid matrixGrid)
        {
            try
            {

                if (ReadMatrix(matrixGrid, out Matrix matrix))
                {
                    Debug.WriteLine("Matrix Dimensions: " + matrix.rows + " x " + matrix.cols);
                    string matrixInString = MatrixToString(matrix);
                    Debug.WriteLine(matrixInString);
                    Matrix transposedMatrix = matrix.Transpose();
                    DisplayResult("Transposed matrix: ", MatrixToString(transposedMatrix), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    

        private void CalculateREF(Grid matrixGrid)
        {
            try
            {
                if (ReadMatrix(matrixGrid, out Matrix matrix))
                {
                    Matrix inverseMatrix = matrix.GaussianElimination();
                    DisplayResult($"Matrix in REF: ", MatrixToString(inverseMatrix), true);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
            private void CalculateMultiplyByScalar(Grid matrixGrid, int scalar)
            {
                try
                {
                    if (ReadMatrix(matrixGrid, out Matrix matrix))
                    {

                        Matrix multipliedMatrix = matrix.ScalarMultiply(scalar);
                        DisplayResult($"Matrix multiplyed by {scalar}: ", MatrixToString(multipliedMatrix), true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }

        private void CalculatePowerOf(Grid matrixGrid, int scalar)
        {
            try
            {
                if (ReadMatrix(matrixGrid, out Matrix matrix))
                {

                    Matrix multipliedMatrix = matrix.Power(scalar);
                    DisplayResult($"Matrix in power of {scalar}: ", MatrixToString(multipliedMatrix), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // --------- Event  Handlers ---------  // 

        //Matrix Buttons

        private void MultiplyByScalarA_click(object sender, EventArgs e)
        {

            if (int.TryParse(MultiplyByTextBoxA.Text, out int scalar))
            {
                CalculateMultiplyByScalar(MatrixA, scalar);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }
        private void MultiplyByScalarB_click(object sender, EventArgs e)
        {

            if (int.TryParse(MultiplyByTextBoxB.Text, out int scalar))
            {
                CalculateMultiplyByScalar(MatrixB, scalar);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void PowerOfA_click(object sender, EventArgs e)
        {

            if (int.TryParse(PowerOfTextBoxA.Text, out int scalar))
            {
                CalculatePowerOf(MatrixA, scalar);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void PowerOfB_click(object sender, EventArgs e)
        {

            if (int.TryParse(PowerOfTextBoxB.Text, out int scalar))
            {
                CalculatePowerOf(MatrixB, scalar);
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }
        private void REFButtonA_click(object sender, RoutedEventArgs e)
        {
            CalculateREF(MatrixA);
        }
        private void REFButtonB_click(object sender, RoutedEventArgs e)
        {
            CalculateREF(MatrixB);
        }

        private void DeterminantButtonA_Click(object sender, RoutedEventArgs e)
        {
            CalculateDeterminant(MatrixA);
        }

        private void RankButtonA_Click(object sender, RoutedEventArgs e)
        {
            CalculateRank(MatrixA);
        }

        private void RankButtonB_Click(object sender, RoutedEventArgs e)
        {
            CalculateRank(MatrixB);
        }

        private void DeterminantButtonB_Click(object sender, RoutedEventArgs e)
        {
            CalculateDeterminant(MatrixB);
        }

        private void InverseButtonA_Click(object sender, RoutedEventArgs e)
        {
            CalculateInverse(MatrixA);
        }

        private void InverseButtonB_Click(object sender, RoutedEventArgs e)
        {
            CalculateInverse(MatrixB);
        }

        private void TransposeButtonA_Click(object sender, RoutedEventArgs e)
        {
            CalculateTranspose(MatrixA);
        }

        private void TransposeButtonB_Click(object sender, RoutedEventArgs e)
        {
            CalculateTranspose(MatrixB);
        }

        // Two Matrices buttons
        private void MultiplyMatrices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReadMatrix(MatrixA, out Matrix matrixA) && ReadMatrix(MatrixB, out Matrix matrixB))
                {
                    Matrix result = Matrix.TwoMatrixOperations.Multiply(matrixA, matrixB);
                    DisplayResult("A x B", MatrixToString(result), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddMatrices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReadMatrix(MatrixA, out Matrix matrixA) && ReadMatrix(MatrixB, out Matrix matrixB))
                {
                    Matrix result = Matrix.TwoMatrixOperations.MatrixAddition(matrixA, matrixB);
                    DisplayResult("A + B", MatrixToString(result), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubstractMatrices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ReadMatrix(MatrixA, out Matrix matrixA) && ReadMatrix(MatrixB, out Matrix matrixB))
                {
                    Matrix result = Matrix.TwoMatrixOperations.MatrixSubtraction(matrixA, matrixB);
                    DisplayResult("A - B", MatrixToString(result), true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SwapMatrices_Click(object sender, RoutedEventArgs e)
        {
            var matrixATextBoxes = GetTextBoxes(MatrixA);
            var matrixBTextBoxes = GetTextBoxes(MatrixB);

            for (int i = 0; i < matrixATextBoxes.Count; i++)
            {
                var temp = matrixATextBoxes[i].Text;
                matrixATextBoxes[i].Text = matrixBTextBoxes[i].Text;
                matrixBTextBoxes[i].Text = temp;
            }
        }

        //Single matrix scaling buttons
        private void ClearMatrix_Click(object sender, RoutedEventArgs e)
        {
            Button clearButton = sender as Button;
            Grid matrixGrid = clearButton.Tag as Grid;

            if (matrixGrid != null)
            {
                foreach (var child in matrixGrid.Children)
                {
                    if (child is TextBox textBox)
                    {
                        textBox.Text = string.Empty;
                    }
                }
            }
        }

        private void AddDimension_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Grid grid)
            {
                AddDimension(grid);
                UpdateButtonStates(grid);
            }
        }

        private void RemoveDimension_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Grid grid)
            {
                RemoveDimension(grid);
                UpdateButtonStates(grid);
            }
        }

        // --------- Matrix Dimension Management --------- //
        private void AddDimension(Grid grid)
        {
            if (grid.RowDefinitions.Count >= 10 || grid.ColumnDefinitions.Count >= 10)
                return;

            var newRow = new RowDefinition { Height = GridLength.Auto };
            var newCol = new ColumnDefinition { Width = GridLength.Auto };

            grid.RowDefinitions.Add(newRow);
            grid.ColumnDefinitions.Add(newCol);

            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var textBox = new TextBox
                {
                    Width = 50,
                    Height = 25,
                    Margin = new Thickness(5),
                    Style = (Style)FindResource("RoundedTextBoxStyle")
                };
                grid.Children.Add(textBox);
                Grid.SetRow(textBox, i);
                Grid.SetColumn(textBox, grid.ColumnDefinitions.Count - 1);
            }

            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var textBox = new TextBox
                {
                    Width = 50,
                    Height = 25,
                    Margin = new Thickness(5),
                    Style = (Style)FindResource("RoundedTextBoxStyle")
                };
                grid.Children.Add(textBox);
                Grid.SetRow(textBox, grid.RowDefinitions.Count - 1);
                Grid.SetColumn(textBox, i);
            }
        }

        private void RemoveDimension(Grid grid)
        {
            if (grid.RowDefinitions.Count <= 1 || grid.ColumnDefinitions.Count <= 1)
                return;

            int lastRowIndex = grid.RowDefinitions.Count - 1;
            int lastColIndex = grid.ColumnDefinitions.Count - 1;

            for (int i = grid.Children.Count - 1; i >= 0; i--)
            {
                var child = grid.Children[i] as UIElement;
                if (child != null && (Grid.GetRow(child) == lastRowIndex || Grid.GetColumn(child) == lastColIndex))
                {
                    grid.Children.RemoveAt(i);
                }
            }

            grid.RowDefinitions.RemoveAt(lastRowIndex);
            grid.ColumnDefinitions.RemoveAt(lastColIndex);
        }

        private void UpdateButtonStates(Grid grid)
        {
            bool canAdd = grid.RowDefinitions.Count < 10;
            bool canRemove = grid.RowDefinitions.Count > 1;

            if (grid.Name == "MatrixA")
            {
                AddButton_A.IsEnabled = canAdd;
                RemoveButton_A.IsEnabled = canRemove;
            }
            else if (grid.Name == "MatrixB")
            {
                AddButton_B.IsEnabled = canAdd;
                RemoveButton_B.IsEnabled = canRemove;
            }
        }
        private void ClearResults_Click(object sender, RoutedEventArgs e)
        {
            ResultsStackPanel.Children.Clear();
        }

    }
}