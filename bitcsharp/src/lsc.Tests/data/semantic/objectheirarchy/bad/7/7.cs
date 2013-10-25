
//namespace llvm
//{  
//    public class Hello
//    {
//        private static string filename;

//        public virtual string FName
//        {
//            get;
//            set;
//        }
//        //World()
//        //{  }
//        public virtual void WEEHOO()
//        { }
//        public virtual void CheckThis()
//        {
//            //World a = new World();
//        }
//    }
//   public class World:Hello
//    {
//       enum colors { red, blue, green, black };
//       int x;
//         World(int x):base()
//        { }
//         World()
//         {
//            //base;
//         }
//         public virtual int MyVInt
//         {
//             get { return x; }
//             set { x = value; }
//         }
//         public override void WEEHOO(string [] x)
//         {

//         }
//        // public extern void No_Body();
//        // public static implicit operator Hello(Universe h)
//        // {
//        //     return null;
//        // }
//        // public static World operator +(Hello x, Universe y)
//        // {
//        //     return null;
//        // }
//        //public static Universe operator +(World x)
//        //{
//        //    return null;
//        //}
//        //public static Hello operator ++(Universe x)
//        //{
//        //    return null;
//        //}
//        //public extern void function1()
//        //{}
//       //public static override void FunctionX()
//       //{}
       
       
//    }
//    struct Box
//    {
//        struct Item { }
//        struct Something { }
//        //public virtual int x;
//        public string a;
//        //int z = 10;
//      //  public Box(int x)
//       //     : base()
//        //{ }       
//       static Box()
//        { }
//       protected void AA()
//       { }
//        public static explicit operator Case(Box a)
//        {
//            int x;
//           // x = new int[2+5] {4+2,5+1,0, 0,0,0,0 };
//           // int[] x = new int[] { }; 
//            return new Case();
//        }
//        public static implicit operator Box(Case a)
//        {
//            return new Box();
//        }
//        //public virtual void Function()
//        //{ }
//        public override void Something()
//        {
           
//        }
//    }
//    struct Case
//    {
//        protected int x; 
//    }

//     public class Universe
//    {
       
         
//        //int Universe;
//         public static implicit operator Universe(Hello h)
//         {
//             return null;
//         }

//        //static Universe(int a, int b)
//        //{ }
//        //public static override void function()
//        //{ }
//       //public override void fun()
//       // {

//       // }
//        // private internal int ReturnInt()
//        //{
//        //    return 1;
//        //}
        
//         //public override void CheckThis()
//         //{ }
        
//    }
  
//}
class A
{  

    public void b(A abc)
    {
        int x = 0;
        //int y =x;
        x+=1+25;
        A a = new A();
        //string a= new string('a',1)+5;
      //b(a);
    }
    public static bool operator true(A a)
    {
        return false;
    }
  public static bool operator true(A a)
  {
      return false;
  }
  public static bool operator ==(A a,A b)
  {
      return false;
  }
      public static A operator >(A a , X x)
      {
          return new A();
      }
      public static A operator <(A a, X x)
      {
          return new A();
      }
      public static A operator +(A a, X x)
      {
          return new A();
      }
    public static int Main()
    { return 0; }
}
class X:A
{}
struct B
{
   //  virtual int x;
  
   //extern B myself;
   //static string something;
   // override char a;
    public static void Main(string[] args)
    { }
}
 