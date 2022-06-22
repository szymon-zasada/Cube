using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

public class NTP : MonoBehaviour
{


    void Start()
    {
        
    }

    public static async Task<long?> GetTime()
    {
        DateTime? networkTime = await GetNetworkTime();
        if (networkTime.HasValue)
        {
            return new DateTimeOffset((DateTime)networkTime).ToUnixTimeSeconds();
        }
        
        else
            return null;
        


    }

    private static async Task<DateTime?> GetNetworkTime()
    {
        string ntpServer = "pool.ntp.org";
        if (ntpServer == null)
            throw new ArgumentNullException(nameof(ntpServer));
        

        try
        {
            const int daysTo1900 = 1900 * 365 + 95; // 95 = offset for leap-years etc.
            const long ticksPerSecond = 10000000L;
            const long ticksPerDay = 24 * 60 * 60 * ticksPerSecond;
            const long ticksTo1900 = daysTo1900 * ticksPerDay;

            var ntpData = new byte[48];
            ntpData[0] = 0x1B; // LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            // ReSharper disable once RedundantAssignment

            var pingDuration = Stopwatch.GetTimestamp(); // temp access (JIT-Compiler need some time at first call)

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                await socket.ConnectAsync(ipEndPoint);
                socket.ReceiveTimeout = 5000;
                socket.Send(ntpData);
                pingDuration = Stopwatch.GetTimestamp(); // after Send-Method to reduce WinSocket API-Call time

                socket.Receive(ntpData);
                pingDuration = Stopwatch.GetTimestamp() - pingDuration;
            }

            var pingTicks = pingDuration * ticksPerSecond / Stopwatch.Frequency;

            // optional: display response-time
            // Console.WriteLine("{0:N2} ms", new TimeSpan(pingTicks).TotalMilliseconds);

            var intPart = (long)ntpData[40] << 24 | (long)ntpData[41] << 16 | (long)ntpData[42] << 8 | ntpData[43];
            var fractPart = (long)ntpData[44] << 24 | (long)ntpData[45] << 16 | (long)ntpData[46] << 8 | ntpData[47];
            var netTicks = intPart * ticksPerSecond + (fractPart * ticksPerSecond >> 32);

            var networkDateTime = new DateTime(ticksTo1900 + netTicks + pingTicks / 2);

            return networkDateTime.ToLocalTime(); // without ToLocalTime() = faster
        }
        catch
        {
            // fail
            return null;
        }
    }
}
