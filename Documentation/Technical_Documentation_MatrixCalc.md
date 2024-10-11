
# Developer documentation of Matrix Calculator by Oleksii Horovych
		
  ##  MatrixCalcUI (Project for procesing UI)

### Constructor (`MainWindow()`)

  

Initializes the main window of the application, sets up UI components, and updates button states for matrices A and B.

  

### Helper Methods

  

#### `GetTextBoxes(Grid matrixGrid)`

  

Returns a list of all TextBox controls of passed grid.

  

#### `MatrixToString(Matrix matrix)`

  

Converts a Matrix object into a formatted string representation suitable for display.

  

#### `DisplayResult(string nameOfOperation, string result, bool isMatrix)`

  

Displays the result of a matrix operation in a structured format within the UI.

  

#### `PasteToMatrix(Grid targetMatrix)`

  

Pastes a matrix from the result display back into either Matrix A or Matrix B.

  

#### `ReadMatrix(Grid grid, out Matrix matrix)`

  

Reads the values from TextBoxes and constructs a Matrix object from these values.

  

#### `UpdateMatrixGrid(Grid grid, double[,] matrixData)`

  

Updates the TextBox controls with values from a 2D array (`matrixData`).

  

### Calculation Methods

  

#### `CalculateDeterminant(Grid matrixGrid)`

  

Calculates and displays the determinant of the matrix in passed grid.

  

#### `CalculateInverse(Grid matrixGrid)`

  

Calculates and displays the inverse of the matrix in passed grid.

  

#### `CalculateRank(Grid matrixGrid)`

  

Calculates and displays the rank of the matrix in passed grid.

  

#### `CalculateTranspose(Grid matrixGrid)`

  

Calculates and displays the transpose of the matrix in passed grid.

  

#### `CalculateREF(Grid matrixGrid)`

  

Calculates and displays the matrix in Row Echelon Form (REF) using Gaussian elimination in passed grid.

  

#### `CalculateMultiplyByScalar(Grid matrixGrid, int scalar)`

  

Multiplies the matrix in passed grid (matrixGrid) by a scalar integer (scalar) and displays the result.

  

#### `CalculatePowerOf(Grid matrixGrid, int scalar)`

  

Raises the matrix in the specified Grid (`matrixGrid`) to the power of an integer (`scalar`) and displays the result.

  
  
  

### Event Handlers (Button Clicks)

  

#### **Matrix Operations**

  

##### `MultiplyMatrices_Click(object sender, RoutedEventArgs e)`

  

Reads matrices from Grid A and Grid B, multiplies them, and displays the result.

  

##### `AddMatrices_Click(object sender, RoutedEventArgs e)`

  

Reads matrices from Grid A and Grid B, adds them together, and displays the result.

  

##### `SubstractMatrices_Click(object sender, RoutedEventArgs e)`

  

Reads matrices from Grid A and Grid B, subtracts Grid B from Grid A, and displays the result.

  

##### `SwapMatrices_Click(object sender, RoutedEventArgs e)`

  

Swaps the contents of Matrix A with Matrix B.

  

#### **Single Matrix Operations**

  

##### `MultiplyByScalarA_click(object sender, EventArgs e)`

  

Reads the scalar value from the input field and multiplies the matrix in Grid A by this scalar, then displays the result.

  

##### `MultiplyByScalarB_click(object sender, EventArgs e)`

  

Reads the scalar value from the input field and multiplies the matrix in Grid B by this scalar, then displays the result.

  

##### `PowerOfA_click(object sender, EventArgs e)`

  

eads the power value from the input field and raises the matrix in Grid A to this power, then displays the result.

  

##### `PowerOfB_click(object sender, EventArgs e)`

  

Reads the power value from the input field and raises the matrix in Grid B to this power, then displays the result.

  

##### `REFButtonA_click(object sender, RoutedEventArgs e)`

  

onverts the matrix in Grid A to Row Echelon Form using Gaussian elimination and displays the result.

  

##### `REFButtonB_click(object sender, RoutedEventArgs e)`

  

Converts the matrix in Grid B to Row Echelon Form using Gaussian elimination and displays the result.

  

##### `DeterminantButtonA_Click(object sender, RoutedEventArgs e)`

  

Calculates the determinant of the matrix in Grid A and displays the result.

  

##### `DeterminantButtonB_Click(object sender, RoutedEventArgs e)`

  

Calculates the determinant of the matrix in Grid B and displays the result.

  

##### `RankButtonA_Click(object sender, RoutedEventArgs e)`

  

Calculates the rank of the matrix in Grid A and displays the result.

  

##### `RankButtonB_Click(object sender, RoutedEventArgs e)`

  

Calculates the rank of the matrix in Grid B and displays the result.

  

##### `InverseButtonA_Click(object sender, RoutedEventArgs e)`

  

Calculates the inverse of the matrix in Grid A and displays the result.

  

##### `InverseButtonB_Click(object sender, RoutedEventArgs e)`

  

Calculates the inverse of the matrix in Grid B and displays the result.

  

##### `TransposeButtonA_Click(object sender, RoutedEventArgs e)`

  

Calculates the transpose of the matrix in Grid A and displays the result.

  

##### `TransposeButtonB_Click(object sender, RoutedEventArgs e)`

  

Calculates the transpose of the matrix in Grid B and displays the result.

  

#### **Matrix Dimension Management**


##### `ClearMatrix_Click(object sender, RoutedEventArgs e)`

  

Clears the contents of the matrix specified by the button's Tag (either Matrix A or Matrix B).

  

##### `AddDimension_Click(object sender, RoutedEventArgs e)`

  

Adds a new row and column to the matrix specified by the button's Tag (either Matrix A or Matrix B).

  

##### `RemoveDimension_Click(object sender, RoutedEventArgs e)`

  

Removes the last row and column from the matrix specified by the button's Tag (either Matrix A or Matrix B).

  

##### `ClearResults_Click(object sender, RoutedEventArgs e)`

  

Clears all results displayed in the ResultsStackPanel.


## MatrixCalc Library (For Calculations)

### Matrix Class

The **Matrix** class provides functionalities for performing matrix operations

#### Properties

-   **_matrix**: A 2D array representing the matrix values.
-   **rows**: The number of rows in the matrix.
-   **cols**: The number of columns in the matrix.

#### Indexer

-   **this[int row, int col]**: Accesses the matrix element at the specified row and column.

#### Constructors

-   **Matrix(double[,] matrix)**
    
    Initializes a new `Matrix` instance with the provided 2D array.
    

#### Methods

-   **`Matrix Power(int power)`**
    
    Raises the matrix to the given power.
    Returns calculated Matrix
    

-   **`Matrix Transpose()`**
    
    Computes the transpose of the matrix.
    Returns  transposed matrix.
  
-   **`int Rank()`**
    
    Computes the rank of the matrix using Gaussian elimination.
   
    Returns  rank in integer
-   **`Matrix Inverse()`**
    
    Calculates the inverse of the matrix.
	Returns  transposed matrix.
-   **`Matrix ScalarMultiply(double scalar)`**
    
    Multiplies every element of the matrix by a scalar value.
    Returns multiplyed matrix.
-   **`double Determinant()`**
    
    Computes the determinant of the matrix.
    Returns (double) determinant of the matrix.
-   **`Matrix GaussianElimination()`**
    
    Applies Gaussian elimination to convert the matrix to Row Echelon Form (REF).
    Returns matrix in REF.

#### Static Class TwoMatrixOperations

**`TwoMatrixOperations`** Class : Contains static methods for matrix operations involving two matrices.

-   **`Matrix Multiply(Matrix matrix1, Matrix matrix2)`**
    
    Multiplies two matrices.
    
    -   Returns the product of the two matrices.
-   **`Matrix MatrixAddition(Matrix matrix1, Matrix matrix2)`**
    
    Adds two matrices.
    Returns the result of adding the two matrices.

-   **`Matrix MatrixSubtraction(Matrix matrix1, Matrix matrix2)`**
    
    Subtracts one matrix from another.
   Returns The result of subtracting the second matrix from the first.
