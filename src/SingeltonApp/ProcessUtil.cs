using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace SingeltonApp
{    
    public static class ProcessUtil
    {
        /// <summary>
        /// Gets all processes that are copies of this process including .vshost
        /// excluding current process sorted by starttime asc. 
        /// </summary>
        /// <returns>A List of processes</returns>
        public static List<Process> GetProcesses()
        {
            var processName = Process.GetCurrentProcess().ProcessName.Replace(".vshost", "");

            var processId = Process.GetCurrentProcess().Id;

            var processes = Process.GetProcesses().Where(p =>
                                                         p.ProcessName.Contains(processName) &&
                                                         p.Id != processId).ToList();

            processes.Sort((process, process1) => process.StartTime.CompareTo(process1.StartTime));

            return processes;
        }

        public static string GetCurrentProcessName()
        {
            return Process.GetCurrentProcess().ProcessName.Replace(".vshost", "");
        }

        public static void Focus(this Process process)
        {
            User32.SetForegroundWindow(process.MainWindowHandle);
            //FlashWindow(process.MainWindowHandle, true);

            var info = new User32.FLASHWINFO();
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.hwnd = process.MainWindowHandle;
            info.dwFlags = User32.FLASHW_ALL;
            info.dwTimeout = 0;
            info.uCount = 4;

            User32.FlashWindowEx(ref info);
        }
    }
}