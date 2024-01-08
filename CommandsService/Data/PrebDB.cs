using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataService.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data{

    public static class PrebDB{

        public static void PrebPopulation(IApplicationBuilder app){

            using(var serviceScope = app.ApplicationServices.CreateScope()){
                
                
                var grpcPlatformService = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcPlatformService.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>() , platforms);
            }
        }

        private static void SeedData(ICommandRepo repo , IEnumerable<Platform> platforms){
            
            Console.WriteLine("Seeding Data");
            foreach(var platform in platforms){

                if(!repo.PlatformExits(platform.ExternalId)){
                    repo.CreatePlatform(platform);
                }
            }
            repo.SaveChanges();
        }
    }
}