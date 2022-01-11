using System.Threading.Tasks;

namespace GeeekShopping.MessageBus
{
    public interface IMessageBus
    {
        Task PublicMessage(BaseMessage message, string queueName);
    }
}
