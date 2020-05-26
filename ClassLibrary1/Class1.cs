using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ClassLibrary1
{
    public class Class1
    {        
        public static class NativeMethods
        {
            [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
            public static extern void CopyMemory(IntPtr D, IntPtr S, uint size);
        }

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptionsAttribute]

        public void MoveMemory(IntPtr src,ref IntPtr dst ,uint size)
        {

            NativeMethods.CopyMemory(dst, src, size);

        }

    }
}
