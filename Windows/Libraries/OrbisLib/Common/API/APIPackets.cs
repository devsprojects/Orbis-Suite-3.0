﻿using System.Runtime.InteropServices;

namespace OrbisSuite.Common
{
    public enum APICommands : int
    {
        APITest = 1,

        /* ####### Proc functions ####### */
        PROC_START,
        API_PROC_GET_LIST,
        API_PROC_ATTACH,
        API_PROC_DETACH,
        API_PROC_GET_CURRENT,
        API_PROC_READ,
        API_PROC_WRITE,
        API_PROC_KILL,
        API_PROC_LOAD_ELF,
        API_PROC_CALL,

        /* Remote Library functions */
        API_PROC_LOAD_SPRX,
        API_PROC_UNLOAD_SPRX,
        API_PROC_UNLOAD_SPRX_NAME,
        API_PROC_RELOAD_SPRX_NAME,
        API_PROC_RELOAD_SPRX_HANDLE,
        API_PROC_DUMP_MODULE,
        API_PROC_MODULE_LIST,
        PROC_END,
        /* ############################## */

        /* ##### Debugger functions ##### */
        DBG_START,
        API_DBG_START, /* Debugger attach to target */
        API_DBG_STOP, /* Debugger detach from target */
        API_DBG_BREAK,
        API_DBG_RESUME,
        API_DBG_SIGNAL,
        API_DBG_STEP,
        API_DBG_STEP_OVER,
        API_DBG_STEP_OUT,
        API_DBG_GET_CALLSTACK,
        API_DBG_GET_REG,
        API_DBG_SET_REG,
        API_DBG_GET_FREG,
        API_DBG_SET_FREG,
        API_DBG_GET_DBGREG,
        API_DBG_SET_DBGREG,

        /* Thread Management */
        API_DBG_THREAD_LIST,
        API_DBG_THREAD_STOP,
        API_DBG_THREAD_RESUME,

        /* Breakpoint functions */
        API_DBG_BREAKPOINT_GETFREE,
        API_DBG_BREAKPOINT_SET,
        API_DBG_BREAKPOINT_UPDATE,
        API_DBG_BREAKPOINT_REMOVE,
        API_DBG_BREAKPOINT_GETINFO,
        API_DBG_BREAKPOINT_LIST,

        /* Watchpoint functions */
        API_DBG_WATCHPOINT_SET,
        API_DBG_WATCHPOINT_UPDATE,
        API_DBG_WATCHPOINT_REMOVE,
        API_DBG_WATCHPOINT_GETINFO,
        API_DBG_WATCHPOINT_LIST,
        DBG_END,
        /* ############################## */

        /* ###### Kernel functions ###### */
        KERN_START,
        API_KERN_BASE,
        API_KERN_READ,
        API_KERN_WRITE,
        KERN_END,
        /* ############################## */

        /* ###### Target functions ###### */
        TARGET_START,
        API_TARGET_INFO,
        API_TARGET_RESTMODE,
        API_TARGET_SHUTDOWN,
        API_TARGET_REBOOT,
        API_TARGET_NOTIFY,
        API_TARGET_BUZZER,
        API_TARGET_SET_LED,
        API_TARGET_DUMP_PROC,
        API_TARGET_SET_SETTINGS,
        //API_TARGET_LOAD_VSH_MODULE
        TARGET_END,
        /* ############################## */
    }

    public enum APIResults : int
    {
        API_OK = 1,

        API_ERROR_COULDNT_CONNECT,
        API_ERROR_NOT_CONNECTED,
        API_ERROR_NOT_ATTACHED,
        API_ERROR_LOST_PROC,
        API_ERROR_GENERAL,
        API_ERROR_INVALID_ADDRESS,

        //Debugger
        API_ERROR_PROC_RUNNING,
        API_ERROR_DEBUG_TO_ATTACHED,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 40), Serializable]
    public struct APIPacket
    {
        public int PacketVersion;
        public APICommands Command;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ProcName;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Ansi)]
    public struct ProcPacket
    {
        public int ProcessID;
        public int Attached;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string ProcName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string TitleID;
        public UInt64 TextSegmentBase;
        public UInt64 TextSegmentLen;
        public UInt64 DataSegmentBase;
        public UInt64 DataSegmentLen;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct ModuleListPacket
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Path;
        public int Handle;
        public UInt64 TextSegmentBase;
        public UInt64 TextSegmentLen;
        public UInt64 DataSegmentBase;
        public UInt64 DataSegmentLen;
    }

    public enum ConsoleTypes
    {
        UNK,
        DIAG, //0x80
        DEVKIT, //0x81
        TESTKIT, //0x82
        RETAIL, //0x83 -> 0x8F
        KRATOS, //0xA0 IMPOSSIBLE??
    };

    public enum ConsoleLEDColours
    {
        white,
        white_Blinking,
        Blue_Blinking,
    };

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi, Size = 260), Serializable]
    public struct TargetInfoPacket
    {
        public int SDKVersion;
        public int SoftwareVersion;
        public int FactorySoftwareVersion;
        public int CPUTemp;
        public int SOCTemp;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string CurrentTitleID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
        public string ConsoleName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string MotherboardSerial;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Serial;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string Model;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] MACAddressLAN;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] MACAddressWIFI;
        public int UART;
        public int IDUMode;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] IDPS;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] PSID;
        public int ConsoleType;
        public int Attached;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string CurrentProc;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct ProcRWPacket
    {
        public UInt64 Address;
        public UInt64 Length;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct ProcSPRXPacket
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Path;
        public int ModuleHandle;
        public int Flags;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct ProcBreakpointPacket
    {
        public int Index;
        public UInt64 Address;
        public int Enable;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct TargetNotifyPacket
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string IconURI;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
        public string Message;
    }

    public enum BuzzerType
    {
        RingOnce = 1,
        RingThree,
        LongRing,
        ThreeLongRing,
        ThreeLongDoubleBeeps,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
    public struct TargetSettingsPacket
    {
        public int AutoLoadSettings;
        public int ShowDebugTitleIdLabel;
        public int ShowDevkitPanel;
        public int ShowDebugSettings;
        public int ShowAppHome;
        public int ShowBuildOverlay;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string GameOverlayLocation;
        public int ShowCPUUsage;
        public int ShowThreadCount;
        public int Showram;
        public int Showvram;
        public int ShowCPUTemp;
        public int ShowSOCTemp;
    };
}
