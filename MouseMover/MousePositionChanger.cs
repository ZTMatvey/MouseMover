using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MouseMover
{
    public sealed class MousePositionChanger
    {
        private readonly Random m_random = new Random();
        public bool IsTaskRunning { get; private set; }
        public event Action Toggled;
        private CancellationTokenSource m_tokenSource;
        private Win32Point m_lastMousePosition;
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        private Win32Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return w32Mouse;
        }
        public void Toggle()
        {
            if (!IsTaskRunning)
                RunTask();
            else
                StopTask();
            IsTaskRunning = !IsTaskRunning;
            Toggled?.Invoke();
        }
        private async void RunTask()
        {
            m_lastMousePosition = GetMousePosition();
            m_tokenSource = new CancellationTokenSource();
            var cancellationToken = m_tokenSource.Token;
            await Task.Run(async () =>
            {
                var rules = Rules.GetRules();
                while (true)
                {
                    var x = m_random.Next(rules.MinX, rules.MaxX);
                    var y = m_random.Next(rules.MinY, rules.MaxX);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    SetCursorPos(x, y);
                    var delay = m_random.Next(rules.MinDelay, rules.MaxDelay);
                    await Task.Delay(delay);
                }
            }, m_tokenSource.Token);
        }
        private void StopTask()
        {
            m_tokenSource.Cancel();
            SetCursorPos(m_lastMousePosition.X, m_lastMousePosition.Y);
        }
    }
}
