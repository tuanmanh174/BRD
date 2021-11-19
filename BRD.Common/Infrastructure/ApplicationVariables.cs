using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BRD.Common.Infrastructure
{
    public class ApplicationVariables
    {
       
        // Dictionary stored channel that is used for handling messages from the device
        // We added item to this dictionary when device is register, then remove them when deleting device
        public static ConcurrentDictionary<string, List<IModel>> DeviceChannelList = new ConcurrentDictionary<string, List<IModel>>();

        public static IConfiguration Configuration;
        public static ILoggerFactory LoggerFactory;
        // Dictionary that store message temporary when it is being sent to device.
        // Message will be remove from the dictionary when server received the response
        public static ConcurrentDictionary<string, string> SendingMessages = new ConcurrentDictionary<string, string>();

        // Dictionary to store pending list message that is sent via group messages
        // Key are device ID, value is list of message that send to device
        public static ConcurrentDictionary<string, List<string>> PendingGroupMessages = new ConcurrentDictionary<string, List<string>>();
        //public static List<string> DiscardMessageGroup = new List<string>;
        public static IHostingEnvironment env;
        public static void DebugSendingMessages()
        {
            foreach (KeyValuePair<string, string> entry in SendingMessages)
            {
                Console.Out.WriteLine(entry.Value);
            }
        }





    }
    public class QueueAndRoutingKey
    {
        public string Queue { get; set; }
        public string RoutingKey { get; set; }
    }
}
