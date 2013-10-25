class A
{
    class B
    {
        public int i;
    }
    public B x;
    int j;
}
class Test58
{
    public static void Main()
    {
       //A.i=1;
        A obj;
        obj.x.i = 1;
        obj.j = 2;
    }
}
