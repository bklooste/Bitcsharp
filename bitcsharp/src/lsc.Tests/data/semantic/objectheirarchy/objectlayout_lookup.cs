// Lookup in object layout
class A
{
    int a;
    int b;

    protected int c;
    public int d;
}

class B : A
{
}

class C : A
{
    protected int e;
    public int f;
}

class D : C
{
    protected int g;
    public int h;
}