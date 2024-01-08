using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        //For Platform
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExits(int platfromId);
        bool ExternalPlatformId(int externalPlatformId);

        //For Command
        IEnumerable<Command> GetCommandsForPlatform(int platfromId);
        Command GetCommand(int platfromId, int commandId);
        void CreateCommand(int platfromId, Command command);
    }
}