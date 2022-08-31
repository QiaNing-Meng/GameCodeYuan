using System;
using System.Collections.Generic;
using TouchSocket.Core.ByteManager;
using TouchSocket.Core.Serialization;
using TouchSocket.Sockets;

namespace Protocol
{
    public class TS_MessageDistribute<T> : TS_Singleton<TS_MessageDistribute<T>>
    {
        public class MessageArgs
        {
            public T sender;
            public TS_Message message;
        }

        public Queue<MessageArgs> queue_messages = new Queue<MessageArgs>();

        public delegate void MessageHandler<TM>(T sender, TM message);
        private Dictionary<string, System.Delegate> messageHandlers = new Dictionary<string, System.Delegate>();

        public void Queue_Msg()
        {
            if (this.queue_messages.Count == 0) return;

            MessageArgs package = this.queue_messages.Dequeue();
            if (package.message.MessageRequest != null)
                TS_MessageDispatch<T>.Instance.Dispatch(package.sender, package.message.MessageRequest);
            if (package.message.MessageResponse != null)
                TS_MessageDispatch<T>.Instance.Dispatch(package.sender, package.message.MessageResponse);
        }

        public void Distribute(T client,ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            byteBlock.Position = 0;
            TS_Message message = SerializeConvert.FastBinaryDeserialize<TS_Message>(byteBlock.Buffer);
            byteBlock.Dispose();

            if (message == null) return;

            queue_messages.Enqueue(new MessageArgs() { sender = client , message = message }) ;

        }


        public void Subscribe<Tm>(MessageHandler<Tm> messageHandler)
        {
            string type = typeof(Tm).Name;
            if (!messageHandlers.ContainsKey(type))
            {
                messageHandlers[type] = null;
            }
            messageHandlers[type] = (MessageHandler<Tm>)messageHandlers[type] + messageHandler;
        }
        public void Unsubscribe<Tm>(MessageHandler<Tm> messageHandler)
        {
            string type = typeof(Tm).Name;
            if (!messageHandlers.ContainsKey(type))
            {
                messageHandlers[type] = null;
            }
            messageHandlers[type] = (MessageHandler<Tm>)messageHandlers[type] - messageHandler;
        }
        public void RaiseEvent<Tm>(T sender, Tm msg)
        {
            string key = msg.GetType().Name;
            if (messageHandlers.ContainsKey(key))
            {
                MessageHandler<Tm> handler = (MessageHandler<Tm>)messageHandlers[key];
                if (handler != null)
                {
                    try
                    {
                        handler(sender, msg);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("Message handler exception:{0}, {1}, {2}, {3}", ex.InnerException, ex.Message, ex.Source, ex.StackTrace);
                        //if (ThrowException)
                        //    throw ex;
                    }
                }
                else
                {
                    Console.WriteLine("No handler subscribed for {0}" + msg.ToString());
                }
            }
        }
    }


    public class TS_MessageDispatch<T> : TS_Singleton<TS_MessageDispatch<T>>
    {
        public void Dispatch(T sender, MessageRequest message)
        {
            if (message.LoginRequest != null) { TS_MessageDistribute<T>.Instance.RaiseEvent(sender, message.LoginRequest); }
        }

        public void Dispatch(T sender, MessageResponse message)
        {
            if (message.LoginResponse != null) { TS_MessageDistribute<T>.Instance.RaiseEvent(sender, message.LoginResponse); }
        }
    }
}
