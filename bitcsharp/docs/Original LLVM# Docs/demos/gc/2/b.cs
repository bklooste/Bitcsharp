// use heap size as 175

using System;

class Student
{
    public int ID;
}

class Node
{
    public Student Student;
    public Node Next;

}

class A
{
    public static void TestGC()
    {
        Student s2 = new Student();
    }

    public static void Main()
    {
        Node n = new Node();

        TestGC();

        Student s1 = new Student();

    }

}