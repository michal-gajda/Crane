namespace Crane.Infrastructure.MassTransit.ConsumerDefinitions;

using Crane.Infrastructure.MassTransit.Consumers;
using global::MassTransit;

internal sealed class SendFormConsumerDefinition : ConsumerDefinition<SendFormConsumer>
{
    public SendFormConsumerDefinition()
    {
        EndpointName = "send-form";

        ConcurrentMessageLimit = 1;
    }
}
