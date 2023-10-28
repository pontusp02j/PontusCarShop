using System.Threading.Channels;

namespace Core.Queues
{
    public interface INewCarsForSaleQueue
    {
        Task<bool> AnyNewCarsInQueToReadAsync();
        bool TryRead(out string modelName);
        ValueTask WriteAsync(string modelName);
    }

    public class NewCarsForSaleQueue : INewCarsForSaleQueue
    {
        private readonly Channel<string> _queue;

        public NewCarsForSaleQueue()
        {
            _queue = Channel.CreateUnbounded<string>();
        }

        public async Task<bool> AnyNewCarsInQueToReadAsync()
        {
            return await _queue.Reader.WaitToReadAsync();
        }

        public async ValueTask WriteAsync(string modelName)
        {
            await _queue.Writer.WriteAsync(modelName);
        }

        public bool TryRead(out string modelName)
        {
            return _queue.Reader.TryRead(out modelName!);
        }
    }
}
