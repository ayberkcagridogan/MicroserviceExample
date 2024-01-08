using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using PlatformService;

namespace CommandsService.SyncDataService.Grpc{

    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PlatformDataClient(IConfiguration configuration , IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
        }
        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            Console.WriteLine($"==> Calling gPRC Service {_configuration["GprcPlatform"]}");           
            var channel = GrpcChannel.ForAddress(_configuration["GprcPlatform"]);
         
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();
            try
            {
                var reply = client.GetAllPlatforms(request);

                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not call GRPC Server {ex.Message}");
                return null;               
            }
        }
    }
}