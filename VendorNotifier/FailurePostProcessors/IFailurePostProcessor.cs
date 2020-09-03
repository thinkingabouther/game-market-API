using Consumer.DTOs;

namespace Consumer.FailurePostProcessors
{
    public interface IFailurePostProcessor
    {
        void Process(Message message);
    }
}