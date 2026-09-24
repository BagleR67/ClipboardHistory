using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ClipboardHistory
{
    public class ClipboardLogic
    {
        // импорт из функций Windows чтобы можно было в шарпе вызывать эти функции
        [DllImport("user32.dll")]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        //код нужного письма от винды
        private const int WM_CLIPBOARDUPDATE = 0x031D;
        
        // событие "буфер апдейтнут" и мост
        public event Action<string>? ClipboardUpdate;
        private HwndSource? hwndSource;

        // вызывается, когда окно запускается. Подписывается на получение от винды сигналов по адресу 
        public void Attach(Window window)
        {
            hwndSource = HwndSource.FromVisual(window) as HwndSource;
            if (hwndSource == null) return;
            hwndSource.AddHook(WndProc);

            AddClipboardFormatListener(hwndSource.Handle);
        }

        // вызывается, когда окно умирает
        public void Detach()
        {
            if (hwndSource == null) return;

            RemoveClipboardFormatListener(hwndSource.Handle);
            hwndSource.RemoveHook(WndProc);
            hwndSource = null;
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // среди всех писем ловим одно нужное по коду
            if (msg == WM_CLIPBOARDUPDATE)
            {
                RaiseClipboardUpdated();
            }

            return IntPtr.Zero;
        }

        private void RaiseClipboardUpdated()
        {
            try
            {
                // В буфере может лежать не текст - выходим
                if (!Clipboard.ContainsText()) return;

                string text = Clipboard.GetText();

                // Пустое копирование в историю не тащим
                if (string.IsNullOrWhiteSpace(text)) return;

                // все подписчики получат текст
                ClipboardUpdate?.Invoke(text);
            }
            catch
            {
                // Буфер занят другим процессом — молча пропускаем это изменение
            }
        }


    }



}
