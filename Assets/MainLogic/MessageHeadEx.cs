using FrameMessage;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FrameLockStepServer.Ex
{
    public static class MessageHeadEx
    {
        static MessageHeadEx()
        {
            foreach (var type in Enum.GetValues(typeof(MessageHeadServer.Types.Type)))
                messageHeadServers.Add((MessageHeadServer.Types.Type)type, new MessageHeadServer() { Value = (MessageHeadServer.Types.Type)type });
            foreach (var type in Enum.GetValues(typeof(MessageHeadClient.Types.Type)))
                messageHeadClients.Add((MessageHeadClient.Types.Type)type, new MessageHeadClient() { Value = (MessageHeadClient.Types.Type)type });
        }
        private static Dictionary<MessageHeadServer.Types.Type, MessageHeadServer> messageHeadServers = new Dictionary<MessageHeadServer.Types.Type, MessageHeadServer>();
        private static Dictionary<MessageHeadClient.Types.Type, MessageHeadClient> messageHeadClients = new Dictionary<MessageHeadClient.Types.Type, MessageHeadClient>();
        public static MessageHeadServer GetHead(this MessageHeadServer.Types.Type type)
        {
            return messageHeadServers[type];
        }
        public static MessageHeadClient GetHead(this MessageHeadClient.Types.Type type)
        {
            return messageHeadClients[type];
        }
        public static MessageHeadClient GetHeadClient(this Object body)
        {
            Type type = body.GetType();
            return Enum.Parse<MessageHeadClient.Types.Type>(type.Name).GetHead();
        }
        public static MessageHeadServer GetHeadServer(this Object body)
        {
            Type type = body.GetType();
            return Enum.Parse<MessageHeadServer.Types.Type>(type.Name).GetHead();
        }
    }
}
