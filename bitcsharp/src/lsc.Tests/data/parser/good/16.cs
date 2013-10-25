public class helloworld
{
    helloworld() { }
    public helloworld() { }
    private helloworld() { }
    protected helloworld() { }

    helloworld() : this() { }
    helloworld() : base() { }
}