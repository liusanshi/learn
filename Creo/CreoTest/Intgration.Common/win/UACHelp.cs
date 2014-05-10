using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Intgration.Common.win
{
    public static class UACHelp
    {
        /// <summary>
        /// 此函数检测当前进程是否以管理员身份运行。换而言之，此进程要求用户是
        /// 拥有主访问令牌的用户是管理员组成员并且已经执行了权限提升。
        /// </summary>
        /// <returns>
        /// 如果拥有主访问令牌的用户是管理员组成员且已经执行了权限提升则返回
        /// TRUE，反之则返回FALSE。
        /// </returns>
        public static bool IsRunAsAdmin()
        {
            //if (!IsRunAsAdmin())
            //{
            //    // Launch itself as administrator 
            //    ProcessStartInfo proc = new ProcessStartInfo();
            //    proc.UseShellExecute = true;
            //    proc.WorkingDirectory = Environment.CurrentDirectory;
            //    proc.FileName = Assembly.GetExecutingAssembly().Location;
            //    proc.Verb = "runas"; //管理员启动

            //    try
            //    {
            //        Process.Start(proc);
            //    }
            //    catch
            //    {
            //        // The user refused the elevation. 
            //        // Do nothing and return directly ... 
            //        return;
            //    }

            //    Application.Exit();  // Quit itself 
            //} 

            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 即使在还没有为当前用户提升权限的前提下，此函数检测拥有此进程的主访
        /// 问令牌的用户是否是本地管理员组的成员。
        /// </summary>
        /// <returns>
        /// 如果拥有主访问令牌的用户是管理员组成员则返回TRUE，反之则返回FALSE。
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// 如果任何原生的Windows API函数出错，此函数会抛出一个包含最后错误代码的Win32Exception。
        /// </exception>
        public static bool IsUserInAdminGroup()
        {
            bool fInAdminGroup = false;
            SafeTokenHandle hToken = null;
            SafeTokenHandle hTokenToCheck = null;
            IntPtr pElevationType = IntPtr.Zero;
            IntPtr pLinkedToken = IntPtr.Zero;
            int cbSize = 0;

            try
            {
                // Open the access token of the current process for query and duplicate.
                if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
                    NativeMethods.TOKEN_QUERY | NativeMethods.TOKEN_DUPLICATE, out hToken))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                // Determine whether system is running Windows Vista or later operating
                // systems (major version >= 6) because they support linked tokens, but
                // previous versions (major version < 6) do not.
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    // Running Windows Vista or later (major version >= 6).
                    // Determine token type: limited, elevated, or default.

                    // Allocate a buffer for the elevation type information.
                    cbSize = sizeof(TOKEN_ELEVATION_TYPE);
                    pElevationType = Marshal.AllocHGlobal(cbSize);
                    if (pElevationType == IntPtr.Zero)
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }

                    // Retrieve token elevation type information.
                    if (!NativeMethods.GetTokenInformation(hToken,
                        TOKEN_INFORMATION_CLASS.TokenElevationType, pElevationType,
                        cbSize, out cbSize))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }

                    // Marshal the TOKEN_ELEVATION_TYPE enum from native to .NET.
                    TOKEN_ELEVATION_TYPE elevType = (TOKEN_ELEVATION_TYPE)
                        Marshal.ReadInt32(pElevationType);

                    // If limited, get the linked elevated token for further check.
                    if (elevType == TOKEN_ELEVATION_TYPE.TokenElevationTypeLimited)
                    {
                        // Allocate a buffer for the linked token.
                        cbSize = IntPtr.Size;
                        pLinkedToken = Marshal.AllocHGlobal(cbSize);
                        if (pLinkedToken == IntPtr.Zero)
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }

                        // Get the linked token.
                        if (!NativeMethods.GetTokenInformation(hToken,
                            TOKEN_INFORMATION_CLASS.TokenLinkedToken, pLinkedToken,
                            cbSize, out cbSize))
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }

                        // Marshal the linked token value from native to .NET.
                        IntPtr hLinkedToken = Marshal.ReadIntPtr(pLinkedToken);
                        hTokenToCheck = new SafeTokenHandle(hLinkedToken);
                    }
                }

                // CheckTokenMembership requires an impersonation token. If we just got
                // a linked token, it already is an impersonation token.  If we did not
                // get a linked token, duplicate the original into an impersonation
                // token for CheckTokenMembership.
                if (hTokenToCheck == null)
                {
                    if (!NativeMethods.DuplicateToken(hToken,
                        SECURITY_IMPERSONATION_LEVEL.SecurityIdentification,
                        out hTokenToCheck))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }

                // Check if the token to be checked contains admin SID.
                WindowsIdentity id =
                        new WindowsIdentity(hTokenToCheck.DangerousGetHandle());
                WindowsPrincipal principal = new WindowsPrincipal(id);
                fInAdminGroup = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            finally
            {
                // Centralized cleanup for all allocated resources.
                if (hToken != null)
                {
                    hToken.Close();
                    hToken = null;
                }
                if (hTokenToCheck != null)
                {
                    hTokenToCheck.Close();
                    hTokenToCheck = null;
                }
                if (pElevationType != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pElevationType);
                    pElevationType = IntPtr.Zero;
                }
                if (pLinkedToken != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pLinkedToken);
                    pLinkedToken = IntPtr.Zero;
                }
            }

            return fInAdminGroup;
        }

        /// <summary>
        /// 此函数获取当前进程的权限提升信息。它由此进程是否进行了权限提升所
        /// 决定。令牌权限提升只有在Windows Vista及后续版本的Windows中有效。所以在
        /// Windows Vista之前的版本中执行IsProcessElevated， 它会抛出一个C++异常。
        /// 此函数并不适用于检测是否此进程以管理员身份运行。
        /// </summary>
        /// <returns>
        /// 如果此进程的权限已被提升，返回TRUE，反之则返回FALSE。
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// 如果任何原生的Windows API函数出错，此函数会抛出一个包含最后错误代码的Win32Exception。
        /// </exception>
        /// <remarks>
        /// TOKEN_INFORMATION_CLASS提供了TokenElevationType以便对当前进程的提升
        /// 类型（TokenElevationTypeDefault / TokenElevationTypeLimited /
        /// TokenElevationTypeFull）进行检测。 它和TokenElevation的不同之处在于：当UAC
        /// 关闭时，即使当前进程已经被提升(完整性级别 == 高)，权限提升类型总是返回
        /// TokenElevationTypeDefault。换而言之，以此来确认当前线程的提升类型是不安全的。
        /// 相对的，我们应该使用TokenElevation。
        /// </remarks>
        public static bool IsProcessElevated()
        {
            bool fIsElevated = false;
            SafeTokenHandle hToken = null;
            int cbTokenElevation = 0;
            IntPtr pTokenElevation = IntPtr.Zero;

            try
            {
                // Open the access token of the current process with TOKEN_QUERY. 
                if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
                    NativeMethods.TOKEN_QUERY, out hToken))
                {
                    throw new Win32Exception();
                }

                // Allocate a buffer for the elevation information. 
                cbTokenElevation = Marshal.SizeOf(typeof(TOKEN_ELEVATION));
                pTokenElevation = Marshal.AllocHGlobal(cbTokenElevation);
                if (pTokenElevation == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                // Retrieve token elevation information. 
                if (!NativeMethods.GetTokenInformation(hToken,
                    TOKEN_INFORMATION_CLASS.TokenElevation, pTokenElevation,
                    cbTokenElevation, out cbTokenElevation))
                {
                    // When the process is run on operating systems prior to Windows  
                    // Vista, GetTokenInformation returns false with the error code  
                    // ERROR_INVALID_PARAMETER because TokenElevation is not supported  
                    // on those operating systems. 
                    throw new Win32Exception();
                }

                // Marshal the TOKEN_ELEVATION struct from native to .NET object. 
                TOKEN_ELEVATION elevation = (TOKEN_ELEVATION)Marshal.PtrToStructure(
                    pTokenElevation, typeof(TOKEN_ELEVATION));

                // TOKEN_ELEVATION.TokenIsElevated is a non-zero value if the token  
                // has elevated privileges; otherwise, a zero value. 
                fIsElevated = (elevation.TokenIsElevated != 0);
            }
            finally
            {
                // Centralized cleanup for all allocated resources.  
                if (hToken != null)
                {
                    hToken.Close();
                    hToken = null;
                }
                if (pTokenElevation != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pTokenElevation);
                    pTokenElevation = IntPtr.Zero;
                    cbTokenElevation = 0;
                }
            }

            return fIsElevated;
        }


        /// <summary>
        /// 此函数获取当前线程的完整性级别。完整性级别只有在Windows Vista及后
        /// 续版本的Windows中有效。所以在Windows Vista之前的版本中执行GetProcessIntegrityLevel， 
        /// 它会抛出一个C++异常。
        /// </summary>
        /// <returns>
        /// 返回当前进程的完整性级别。它可能是以下某一个值。
        ///
        ///     SECURITY_MANDATORY_UNTRUSTED_RID (SID: S-1-16-0x0)
        ///     表示不被信任的级别。它被用于一个匿名组成员起动的进程。这时大多数
        ///     写入操作被禁止。
        ///
        ///     SECURITY_MANDATORY_LOW_RID (SID: S-1-16-0x1000)
        ///     表示低完整性级别。它被用于保护模式下的IE浏览器。这时大多数系统中对
        ///     象（包括文件及注册表键值）的写入操作被禁止。
        ///
        ///     SECURITY_MANDATORY_MEDIUM_RID (SID: S-1-16-0x2000)
        ///     表示中完整性级别。它被用于在UAC开启时启动普通应用程序。
        ///
        ///
        ///     SECURITY_MANDATORY_HIGH_RID (SID: S-1-16-0x3000)
        ///     表示高完整性级别。它被用于用户通过UAC提升权限启动一个管理员应用程序。
        ///     或则当UAC关闭时，管理员用户启动一个普通程序。
        ///
        ///
        ///     SECURITY_MANDATORY_SYSTEM_RID (SID: S-1-16-0x4000)
        ///     表示系统完整性级别。它被用于服务或则其他系统级别的应用程序（比如
        ///     Wininit, Winlogon, Smss，等等）

        /// 
        /// </returns>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// 如果任何原生的Windows API函数出错，此函数会抛出一个包含最后错误代码的Win32Exception。
        /// </exception>
        public static int GetProcessIntegrityLevel()
        {
            int IL = -1;
            SafeTokenHandle hToken = null;
            int cbTokenIL = 0;
            IntPtr pTokenIL = IntPtr.Zero;

            try
            {
                // Open the access token of the current process with TOKEN_QUERY. 
                if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle,
                    NativeMethods.TOKEN_QUERY, out hToken))
                {
                    throw new Win32Exception();
                }

                // Then we must query the size of the integrity level information  
                // associated with the token. Note that we expect GetTokenInformation  
                // to return false with the ERROR_INSUFFICIENT_BUFFER error code  
                // because we've given it a null buffer. On exit cbTokenIL will tell  
                // the size of the group information. 
                if (!NativeMethods.GetTokenInformation(hToken,
                    TOKEN_INFORMATION_CLASS.TokenIntegrityLevel, IntPtr.Zero, 0,
                    out cbTokenIL))
                {
                    int error = Marshal.GetLastWin32Error();
                    if (error != NativeMethods.ERROR_INSUFFICIENT_BUFFER)
                    {
                        // When the process is run on operating systems prior to  
                        // Windows Vista, GetTokenInformation returns false with the  
                        // ERROR_INVALID_PARAMETER error code because  
                        // TokenIntegrityLevel is not supported on those OS's. 
                        throw new Win32Exception(error);
                    }
                }

                // Now we allocate a buffer for the integrity level information. 
                pTokenIL = Marshal.AllocHGlobal(cbTokenIL);
                if (pTokenIL == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                // Now we ask for the integrity level information again. This may fail  
                // if an administrator has added this account to an additional group  
                // between our first call to GetTokenInformation and this one. 
                if (!NativeMethods.GetTokenInformation(hToken,
                    TOKEN_INFORMATION_CLASS.TokenIntegrityLevel, pTokenIL, cbTokenIL,
                    out cbTokenIL))
                {
                    throw new Win32Exception();
                }

                // Marshal the TOKEN_MANDATORY_LABEL struct from native to .NET object. 
                TOKEN_MANDATORY_LABEL tokenIL = (TOKEN_MANDATORY_LABEL)
                    Marshal.PtrToStructure(pTokenIL, typeof(TOKEN_MANDATORY_LABEL));

                // Integrity Level SIDs are in the form of S-1-16-0xXXXX. (e.g.  
                // S-1-16-0x1000 stands for low integrity level SID). There is one  
                // and only one subauthority. 
                IntPtr pIL = NativeMethods.GetSidSubAuthority(tokenIL.Label.Sid, 0);
                IL = Marshal.ReadInt32(pIL);
            }
            finally
            {
                // Centralized cleanup for all allocated resources.  
                if (hToken != null)
                {
                    hToken.Close();
                    hToken = null;
                }
                if (pTokenIL != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pTokenIL);
                    pTokenIL = IntPtr.Zero;
                    cbTokenIL = 0;
                }
            }

            return IL;
        } 
    }
}
