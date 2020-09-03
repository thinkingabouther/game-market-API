using System.Threading.Tasks;
using Consumer.DTOs;
using RabbitMQ.Client.Events;

namespace Consumer.Requesters
{
    public interface IRequester
    {
        Task<bool> TryRequest(Message message);
    }
}