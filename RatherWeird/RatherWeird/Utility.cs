using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RatherWeird
{
    public class Utility
    {
        public class Networking
        {
            [DllImport("iphlpapi.dll", SetLastError = true)]
            private static extern uint GetExtendedTcpTable(
                IntPtr pTcpTable
                , ref int dwOutBufLen
                , bool sort
                , int ipVersion
                , TCP_TABLE_CLASS tblClass
                , uint reserved = 0
            );

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_TCPTABLE_OWNER_PID
            {
                public uint dwNumEntries;
                [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
                public MIB_TCPROW_OWNER_PID[] table;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_TCP6ROW_OWNER_PID
            {
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public byte[] localAddr;
                public uint localScopeId;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] localPort;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public byte[] remoteAddr;
                public uint remoteScopeId;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] remotePort;
                public uint state;
                public uint owningPid;

                public uint ProcessId
                {
                    get { return owningPid; }
                }

                public long LocalScopeId
                {
                    get { return localScopeId; }
                }

                public IPAddress LocalAddress
                {
                    get { return new IPAddress(localAddr, LocalScopeId); }
                }

                public ushort LocalPort
                {
                    get { return BitConverter.ToUInt16(localPort.Take(2).Reverse().ToArray(), 0); }
                }

                public long RemoteScopeId
                {
                    get { return remoteScopeId; }
                }

                public IPAddress RemoteAddress
                {
                    get { return new IPAddress(remoteAddr, RemoteScopeId); }
                }

                public ushort RemotePort
                {
                    get { return BitConverter.ToUInt16(remotePort.Take(2).Reverse().ToArray(), 0); }
                }

                public MIB_TCP_STATE State
                {
                    get { return (MIB_TCP_STATE)state; }
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_TCP6TABLE_OWNER_PID
            {
                public uint dwNumEntries;
                [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
                public MIB_TCP6ROW_OWNER_PID[] table;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MIB_TCPROW_OWNER_PID
            {
                public uint state;
                public uint localAddr;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] localPort;
                public uint remoteAddr;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] remotePort;
                public uint owningPid;

                public uint ProcessId
                {
                    get { return owningPid; }
                }

                public IPAddress LocalAddress
                {
                    get { return new IPAddress(localAddr); }
                }

                public ushort LocalPort
                {
                    get
                    {
                        return BitConverter.ToUInt16(new byte[2] { localPort[1], localPort[0] }, 0);
                    }
                }

                public IPAddress RemoteAddress
                {
                    get { return new IPAddress(remoteAddr); }
                }

                public ushort RemotePort
                {
                    get
                    {
                        return BitConverter.ToUInt16(new byte[2] { remotePort[1], remotePort[0] }, 0);
                    }
                }

                public MIB_TCP_STATE State
                {
                    get { return (MIB_TCP_STATE)state; }
                }
            }

            public enum MIB_TCP_STATE
            {
                MIB_TCP_STATE_CLOSED = 1,
                MIB_TCP_STATE_LISTEN = 2,
                MIB_TCP_STATE_SYN_SENT = 3,
                MIB_TCP_STATE_SYN_RCVD = 4,
                MIB_TCP_STATE_ESTAB = 5,
                MIB_TCP_STATE_FIN_WAIT1 = 6,
                MIB_TCP_STATE_FIN_WAIT2 = 7,
                MIB_TCP_STATE_CLOSE_WAIT = 8,
                MIB_TCP_STATE_CLOSING = 9,
                MIB_TCP_STATE_LAST_ACK = 10,
                MIB_TCP_STATE_TIME_WAIT = 11,
                MIB_TCP_STATE_DELETE_TCB = 12
            }

            

            public enum TCP_TABLE_CLASS
            {
                TCP_TABLE_BASIC_LISTENER,
                TCP_TABLE_BASIC_CONNECTIONS,
                TCP_TABLE_BASIC_ALL,
                TCP_TABLE_OWNER_PID_LISTENER,
                TCP_TABLE_OWNER_PID_CONNECTIONS,
                TCP_TABLE_OWNER_PID_ALL,
                TCP_TABLE_OWNER_MODULE_LISTENER,
                TCP_TABLE_OWNER_MODULE_CONNECTIONS,
                TCP_TABLE_OWNER_MODULE_ALL
            }

            public static List<Networking.MIB_TCPROW_OWNER_PID> GetAllTCPConnections()
            {
                return GetTCPConnections<Networking.MIB_TCPROW_OWNER_PID, MIB_TCPTABLE_OWNER_PID>((int)System.Net.Sockets.AddressFamily.InterNetwork);
            }

            public static List<MIB_TCP6ROW_OWNER_PID> GetAllTCPv6Connections()
            {
                return GetTCPConnections<MIB_TCP6ROW_OWNER_PID, MIB_TCP6TABLE_OWNER_PID>((int)System.Net.Sockets.AddressFamily.InterNetworkV6);
            }

            private static List<IPR> GetTCPConnections<IPR, IPT>(int ipVersion)//IPR = Row Type, IPT = Table Type
            {
                IPR[] tableRows;
                int buffSize = 0;

                var dwNumEntriesField = typeof(IPT).GetField("dwNumEntries");

                // how much memory do we need?
                uint ret = GetExtendedTcpTable(IntPtr.Zero, ref buffSize, true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
                IntPtr tcpTablePtr = Marshal.AllocHGlobal(buffSize);

                try
                {
                    ret = GetExtendedTcpTable(tcpTablePtr, ref buffSize, true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
                    if (ret != 0)
                        return new List<IPR>();

                    // get the number of entries in the table
                    IPT table = (IPT)Marshal.PtrToStructure(tcpTablePtr, typeof(IPT));
                    int rowStructSize = Marshal.SizeOf(typeof(IPR));
                    uint numEntries = (uint)dwNumEntriesField.GetValue(table);

                    // buffer we will be returning
                    tableRows = new IPR[numEntries];

                    IntPtr rowPtr = (IntPtr)((long)tcpTablePtr + 4);
                    for (int i = 0; i < numEntries; i++)
                    {
                        IPR tcpRow = (IPR)Marshal.PtrToStructure(rowPtr, typeof(IPR));
                        tableRows[i] = tcpRow;
                        rowPtr = (IntPtr)((long)rowPtr + rowStructSize);   // next entry
                    }
                }
                finally
                {
                    // Free the Memory
                    Marshal.FreeHGlobal(tcpTablePtr);
                }
                return tableRows != null ? tableRows.ToList() : new List<IPR>();
            }
        }
        
    }
}
