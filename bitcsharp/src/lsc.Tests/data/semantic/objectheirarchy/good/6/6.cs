class MainClass
{
    public static int Main()
    {
        MainClass m = new MainClass();
        MainClass m2 = new MainClass("hello", "world", 3);
    }

    MainClass() { }
    MainClass(string a, string b, int c) { }
}
