#include <stdio.h>
#include <windows.h>

int main() {
    // �������������� ������� ������ �������� � 1 �������� (4096 ����)
    LPVOID lpAddress = VirtualAlloc(NULL, 4096, MEM_RESERVE, PAGE_READWRITE);

    if (lpAddress == NULL) {
        fprintf(stderr, "������ �������������� ������\n");
        return GetLastError();
    }

    // ����������� ������� ������
    LPVOID lpMappedAddress = VirtualAlloc(lpAddress, 4096, MEM_COMMIT, PAGE_READWRITE);

    if (lpMappedAddress == NULL) {
        fprintf(stderr, "������ ����������� ������\n");
        VirtualFree(lpAddress, 0, MEM_RELEASE);
        return GetLastError();
    }

    // ���������� ������ ��������� 7Fh
    memset(lpMappedAddress, 0x7F, 4096);

    // ���������� �� ������� ������
    MEMORY_BASIC_INFORMATION mbi;
    DWORD dwReturnLength = VirtualQuery(lpMappedAddress, &mbi, sizeof(mbi));

    if (dwReturnLength == 0) {
        fprintf(stderr, "������ ��� ��������� ���������� � ������\n");
    }
    else {
        printf("Base Address: %p\n", mbi.BaseAddress);
        printf("Allocation Base: %p\n", mbi.AllocationBase);
        printf("Allocation Protect: %lu\n", mbi.AllocationProtect);
        printf("Region Size: %llu\n", (unsigned long long)mbi.RegionSize);
        printf("State: %lu\n", mbi.State);
        printf("Protect: %lu\n", mbi.Protect);
        printf("Type: %lu\n", mbi.Type);
    }

    // ������������ ������
    VirtualFree(lpMappedAddress, 0, MEM_RELEASE);

    return 0;
}