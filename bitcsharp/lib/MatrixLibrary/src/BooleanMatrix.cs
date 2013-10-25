using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DotNetMatrix
{
    public class BooleanMatrix
    {
        private bool[,] values;

        public int Width
        {
            get { return values.GetLength(1); }
        }

        public int Height
        {
            get { return values.GetLength(0); }
        }

        public BooleanMatrix(int height, int width)
        {
            values = new bool[height, width];
        }

        //--Closures
        public BooleanMatrix ReflexiveClosure()
        {
            BooleanMatrix closure = this.Clone();

            //Fill in elements with equal width and height
            for (int x = 0; x < closure.Width; x++)
            {
                for (int y = 0; y < closure.Height; y++)
                {
                    if (x == y)
                        closure[y, x] = true;
                }
            }

            return closure;
        }

        public BooleanMatrix SymmetricClosure()
        {
            BooleanMatrix closure = this.Clone();

            for (int x = 0; x < closure.Width; x++)
            {
                for (int y = 0; y < closure.Height; y++)
                {
                    if (x != y && closure[y, x])
                        closure[x, y] = true;
                }
            }

            return closure;
        }

        public BooleanMatrix TransitiveClosure()
        {
            BooleanMatrix closure = this.Clone();

            //Warshall's Algorithm
            for (int n = 0; n < closure.Height; n++)
            {
                for (int x = 0; x < closure.Width; x++)
                {
                    for (int y = 0; y < closure.Height; y++)
                    {
                        if (!closure[y, x])
                            closure[y, x] = closure[y, n] & closure[n, x];
                    }
                }
            }

            return closure;
        }

        //Creates a copy of this matrix
        public BooleanMatrix Clone()
        {
            BooleanMatrix cloneMatrix = new BooleanMatrix(this.Height, this.Width);

            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    cloneMatrix[y, x] = this[y, x];
                }
            }

            return cloneMatrix;
        }

        //Override indexer
        public bool this[int height, int width]
        {
            get
            {
                return values[height, width];
            }
            set
            {
                values[height, width] = value;
            }
        }
        

        
        //--Operations
      /*  public BooleanMatrix Add(BooleanMatrix b)
        {
            BooleanMatrix c = new BooleanMatrix(b.Height, b.Width);
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {
                    c[i, j] = (b[i, j] | this[i, j]);
                }
            }
            return c;
        }
        */
        public BooleanMatrix Meet(BooleanMatrix m2)
        {
            if (this.Height == m2.Height && this.Width == m2.Width)
            {
                BooleanMatrix newMatrix = new BooleanMatrix(this.Height, this.Width);
                for (int x = 0; x < this.Width; x++)
                {
                    for (int y = 0; y < this.Height; y++)
                    {
                        newMatrix[y, x] = this[y, x] & m2[y, x];
                    }
                }
                return newMatrix;
            }
            else
                throw new Exception("Invalid matrices");
        }

        public BooleanMatrix Meet(bool value)
        {
            BooleanMatrix newMatrix = new BooleanMatrix(this.Height, this.Width);
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    newMatrix[y, x] = this[y, x] & value;
                }
            }
            return newMatrix;
        }

        public BooleanMatrix Join(BooleanMatrix m2)
        {
            if (this.Height == m2.Height && this.Width == m2.Width)
            {
                BooleanMatrix newMatrix = new BooleanMatrix(this.Height, this.Width);
                for (int x = 0; x < this.Width; x++)
                {
                    for (int y = 0; y < this.Height; y++)
                    {
                        newMatrix[y, x] = this[y, x] | m2[y, x]; //or = +
                    }
                }
                return newMatrix;
            }
            else
                throw new Exception("Invalid matrices");
        }

        public BooleanMatrix Join(bool value)
        {
            BooleanMatrix newMatrix = new BooleanMatrix(this.Height, this.Width);
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    newMatrix[y, x] = this[y, x] | value;
                }
            }
            return newMatrix;
        }
        
        public BooleanMatrix Product(BooleanMatrix m2)
        {
            if (this.Width == m2.Height)
            {
                BooleanMatrix newMatrix = new BooleanMatrix(this.Height, m2.Width);
                
                for (int j = 0; j < this.Height; j++) //up-down
                {
                    for (int z = 0; z < m2.Width; z++) //left-right top matrix
                    {
                        for (int i = 0; i < this.Width; i++) //left-right
                        {
                            bool value1 = this[j, i];
                            bool value2 = m2[i, z];

                            newMatrix[j, z] |= (value1 & value2);
                        }
                    }                    
                }

                return newMatrix;
            }
            else
                throw new Exception("Invalid matrices");
        }

        //--Transformations
        public BooleanMatrix Transpose()
        {
            BooleanMatrix newMatrix = new BooleanMatrix(this.Width, this.Height);

            for (int i = 0; i < this.Width; i++)
            {
                for (int j = 0; j < this.Height; j++)
                {
                    newMatrix[i, j] = this[j, i];
                }
            }

            return newMatrix;
        }

        public override string ToString()
        {
            string finalStr = string.Empty;

            for (int x = 0; x < values.GetLength(0); x++)
            {
                finalStr += "[ ";
                for (int y = 0; y < values.GetLength(1); y++)
                {
                    finalStr += values[x, y] ? "1" : "0";

                    if (y != values.GetLength(1) - 1)
                        finalStr += ", ";
                }
                finalStr += "]";

                if (x != values.GetLength(0) - 1)
                    finalStr += "\n";
            }

            return finalStr;
        }

        public static BooleanMatrix IdentityMatrix(int height, int width)
        {
            BooleanMatrix newMatrix = new BooleanMatrix(height, width);
            for (int i = 0; i < newMatrix.Width; i++)
            {
                for (int j = 0; j < newMatrix.Height; j++)
                {
                    if (i == j)
                        newMatrix[j, i] = true;
                }
            }
            return newMatrix;
        }
    }
}
