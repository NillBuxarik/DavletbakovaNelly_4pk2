// StaticLibrary.cpp : Определяет функции для статической библиотеки.
//

#include "pch.h"
#include "cmath"
#include "framework.h"
#include "StaticLibrary.h"

namespace StaticLibrary {

	double StaticLibraryClass::Pow(double a, double b)
	{
		return pow(a, b);
	}
	double StaticLibraryClass::Sqrt(double a, double b)
	{
		return pow(a, 1.0 / b);
	}
	double StaticLibraryClass::Sum(double a, double b)
	{
		return a + b;
	}
}
