#include "iostream"
#include "stdio.h"
#include "StaticLibrary.h"

int main(int)
{
	std::cout << "2 ** 5 = " << StaticLibrary::StaticLibraryClass::Pow(2, 5);
	std::cout << "\n";
	std::cout << "100 in degree 2 = " << StaticLibrary::StaticLibraryClass::Sqrt(100, 2);
	std::cout << "\n";
	std::cout << "100 + 2 = " << StaticLibrary::StaticLibraryClass::Sqrt(100, 2);
	return 0;
}