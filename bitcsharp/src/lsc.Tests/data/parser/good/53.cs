class MainClass
{
    public static void Main()
    {
        int i = 0;
        int y = --i;
        y = --y + i++;
        y = ++y + i--;
    }
}
