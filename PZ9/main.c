#include <stdio.h>
#include <windows.h>

void printRegionAddress(LPVOID lpAddress, SIZE_T dwSize) {
    printf("Region Address: %p - %p\n", lpAddress, (char*)lpAddress + dwSize - 1);
}

int main() {
    SYSTEM_INFO sysInfo;
    GetSystemInfo(&sysInfo);
    SIZE_T dwPageSize = sysInfo.dwPageSize;

    // Reserve two regions of memory, each with a size of 2 pages
    LPVOID lpRegion1 = VirtualAlloc(NULL, 2 * dwPageSize, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
    LPVOID lpRegion2 = VirtualAlloc(NULL, 2 * dwPageSize, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

    if (lpRegion1 == NULL || lpRegion2 == NULL) {
        fprintf(stderr, "Error reserving memory\n");
        return GetLastError();
    }

    // Print addresses of the reserved regions
    printf("Reserved regions:\n");
    printRegionAddress(lpRegion1, 2 * dwPageSize);
    printRegionAddress(lpRegion2, 2 * dwPageSize);

    // Clear data in the first memory region
    ZeroMemory(lpRegion1, 2 * dwPageSize);

    // Read an integer in the range of 0..127
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

    // Fill both regions with the entered number
    int fillValue = number;
    FillMemory(lpRegion2, 2 * dwPageSize, fillValue);

    // Display the contents of the regions
    printf("\nContents of the regions:\n");
    for (char* ptr = (char*)lpRegion1; ptr < (char*)lpRegion1 + 2 * dwPageSize; ptr++) {
        printf("%02X ", *ptr);
    }
    printf("\n");
    for (char* ptr = (char*)lpRegion2; ptr < (char*)lpRegion2 + 2 * dwPageSize; ptr++) {
        printf("%02X ", *ptr);
    }
    printf("\n");

    // Free the allocated memory
    VirtualFree(lpRegion1, 0, MEM_RELEASE);
    VirtualFree(lpRegion2, 0, MEM_RELEASE);

    return 0;
}