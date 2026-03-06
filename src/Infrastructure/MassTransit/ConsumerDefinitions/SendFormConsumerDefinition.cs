namespace Crane.Infrastructure.MassTransit.ConsumerDefinitions;

using Crane.Infrastructure.EntityFramework;
using Crane.Infrastructure.MassTransit.Consumers;
using global::MassTransit;

internal sealed class SendFormConsumerDefinition : ConsumerDefinition<SendFormConsumer>
{
    public SendFormConsumerDefinition()
    {
        EndpointName = "send-form";

        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SendFormConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<CraneDbContext>(context);
    }
}
