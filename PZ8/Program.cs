using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr HeapCreate(uint flOptions, UIntPtr dwInitialSize, UIntPtr dwMaximumSize);

    static void Main()
    {
        IntPtr hHeap = HeapCreate(0x00040000, 900000, 900860);

        //Создаём потоки
        Thread thread1 = new Thread(SlipTask);
        Thread thread2 = new Thread(SlipATask);
        Thread thread3 = new Thread(SlipAAATask);
        thread1.Start();
        thread2.Start();
        thread3.Start();
        Console.ReadLine();
    }

    static void SlipTask()
    {
        while (true)
        {
            Console.WriteLine("ХОЧУ");
                
        }
    }
    static void SlipATask()
    {
        while (true)
        {
            Console.WriteLine("СПАТЬ Zzz");

        }
    }
    static void SlipAAATask()
    {
        while (true)
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

        }
    }
}





