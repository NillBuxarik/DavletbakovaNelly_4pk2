#include <stdio.h>
extern int y;
int x = 10;
void func2(void);
void func3(void);
int main(void)
{
	func3();
	return 0;
}
void func1()
{
	printf("func1: x=%d \t y=%d \n", x, y);
	func2();
}