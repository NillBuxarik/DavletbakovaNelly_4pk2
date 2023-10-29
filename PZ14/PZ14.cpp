#define STRICT
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <tchar.h>

static TCHAR szWindowClass[] = _T("DesktopApp");
static TCHAR szTitle[] = _T("Windows Desktop Guided Tour Application");

LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);

HINSTANCE hInst;

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    WNDCLASSEX wcex;
    HWND hWnd;
    MSG msg;

    wcex.cbSize = sizeof(WNDCLASSEX);
    wcex.style = CS_HREDRAW | CS_VREDRAW;
    wcex.lpfnWndProc = WndProc;
    wcex.cbClsExtra = 0;
    wcex.cbWndExtra = 0;
    wcex.hInstance = hInstance;
    wcex.hIcon = LoadIcon(wcex.hInstance, IDI_APPLICATION);
    wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
    wcex.lpszMenuName = NULL;
    wcex.lpszClassName = szWindowClass;
    wcex.hIconSm = LoadIcon(wcex.hInstance, IDI_APPLICATION);

    if (!RegisterClassEx(&wcex))
    {
        MessageBox(NULL,
            _T("Call to RegisterClassEx failed!"),
            _T("Windows Desktop Guided Tour"),
            NULL);
        return 1;
    }

    hInst = hInstance;

    hWnd = CreateWindowEx(
        WS_EX_OVERLAPPEDWINDOW,
        szWindowClass,
        szTitle,
        WS_OVERLAPPEDWINDOW,
        CW_USEDEFAULT, CW_USEDEFAULT,
        650, 200,
        NULL,
        NULL,
        hInstance,
        NULL
    );

    if (!hWnd)
    {
        MessageBox(NULL,
            _T("Вызов CreateWindow завершился неудачно!"),
            NULL, NULL);
        return 1;
    }

    ShowWindow(hWnd, nCmdShow);
    UpdateWindow(hWnd);

    while (GetMessage(&msg, NULL, 0, 0))
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return (int)msg.wParam;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    PAINTSTRUCT ps;
    HDC hdc;
    TCHAR greeting[] = _T("Место преступления");

    switch (message)
    {
    case WM_PAINT:
    {
        hdc = BeginPaint(hWnd, &ps);

        //  новый шрифт
        HFONT hFont = CreateFont(
            30, // высота шрифта
            0,  // ширина шрифта 
            50,  // Угол наклона 
            0,  // Угол поворота 
            FW_NORMAL, // насыщенность шрифта
            TRUE,     // курсив
            FALSE,     // подчеркивание
            TRUE,     // зачеркивание
            DEFAULT_CHARSET,  // идентификатор набора символов
            OUT_DEFAULT_PRECIS, // точность вывода
            CLIP_DEFAULT_PRECIS, // точность обрезки
            DEFAULT_QUALITY, // качество вывода
            DEFAULT_PITCH, // шаг и семейство
            _T("Arial")    // название шрифта
        );

        // Выбираем шрифт в контекст устройства
        HFONT hOldFont = (HFONT)SelectObject(hdc, hFont);

        // Устанавливаем цвет текста
        SetTextColor(hdc, RGB(0, 0, 0));

        // Устанавливаем цвет фона
        SetBkColor(hdc, RGB(255, 255, 0));

        // Рисуем текст
        TextOut(hdc, 100, 60, greeting, _tcslen(greeting));

        // Восстанавливаем исходный шрифт и освобождаем ресурсы
        SelectObject(hdc, hOldFont);
        DeleteObject(hFont);

        EndPaint(hWnd, &ps);
    }
    break;

    case WM_ERASEBKGND:
    {
        HDC hdc = (HDC)wParam;
        RECT rect;
        GetClientRect(hWnd, &rect);

        HBRUSH hBrush = CreateSolidBrush(RGB(255, 0, 0));

        FillRect(hdc, &rect, hBrush);

        DeleteObject(hBrush);

        return (LRESULT)1; // Указываем, что фон был обработан
    }
    break;

    case WM_DESTROY:
        PostQuitMessage(0);
        break;

    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
        break;
    }

    return 0;
}
