using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    const int TH32CS_SNAPMODULE = 0x00000008;
    const int TH32CS_SNAPPROCESS = 0x00000002;

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

    [DllImport("kernel32.dll")]
    public static extern bool Module32First(IntPtr hSnapshot, ref ModuleEntry32 lpme);

    [DllImport("kernel32.dll")]
    public static extern bool Module32Next(IntPtr hSnapshot, ref ModuleEntry32 lpme);

    [DllImport("kernel32.dll")]
    public static extern bool CloseHandle(IntPtr hSnapshot);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool Process32First(IntPtr hSnapshot, ref ProcessEntry32 lppe);

    [DllImport("kernel32.dll")]
    public static extern bool Process32Next(IntPtr hSnapshot, ref ProcessEntry32 lppe);

    [StructLayout(LayoutKind.Sequential)]
    public struct ModuleEntry32
    {
        public uint dwSize;
        public uint th32ModuleID;
        public uint th32ProcessID;
        public uint GlblcntUsage;
        public uint ProccntUsage;
        public IntPtr modBaseAddr;
        public uint modBaseSize;
        public IntPtr hModule;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szModule;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessEntry32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szExeFile;
    }

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine(
                "1: Вывести все\n" +
                "2: По имени\n" +
                "3: По полному имени\n" +
                "4: По дескриптору\n" +
                "5: Информация о процессе\n" +
                "6: Информация о всех запущенных процессах, потоках, модулях\n"
            );

            string input = Console.ReadLine();

            if (int.TryParse(input, out int pressedKey))
            {
                switch (pressedKey)
                {
                    case 1: // Вывести все
                        DisplayAll();
                        break;
                    case 2: // По имени
                        Console.WriteLine("Введите имя модуля:");
                        string moduleName = Console.ReadLine();
                        DisplayModuleInfoByName(moduleName);
                        break;
                    case 3: // По полному имени
                        Console.WriteLine("Введите полное имя модуля:");
                        string moduleFullPath = Console.ReadLine();
                        DisplayModuleInfoByFullPath(moduleFullPath);
                        break;
                    case 4: // По дескриптору \  \
                        Console.WriteLine("Введите дескриптор модуля:");
                        IntPtr moduleHandle;
                        if (IntPtr.TryParse(Console.ReadLine(), out moduleHandle))
                        {
                            DisplayModuleInfoByHandle(moduleHandle);
                        }
                        else
                        {
                            Console.WriteLine("Неверный формат дескриптора.");
                        }
                        break;
                    case 5: // Информация о процессе
                        DisplayProcessInfo();
                        break;
                    case 6: // Информация о всех запущенных процессах, потоках, модулях
                        DisplayAllProcessesInfo();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ввода.");
            }
        }
    }

    static void DisplayModuleInfoByName(string moduleName)
    {
        IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, 0);
        if (hSnapshot == IntPtr.Zero)
        {
            Console.WriteLine("Error creating snapshot.");
            return;
        }

        Process currentProcess = Process.GetCurrentProcess();
        IntPtr currentProcessHandle = OpenProcess(0x0400 | 0x0010, false, currentProcess.Id);

        ModuleEntry32 moduleEntry = new ModuleEntry32();
        moduleEntry.dwSize = (uint)Marshal.SizeOf(typeof(ModuleEntry32));

        if (Module32First(hSnapshot, ref moduleEntry))
        {
            do
            {
                if (moduleEntry.szModule == moduleName)
                {
                    Console.WriteLine($"Имя: {moduleEntry.szModule}");
                    Console.WriteLine($"Полное имя: {moduleEntry.szExePath}");
                    Console.WriteLine($"Дескриптор: {moduleEntry.hModule}");
                    Console.WriteLine();
                }
            } while (Module32Next(hSnapshot, ref moduleEntry));
        }

        CloseHandle(hSnapshot);
        CloseHandle(currentProcessHandle);
    }

    static void DisplayModuleInfoByFullPath(string moduleFullPath)
    {
        IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, 0);
        if (hSnapshot == IntPtr.Zero)
        {
            Console.WriteLine("Error creating snapshot.");
            return;
        }

        Process currentProcess = Process.GetCurrentProcess();
        IntPtr currentProcessHandle = OpenProcess(0x0400 | 0x0010, false, currentProcess.Id);

        ModuleEntry32 moduleEntry = new ModuleEntry32();
        moduleEntry.dwSize = (uint)Marshal.SizeOf(typeof(ModuleEntry32));

        if (Module32First(hSnapshot, ref moduleEntry))
        {
            do
            {
                if (moduleEntry.szExePath == moduleFullPath)
                {
                    Console.WriteLine($"Имя: {moduleEntry.szModule}");
                    Console.WriteLine($"Полное имя: {moduleEntry.szExePath}");
                    Console.WriteLine($"Дескриптор: {moduleEntry.hModule}");
                    Console.WriteLine();
                }
            } while (Module32Next(hSnapshot, ref moduleEntry));
        }

        CloseHandle(hSnapshot);
        CloseHandle(currentProcessHandle);
    }

    static void DisplayModuleInfoByHandle(IntPtr moduleHandle)
    {
        IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, 0);
        if (hSnapshot == IntPtr.Zero)
        {
            Console.WriteLine("Error creating snapshot.");
            return;
        }

        ModuleEntry32 moduleEntry = new ModuleEntry32();
        moduleEntry.dwSize = (uint)Marshal.SizeOf(typeof(ModuleEntry32));

        if (Module32First(hSnapshot, ref moduleEntry))
        {
            do
            {
                if (moduleEntry.hModule == moduleHandle)
                {
                    Console.WriteLine($"Имя: {moduleEntry.szModule}");
                    Console.WriteLine($"Полное имя: {moduleEntry.szExePath}");
                    Console.WriteLine($"Дескриптор: {moduleEntry.hModule}");
                    Console.WriteLine();
                    break;
                }
            } while (Module32Next(hSnapshot, ref moduleEntry));
        }

        CloseHandle(hSnapshot);
    }

    static void DisplayProcessInfo()
    {
        Process currentProcess = Process.GetCurrentProcess();

        Console.WriteLine($"Имя процесса: {currentProcess.ProcessName}");
        Console.WriteLine($"ID процесса: {currentProcess.Id}");
    }

    static void DisplayAllProcessesInfo()
    {
        IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
        if (hSnapshot == IntPtr.Zero)
        {
            Console.WriteLine("Error creating snapshot.");
            return;
        }

        ProcessEntry32 processEntry = new ProcessEntry32();
        processEntry.dwSize = (uint)Marshal.SizeOf(typeof(ProcessEntry32));

        if (Process32First(hSnapshot, ref processEntry))
        {
            do
            {
                Console.WriteLine($"Имя процесса: {processEntry.szExeFile}");
                Console.WriteLine($"ID процесса: {processEntry.th32ProcessID}");
                Console.WriteLine();
            } while (Process32Next(hSnapshot, ref processEntry));
        }

        CloseHandle(hSnapshot);
    }

    static void DisplayAll()
    {
        IntPtr hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, 0);
        if (hSnapshot == IntPtr.Zero)
        {
            Console.WriteLine("Error creating snapshot.");
            return;
        }

        Process currentProcess = Process.GetCurrentProcess();
        IntPtr currentProcessHandle = OpenProcess(0x0400 | 0x0010, false, currentProcess.Id);

        ModuleEntry32 moduleEntry = new ModuleEntry32();
        moduleEntry.dwSize = (uint)Marshal.SizeOf(typeof(ModuleEntry32));

        if (Module32First(hSnapshot, ref moduleEntry))
        {
            Console.WriteLine($"Имя: {moduleEntry.szModule}");
            Console.WriteLine($"Полное имя: {moduleEntry.szExePath}");
            Console.WriteLine($"Дескриптор: {moduleEntry.hModule}");
            Console.WriteLine();
            do
            {
                Console.WriteLine($"Имя: {moduleEntry.szModule}");
                Console.WriteLine($"Полное имя: {moduleEntry.szExePath}");
                Console.WriteLine($"Дескриптор: {moduleEntry.hModule}");
                Console.WriteLine();
            } while (Module32Next(hSnapshot, ref moduleEntry));
        }

        CloseHandle(hSnapshot);
        CloseHandle(currentProcessHandle);
    }
}
