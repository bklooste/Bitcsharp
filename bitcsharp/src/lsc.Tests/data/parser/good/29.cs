class SomeType
{

    public static implicit operator int(SomeType typ)
    {
        // code to convert from SomeType  to int
        return 0;
    }


    public static explicit operator SomeType(int i)
    {
        // code to convert from int to SomeType
        return null;
    }
}