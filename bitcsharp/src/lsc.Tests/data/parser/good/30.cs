using System;

struct Complex
{
    float real;
    float imaginary;

    public Complex(float real, float imaginary)
    {
        this.real = real;
        this.imaginary = imaginary;
    }

    public float Real
    {
        get
        {
            return (real);
        }
        set
        {
            real = value;
        }
    }

    public float Imaginary
    {
        get
        {
            return (imaginary);
        }
        set
        {
            imaginary = value;
        }
    }

    public override string ToString()
    {
        return (String.Format("({0}, {1}i)", real, imaginary));
    }

    public static bool operator ==(Complex c1, Complex c2)
    {
        if ((c1.real == c2.real) && (c1.imaginary == c2.imaginary))
            return (true);
        else
            return (false);
    }

    public static bool operator !=(Complex c1, Complex c2)
    {
        return (!(c1 == c2));
    }

    public override bool Equals(object o2)
    {
    }

    public override int GetHashCode()
    {
    }

    public static Complex operator +(Complex c1, Complex c2)
    {
    }

    public static Complex operator -(Complex c1, Complex c2)
    {
    }

    public static Complex operator *(Complex c1, Complex c2)
    {
    }

    public static Complex operator /(Complex c1, Complex c2)
    {
    }

}

struct MyType
{
    public MyType(int value)
    {
        this.value = value;
    }
    public override string ToString()
    {
        return(value.ToString());
    }
    public static MyType operator -(MyType roman)
    {
        return(new MyType(-roman.value));
    }
    public static MyType operator +( MyType roman1, MyType roman2)
    {
        return(new MyType(roman1.value + roman2.value));
    }
    
    public static MyType operator ++(MyType roman)
    {
        return(new MyType(roman.value + 1));
    }
    int value;
}