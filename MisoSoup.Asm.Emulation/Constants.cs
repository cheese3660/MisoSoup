using System.Runtime.CompilerServices;

namespace MisoSoup.Asm.Emulation;

public static class Constants
{
    private static Dictionary<string, int> _constants = new()
    {
        ["true"] = 1,
        ["false"] = 0,
        ["PI"] = Unsafe.BitCast<float, int>(float.Pi),
        ["TAU"] = Unsafe.BitCast<float, int>(float.Tau),
        ["EXP1"] = Unsafe.BitCast<float, int>(float.E),
        ["INF"] = Unsafe.BitCast<float, int>(float.PositiveInfinity),
        ["NAN"] = Unsafe.BitCast<float, int>(float.NaN),

        ["SCREEN_WIDTH"] = 320,
        ["SCREEN_HEIGHT"] = 240,
        ["MAX_TERMINAL_INPUT_SIZE"] = 256,

        ["BTN_SELECT"] = 512,
        ["BTN_START"] = 256,
        ["BTN_LEFT"] = 128,
        ["BTN_RIGHT"] = 64,
        ["BTN_UP"] = 32,
        ["BTN_DOWN"] = 16,
        ["BTN_A"] = 8,
        ["BTN_B"] = 4,
        ["BTN_X"] = 2,
        ["BTN_Y"] = 1,

        ["MOUSE_BUTTON_LEFT"] = 1,
        ["MOUSE_BUTTON_RIGHT"] = 2,
        ["MOUSE_BUTTON_MIDDLE"] = 4,
        ["MOUSE_BUTTON_WHEEL_UP"] = 8,
        ["MOUSE_BUTTON_WHEEL_DOWN"] = 16,

        ["KBE_PRESSED"] = 1,
        ["KBE_REPEAT"] = 2,
        ["KBE_CTRL"] = 4,
        ["KBE_SHIFT"] = 8,
        ["KBE_ALT"] = 16,

        ["KEY_TAB"] = 0x80,
        ["KEY_BACKSPACE"] = 0x81,
        ["KEY_ENTER"] = 0x82,
        ["KEY_ESC"] = 0x83,
        ["KEY_CTRL"] = 0x84,
        ["KEY_SHIFT"] = 0x85,
        ["KEY_ALT"] = 0x86,
        ["KEY_LEFT"] = 0x87,
        ["KEY_RIGHT"] = 0x88,
        ["KEY_UP"] = 0x89,
        ["KEY_DOWN"] = 0x8a,
        ["KEY_INSERT"] = 0x8b,
        ["KEY_DELETE"] = 0x8c,
        ["KEY_HOME"] = 0x8d,
        ["KEY_END"] = 0x8e,
        ["KEY_PAGE_UP"] = 0x8f,
        ["KEY_PAGE_DOWN"] = 0x90,
    };

    public static int? TryGetConstantValue(string constant) =>
        _constants.TryGetValue(constant, out var result) ? result : null;
}