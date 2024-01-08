using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
           var eventType = DetermineEvent(message);


            switch (eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:
                    break;
            } 
        }

        private void addPlatform(string platformPublishedMessage)
        {
            using(var scope = _scopeFactory.CreateScope()){

                var repo =  scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);     

            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);

                if(!repo.PlatformExits(platform.ExternalId)){
                    
                    repo.CreatePlatform(platform);
                    repo.SaveChanges();
                    Console.WriteLine($"==> Platform added... PlatformExternalId= {platform.ExternalId}");
                }
                else{

                    Console.WriteLine($"==> Platform already exists... PlatformExternalId= {platform.ExternalId}");
                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"==> Platform could not been added to CommanderDB {ex.Message}");
            }

                
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("==> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("==> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("==> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}