using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;


namespace hotkeyManager
{
    public class HotKeyManager : IHotkeyManager, IDisposable
    {
        private static readonly ManualResetEvent WindowReadyEvent = new ManualResetEvent(false);

        private delegate void RegisterHotKeyDelegate();

        private delegate void UnRegisterHotKeyDelegate();

        private static IntPtr _windowHandle;
        private static MessageWindow _messageWindow;
        private readonly string _hotkeyPrefix;
        private readonly Dictionary<int, GlobalHotkey> _hotkeys;

        internal event EventHandler<HotKeyEventArgs> OnHotKeyPressed;

        public HotKeyManager()
        {
            _hotkeys = new Dictionary<int, GlobalHotkey>();


            var messageLoop = new Thread(delegate()
            {
                _messageWindow = new MessageWindow(WindowReadyEvent);
                _windowHandle = _messageWindow.Handle;
                _messageWindow.OnKeyPressed += ExecuteOnHotKeyPressed;
                Application.Run(_messageWindow);
            })
            {
                Name = "MessageLoopThread",
                IsBackground = true
            };
            _hotkeyPrefix = Thread.CurrentThread.ManagedThreadId.ToString("X8") + GetType().FullName;
            messageLoop.Start();
            WindowReadyEvent.WaitOne(TimeSpan.FromSeconds(3f));
        }

        public void RegisterHotKey(Keys key, KeyModifiers modifiers, Action hotKeyAction)
        {
            if (_windowHandle == IntPtr.Zero)
            {
                throw new Exception("Handle is null. Could not register Key");
            }
            var id = GetHotKeyId(key, modifiers);
            if (!_hotkeys.ContainsKey(id))
            {
                WindowReadyEvent.WaitOne();

                var hotkey = new GlobalHotkey(key, modifiers, id, _windowHandle, hotKeyAction);
                OnHotKeyPressed += hotkey.ExecuteHotkey;
                _messageWindow.Invoke(new RegisterHotKeyDelegate(hotkey.RegisterGlobalHotKey));
                _hotkeys.Add(hotkey.HotkeyId, hotkey);
            }
            else
            {
                var hotKey = _hotkeys[id];
                hotKey.HotkeyAction = hotKeyAction;
            }
        }

        public void UnregisterHotKey(Keys key, KeyModifiers modifiers)
        {
            var id = GetHotKeyId(key, modifiers);
            if (_hotkeys.ContainsKey(id))
            {
                var hotKey = _hotkeys[id];
                _messageWindow.Invoke(new UnRegisterHotKeyDelegate(hotKey.UnregisterGlobalHotKey));
                OnHotKeyPressed -= hotKey.ExecuteHotkey;
                _hotkeys.Remove(id);
            }
        }

        private void ExecuteOnHotKeyPressed(Object sender, HotKeyEventArgs hotKeyEventArgs)
        {
            if (OnHotKeyPressed != null)
            {
                OnHotKeyPressed(this, hotKeyEventArgs);
            }
        }

        private int GetHotKeyId(Keys key, KeyModifiers modifiers)
        {
            return (_hotkeyPrefix + key + _hotkeyPrefix + modifiers).GetHashCode();
        }

        private class MessageWindow : Form
        {
            private readonly ManualResetEvent _windowReadyEvent;
            private const int WmHotkey = 0x312;
            public event EventHandler<HotKeyEventArgs> OnKeyPressed;

            public MessageWindow(ManualResetEvent windowReadyEvent)
            {
                _windowReadyEvent = windowReadyEvent;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WmHotkey)
                {
                    if (OnKeyPressed != null)
                    {
                        HotKeyEventArgs e = new HotKeyEventArgs(m.LParam);
                        OnKeyPressed(this, e);
                    }
                }
                base.WndProc(ref m);
            }

            protected override void SetVisibleCore(bool value)
            {
                // Ensure the window never becomes visible
                base.SetVisibleCore(false);
                _windowReadyEvent.Set();
            }
        }

        public void Dispose()
        {
            foreach (var hotKey in _hotkeys)
            {
                _messageWindow.Invoke(new UnRegisterHotKeyDelegate(hotKey.Value.UnregisterGlobalHotKey));
            }
        }
    }

    public interface IHotkeyManager
    {
        void UnregisterHotKey(Keys key, KeyModifiers modifiers);
        void RegisterHotKey(Keys key, KeyModifiers modifiers, Action hotKeyAction);
    }
}