using System.Runtime.InteropServices;
using System;
public class Program
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;

        public MEMORYSTATUSEX()
        {
            dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    public static void Main()
    {
        MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();

        if (GlobalMemoryStatusEx(memStatus))
        {
            Console.WriteLine($"Объем физической памяти: {memStatus.ullTotalPhys / 1024 / 1024} Mегабайт");
            Console.WriteLine($"Доступно физической памяти: {memStatus.ullAvailPhys / 1024 / 1024} Mегабайт");
            Console.WriteLine($"Объем файла подкачки: {memStatus.ullTotalPageFile / 1024 / 1024} Mегабайт");
            Console.WriteLine($"Доступно файла подкачки: {memStatus.ullAvailPageFile / 1024 / 1024} Mегабайт");
            Console.WriteLine($"Всего виртуальной памяти: {memStatus.ullTotalVirtual / 1024 / 1024 / 1024} Гигабайт");
            Console.WriteLine($"Доступно виртуальной памяти: {memStatus.ullAvailVirtual / 1024 / 1024 / 1024} Гигабайт");
            Console.WriteLine("Используемая память процессами: {0}%", memStatus.dwMemoryLoad);
        }
        else
        {
            Console.WriteLine("Не удалось получить информацию о памяти.");
        }
    }
}