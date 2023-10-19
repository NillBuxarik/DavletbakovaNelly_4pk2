#include <stdio.h>
#include <windows.h>

void printRegionAddress(LPVOID lpAddress, SIZE_T dwSize) {
    printf("Region Address: %p - %p\n", lpAddress, (char*)lpAddress + dwSize - 1);
}

int main() {
    SYSTEM_INFO sysInfo;
    GetSystemInfo(&sysInfo);
    SIZE_T dwPageSize = sysInfo.dwPageSize;

    
    LPVOID lpRegion1 = VirtualAlloc(NULL, 2 * dwPageSize, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
    LPVOID lpRegion2 = VirtualAlloc(NULL, 2 * dwPageSize, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

    if (lpRegion1 == NULL || lpRegion2 == NULL) {
        fprintf(stderr, "Error reserving memory\n");
        return GetLastError();
    }

    printf("Reserved regions:\n");
    printRegionAddress(lpRegion1, 2 * dwPageSize);
    printRegionAddress(lpRegion2, 2 * dwPageSize);


    ZeroMemory(lpRegion1, 2 * dwPageSize);

    int number;
    printf("Enter an integer between 0 and 127: ");
    if (scanf_s("%d", &number) != 1) {
        fprintf(stderr, "Error reading the number\n");
        VirtualFree(lpRegion1, 0, MEM_RELEASE);
        VirtualFree(lpRegion2, 0, MEM_RELEASE);
        return -1;
    }

    if (number < 0 || number > 127) {
        fprintf(stderr, "The number is outside the valid range\n");
        VirtualFree(lpRegion1, 0, MEM_RELEASE);
        VirtualFree(lpRegion2, 0, MEM_RELEASE);
        return -1;
    }

    int fillValue = number;
    FillMemory(lpRegion2, 2 * dwPageSize, fillValue);

    printf("\nContents of the regions:\n");
    for (char* ptr = (char*)lpRegion1; ptr < (char*)lpRegion1 + 2 * dwPageSize; ptr++) {
        printf("%02X ", *ptr);
    }
    printf("\n");
    for (char* ptr = (char*)lpRegion2; ptr < (char*)lpRegion2 + 2 * dwPageSize; ptr++) {
        printf("%02X ", *ptr);
    }
    printf("\n");

  
    VirtualFree(lpRegion1, 0, MEM_RELEASE);
    VirtualFree(lpRegion2, 0, MEM_RELEASE);

    return 0;
}