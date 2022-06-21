using EasyNetQ;
using Microsoft.Extensions.Options;

namespace Common.API.MessageBroker;

public interface IMessageBroker
{
    Task PublishAsync<T>(T msg, string topic) where T : class;
    Task<ISubscriptionResult> SubscribeAsync<T>(string topic, Func<T, CancellationToken, Task> callback) where T : class;
    void Dispose();
}

public class MessageBrokerBase : IMessageBroker
{
    protected IBus _brokerClient;
    protected MessageBrokerOptions _brokerOptions;

    public MessageBrokerBase(IOptions<MessageBrokerOptions> brokerOptions)
    {
        _brokerOptions = brokerOptions.Value;
    }

    public async Task PublishAsync<T>(T msg, string topic) where T : class
    {
        await _brokerClient.PubSub.PublishAsync(msg, topic);
    }

    public async Task<ISubscriptionResult> SubscribeAsync<T>(string topic, Func<T, CancellationToken, Task> callback) where T : class
    {
        return await _brokerClient.PubSub.SubscribeAsync(
            _brokerOptions.SubscriptionId,
            callback,
            msg => msg.WithTopic(topic)
        );
    }

    public void Dispose()
    {
        _brokerClient.Dispose();
    }
}

public class MessageBroker : MessageBrokerBase, IMessageBroker
{
    public MessageBroker(IOptions<MessageBrokerOptions> brokerOptions)
        : base(brokerOptions)
    {
        _brokerClient = RabbitHutch.CreateBus(_brokerOptions.ConnectionString);
    }
}

