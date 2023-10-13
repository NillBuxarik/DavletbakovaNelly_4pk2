using System;
using System.Threading;

public class Database
{
    public void SaveData(string text)
    {
        //блокирует Savedata для других потоков, если Savedata уже используется другим потоком
        //lock (this)
        //{
            Console.WriteLine("Database.SaveData - начатый");
            Console.WriteLine("Database.SaveData - работающий");
            Console.WriteLine(text);
            Console.WriteLine("Database.SaveData - законченный");
        //}
    }
}

class Program
{
    public static Database db = new Database();

    public static void WorkerThreadMethod1()
    {
        Console.WriteLine("рабочий поток #1 - запущен");
        Console.WriteLine("рабочий поток #1 - использует Database.Savedata");
        db.SaveData("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        Console.WriteLine("рабочий поток #1 - возвращен из выходных данных");
    }

    public static void WorkerThreadMethod2()
    {
        Console.WriteLine("рабочий поток #2 - запущен");
        Console.WriteLine("рабочий поток #2 - использует Database.Savedata");
        db.SaveData("YYYYYYYYYYYYYYYYYYYYYYYYYYYYYY");
        Console.WriteLine("рабочий поток #2 - возвращен из выходных данных");
    }

    static void Main()
    {
        Console.WriteLine("main - создание рабочих потоков");

        Thread t1 = new Thread(new ThreadStart(WorkerThreadMethod1));
        Thread t2 = new Thread(new ThreadStart(WorkerThreadMethod2));

        t1.Start();
        t2.Start();
        //ожидает завершения потоков
        //t1.Join();
        //t2.Join();
    }
}
