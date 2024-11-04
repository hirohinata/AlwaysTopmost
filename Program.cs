using System.Runtime.InteropServices;
using System.Text;

[DllImport("user32.dll")] static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
[DllImport("user32.dll")] static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
[DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr hWnd);

const int HWND_TOPMOST = -1;
const uint SWP_NOSIZE = 0x0001;
const uint SWP_NOMOVE = 0x0002;
const uint SWP_SHOWWINDOW = 0x0040;
[DllImport("user32.dll")] static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

Console.WriteLine("最前面にしたいWindowのタイトル（の一部）を入力してください。");
Console.Write("> ");
string targetText = Console.ReadLine() ?? string.Empty;
if (string.IsNullOrEmpty(targetText)) return;

EnumWindows(new EnumWindowsProc(
    (IntPtr hWnd, IntPtr lParam) =>
        {
            if (!IsWindowVisible(hWnd)) return true;

            StringBuilder windowText = new StringBuilder(256);
            GetWindowText(hWnd, windowText, 256);
            if (!windowText.ToString().Contains(targetText)) return true;

            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_SHOWWINDOW);
            return true;
        }),
    IntPtr.Zero);

delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
