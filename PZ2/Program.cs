using System;
using System.Runtime.InteropServices;
using System.Text;

class Program
{
    [DllImport("kernel32.dll")]
    static extern uint GetSystemDirectory([Out] StringBuilder lpBuffer, uint uSize);

    [DllImport("Kernel32", CharSet = CharSet.Auto)]
    static extern bool GetComputerName(StringBuilder buffer, ref uint size);



    static void Main()
    {
        StringBuilder buffer = new StringBuilder(128);
        StringBuilder tmp = new StringBuilder(128);

        GetSystemDirectory(tmp, 128);
        Console.WriteLine($"System Directory {tmp}");

        uint l = 128;
        GetComputerName(tmp, ref l);
        Console.WriteLine($"Computer name {tmp}");

        Console.WriteLine($"Major Version: {System.Environment.OSVersion.Version.Major}");
        Console.WriteLine($"Minor Version: {System.Environment.OSVersion.Version.Minor}");
        Console.WriteLine($"Build Number: {System.Environment.OSVersion.Version}");
    }
}

