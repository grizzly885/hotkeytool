using System;

namespace hotkeyManager
{
    [Flags]
    public enum KeyModifiers : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
        NoRepeat = 0x4000
    }
}
