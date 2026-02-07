using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Project3.Application.Services.Websocket
{
    public class WsSession
    {
        public WebSocket Socket { get; init; } = default!;
        public DateTime LastHeartbeat { get; set; }
    }
}
