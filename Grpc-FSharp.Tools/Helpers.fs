namespace Grpc.FSharp.Tools

open System
open System.Runtime.InteropServices

module Metadata =
    let source = "Source"
    let protoRoot = "ProtoRoot"
    let outputDir = "OutputDir"
    let grpcServices = "GrpcServices"
    
module Platform =
    type OSKind = Windows | Linux | MacOSX | Unknown
    type ProcessorArchitecture = X64 | X86 | Unknown

#if NETCOREAPP
    let os =
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        then Windows
        elif RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
        then Linux
        elif RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
        then MacOSX
        else OSKind.Unknown

    let processorArchitecture =
        match RuntimeInformation.ProcessArchitecture with
        | Architecture.X86 -> X86
        | Architecture.X64 -> X64
        | _ -> Unknown
#else
    [<DllImport("libc")>]
    extern int uname(IntPtr buffer)

    let private GetUname() =
        let buffer = Marshal.AllocHGlobal 8192
        try
            try
                if uname buffer = 0
                then Marshal.PtrToStringAnsi buffer
                else String.Empty
            with _ -> String.Empty
        finally
            if buffer <> IntPtr.Zero
            then Marshal.FreeHGlobal buffer

    let os =
        let platform = Environment.OSVersion.Platform
        match platform with
        | PlatformID.Win32NT | PlatformID.Win32S | PlatformID.Win32Windows -> Windows
        | PlatformID.Unix when GetUname() = "Darwin" -> MacOSX
        | PlatformID.Unix -> Linux
        | _ -> OSKind.Unknown

    let processorArchitecture =
        // The official Grpc.Tools package has this to say:
        // Hope we are not building on ARM under Xamarin!
        if Environment.Is64BitProcess
        then X64
        else X86
#endif

module Exceptions =
    let isIORelated (ex: Exception) =
        ex :? System.IO.IOException
        || (ex :? ArgumentException && not <| ex :? ArgumentNullException)
        || ex :? Security.SecurityException
        || ex :? UnauthorizedAccessException
        || ex :? NotSupportedException

module String =
    let equalsIgnoreCase a b =
        String.Equals(a, b, StringComparison.OrdinalIgnoreCase)