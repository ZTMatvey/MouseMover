using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;

namespace MouseMover
{
    public sealed class HotKeyManager
    {
        private HwndSource _source;
        private readonly int ID;
        private readonly Window m_window;
        public event Action OnHotKeyPressed;

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
    [In] IntPtr hWnd,
    [In] int id,
    [In] uint fsModifiers,
    [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);
        public HotKeyManager(int id, Window window)
        {
            ID = id;
            m_window = window;
        }
        public void OnInitialized()
        {
            var helper = new WindowInteropHelper(m_window);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source.AddHook(HwndHook);
            RegisterHotKey();
        }

        public void OnClosed()
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
        }
        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(m_window);
            const uint VK_F10 = 0x51;
            const uint MOD_CTRL = 0x0006;
            if (!RegisterHotKey(helper.Handle, ID, MOD_CTRL, VK_F10))
            {}
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(m_window);
            UnregisterHotKey(helper.Handle, ID);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    if(wParam.ToInt32() == ID)
                    {
                        OnHotKeyPressed?.Invoke();
                        handled = true;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
    }
}
