using Google.Protobuf.WellKnownTypes;
using gRPC.DataAccess.Models;
using gRPC.DataAccess.Repositories;
using gRPC.server;
using Grpc.Core;
using Empty = gRPC.server.Empty;


namespace Grpc.server.Services;

public class CommunicationService(IGrpcMessageRepository grpcMessageRepository) : Communication.CommunicationBase
{
    private readonly IGrpcMessageRepository _repository = grpcMessageRepository;

    public override async Task<MessageResponse> SendMessage(Message request, ServerCallContext context)
    {
        try
        {
            // Create a new Message entity based on the request
            var messageEntity = new MessageModel()
            {
                Sender = request.Sender,
                Recipient = request.Recipient,
                Content = request.Content,
                TimeStamp = DateTime.UtcNow
            };

            // Add the message entity to the database context
            await _repository.AddAsync(messageEntity);


            // Return a success response
            return new MessageResponse
            {
                MessageId = messageEntity.Id.ToString(),
                Status = "Sent"
            };
        }
        catch (Exception ex)
        {
            // Handle any exceptions and return an error response
            Console.WriteLine($"Error sending message: {ex.Message}");
            return new MessageResponse
            {
                Status = "Failed to send message" + ex.Message
            };
        }
    }

    public override async Task<Message> GetOneMessage(MessageFilter request, IServerStreamWriter<Message> responseStream, ServerCallContext context)
    {
        var message = new Message();
        try
        {
            // Retrieve the message from the database based on the provided filter
            var messageEntity = await _repository.GetByIdAsync(Guid.Parse(request.MessageId));

            if (messageEntity is not null)
            {
                // Convert the MessageEntity to a Message and stream it to the client
                message.MessageId = messageEntity.Id.ToString();
                message.Sender = messageEntity.Sender;
                message.Recipient = messageEntity.Recipient;
                message.Content = messageEntity.Content;
                message.TimeStamp = messageEntity.TimeStamp.ToTimestamp();

                // Stream the message back to the client
                await responseStream.WriteAsync(message);
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions and send an error response
            Console.WriteLine($"Error retrieving message: {ex.Message}");
            message.MessageId = "";
            message.Sender = "";
            message.Recipient = "";
            message.Content = "Message not found";
            message.TimeStamp = Timestamp.FromDateTime(DateTime.UtcNow);

            await responseStream.WriteAsync(message);
        }
        return message;
    }

    public override async Task<Messages> GetAllMessages(Empty request, IServerStreamWriter<Messages> responseStream, ServerCallContext context)
    {
        var messagesList = new Messages();
        try
        {
            var messages = await _repository.GetAllAsync();
            if (messages is not null)
            {
                messagesList.Messages_.AddRange(messages.Select(messageEntity => new Message
                {
                    MessageId = messageEntity.Id.ToString(),
                    Sender = messageEntity.Sender,
                    Recipient = messageEntity.Recipient,
                    Content = messageEntity.Content,
                    TimeStamp = messageEntity.TimeStamp.ToTimestamp()
                }));
            }
            await responseStream.WriteAsync(messagesList);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving messages: {ex.Message}");
            throw new RpcException(new Status(StatusCode.Internal, "no message found"));
        }
        return messagesList;
    }

    public override async Task<Message> EditMessage(Message request, ServerCallContext context)
    {
        try
        {
            var message = new MessageModel()
            {
                Id = Guid.Parse(request.MessageId),
                Sender = request.Sender,
                Recipient = request.Recipient,
                Content = request.Content,
                TimeStamp = request.TimeStamp.ToDateTime()
            };

            return new Message
            {
                MessageId = message.Id.ToString(),
                Sender = message.Sender,
                Recipient = message.Recipient,
                Content = message.Content,
                TimeStamp = Timestamp.FromDateTime(message.TimeStamp)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<Empty> DeleteMessage(MessageFilter request, ServerCallContext context)
    {
        try
        {
            await _repository.DeleteAsync(Guid.Parse(request.MessageId));
            return new Empty();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}





