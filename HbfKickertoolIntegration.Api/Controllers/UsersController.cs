using HbfKickertoolIntegration.Api.Models;
using HbfKickertoolIntegration.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace HbfKickertoolIntegration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserService service;
        public UsersController()
        {
             service = new UserService();
        }

        [HttpGet]
        [Route("GetPlayerSuggestions")]
        public PlayerSuggestions GeGetPlayerSuggestionstPlayer(string name)
        {
            if(name == null)
            {
                return new PlayerSuggestions();
            }
            return new PlayerSuggestions
            {
                Suggestions = name.Select(x =>
                    new PlayerSuggestion
                    {
                        Name = x.ToString() + "_sug",
                        Number = "4432133SUG"
                    }).ToList()        
            };
        }

        [HttpGet]
        [Route("GetTeam")]
        public Team GetTeam(string number1, string number2)
        {
            return new Team
            {
                Player1Name = number1,
                Player1Number = number1,
                Player2Name = number2,
                Player2Number = number2,

            };
      }
    }
}
