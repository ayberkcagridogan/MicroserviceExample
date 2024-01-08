using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms")]
    [ApiController]
    public class PlatformsController : ControllerBase{
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper; 

        public PlatformsController(ICommandRepo repo , IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]

        public ActionResult<IEnumerable<PlatformReadDto>> GetAllPlatform(){

            Console.WriteLine("==> Get All Platfrom from CommandService");

            var platformItems = _repo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }
        [HttpPost]
        public ActionResult Test(){

            Console.WriteLine("==> Inbound Post a Command Service");

            return Ok("Inbound test of from Platforms Controller");
        }
    }
}