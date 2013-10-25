namespace LLVMSharp
{
    class A
    {
        void method()
        {
            int big_a; 

            for (int i = 0; i < 10; i++)
            {
                int big_a; 
                big_a = 1;
                
            }
            if (true)
            {
                int small_a;
            }

            int int_a;
            string str_a;
            bool bool_a;

            //Type matched
            int_a = int_a + int_a + 1;
            str_a = str_a + "string";
            bool_a = true;
            

            //Type mismatched
            int_a = "string";
            int_a = 1 + "string";
            str_a = 1 + 2;
            bool_a = 1;
            int_a = bool_a;
        }

    }
}
