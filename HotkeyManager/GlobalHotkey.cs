using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Common.Logging;

namespace hotkeyManager
{
    /// <summary> This class allows you to manage a hotkey </summary>
    public class GlobalHotkey
    {
        public int HotkeyId { get; private set; }
        public Action HotkeyAction { get; internal set; }
        public Keys HotkeKeys { get; private set; }
        public KeyModifiers HotkeyModifiers { get; private set; }        
        public event EventHandler<HotKeyEventArgs> OnBeforeHotkeyExecute;
        public event EventHandler<HotKeyEventArgs> OnAfterHotKeyExecute;

        private readonly ILog _log = LogManager.GetLogger(typeof (GlobalHotkey));

        [DllImport("user32", SetLastError = true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32", SetLastError = true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private readonly IntPtr _handle;
        private bool _isRegistered;


        public GlobalHotkey(Keys hotkeKeys, KeyModifiers hotkeyModifiers, int id, IntPtr handle, Action action)
        {
            HotkeKeys = hotkeKeys;
            HotkeyModifiers = hotkeyModifiers;
            _handle = handle;
            HotkeyId = id;
            HotkeyAction = action;
        }

        /// <summary>Register the hotkey</summary>
        public void RegisterGlobalHotKey()
        {
            UnregisterGlobalHotKey();

            // register the hotkey, throw if any error
            if (RegisterHotKey(_handle, HotkeyId, (uint)HotkeyModifiers, (uint)HotkeKeys))
            {
                _isRegistered = true;
                return;
            }
               

            int error = Marshal.GetLastWin32Error();
            throw new HotkeyException("Unable to register hotkey", error);
        }

        /// <summary>Unregister the hotkey</summary>
        public void UnregisterGlobalHotKey()
        {
            if (!_isRegistered)
                return;
            if (UnregisterHotKey(_handle, HotkeyId))
            {
                _isRegistered = false;
                return;
            }

            int error = Marshal.GetLastWin32Error();
            throw new HotkeyException("Unable to register hotkey", error);
        }

        public void ExecuteHotkey(Object sender, HotKeyEventArgs eventArgs)
        {
            if (HotkeKeys == eventArgs.Key && HotkeyModifiers == eventArgs.Modifiers)
            {
                if (OnBeforeHotkeyExecute != null)
                {
                    OnBeforeHotkeyExecute(this, new HotKeyEventArgs(HotkeKeys, HotkeyModifiers));
                }
                if (HotkeyAction != null)
                {
                    HotkeyAction();
                }
                else
                {
                    _log.WarnFormat("For the recognized hotkey is no action registered. Hotkey:{0}, Modifier:{1}",HotkeKeys, HotkeyModifiers);
                }
                if (OnAfterHotKeyExecute != null)
                {
                    OnAfterHotKeyExecute(this, new HotKeyEventArgs(HotkeKeys, HotkeyModifiers));
                }
            }
        }
    }
}