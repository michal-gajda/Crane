namespace Crane.Infrastructure.MassTransit.Consumers;

using global::MassTransit;
using Iacula.Shared;
using Microsoft.Extensions.Logging;

internal sealed class SendFormConsumer(ILogger<SendFormConsumer> logger) : IConsumer<SendForm>
{
    public Task Consume(ConsumeContext<SendForm> context)
    {
        logger.LogInformation(
            "Received SendForm message. MessageId: {MessageId}, FormId: {FormId}, Payload: {Payload}",
            context.MessageId,
            context.Message.Id,
            context.Message.Payload);

        logger.LogInformation(
            "Processed SendForm message. MessageId: {MessageId}, FormId: {FormId}",
            context.MessageId,
            context.Message.Id);

        return Task.CompletedTask;
    }
}
