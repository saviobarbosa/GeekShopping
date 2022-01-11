using System;

namespace GeeekShopping.MessageBus
{
    public class BaseMessage
    {
        public long Id { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}
