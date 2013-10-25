namespace lsc.Tests.data.semantic.symbolTable.bad._4
{

    class AA
    {

        string x;
        public int MyNum
        {
            get { return x; }
        }
    }


    class _4
    {
        private int a;

        void method()
        {
            if (true)
            {
                _4 obj4 = new _4();
            
            ;

                obj4.a = 1;
                obj4.method();
            }
            else
            {
                _4 obj4 = new _4();

                obj4.a = 1;
                obj4.method();
            }

            a = 2;
        }
        void method2()
        {
            int obj4;
            obj4 = 1;
        }
    }
    class A
    {
        void method() {
            _4 obj4 = new _4();
            obj4.a = 1;
            obj4.method();
        }
    }
}
