using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCommand(int platfromId, Command command)
        {
            if(command == null){

                throw new ArgumentNullException(nameof(command));
            }
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            command.PlatformId = platfromId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null){

                throw new ArgumentNullException(nameof(plat));
            }
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            _context.Platforms.Add(plat);
        }

        public bool ExternalPlatformId(int externalPlatformId)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return _context.Platforms.Any(p => p.ExternalId == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
             return _context.Platforms.ToList();
        }

        public Command GetCommand(int platfromId, int commandId)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return _context.Commands
                            .Where(c => c.Id == commandId && c.PlatformId == platfromId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platfromId)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return _context.Commands
                                .Where(c => c.PlatformId == platfromId);
        }

        public bool PlatformExits(int platfromId)
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            return _context.Platforms.Any(p => p.Id == platfromId);
        }

        public bool SaveChanges()
        {
             return _context.SaveChanges() >= 0;      
        }
    }
}