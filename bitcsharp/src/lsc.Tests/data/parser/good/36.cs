class DoLoop
{
    void Hello()
    {
        do
        {
            if (true)
            {
                int a = 1;
            }
            else
            {
                int b = 2,
                    c = 2,
                    d = c;
                break;
            }
        } while (true);
        do
        {
            if (true)
            {
                int a = 1;
            }
            break;
        } while (a == 1 && true);
    }
}