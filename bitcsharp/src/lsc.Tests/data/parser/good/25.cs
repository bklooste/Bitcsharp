class hello
{
    const int x=2;
    const int y=1, z=3;

    public const int x=2;
    private const int y=2, z=2;
    const string hi = "hi Prabir";

    public extern const char x='a';
    protected const hello y=new hello();
    protected const hello.w x=new hello.w(), y=new hello.w(), z=new hello.w();
    public const hello.world x=new hello.world(), y=new hello.world(), z=new hello.world();
    private const hello x=y, y=hello, z=hello;
}