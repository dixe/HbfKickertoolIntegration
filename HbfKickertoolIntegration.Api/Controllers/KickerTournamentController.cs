using HbfKickertoolIntegration.Api.Models;
using HbfKickertoolIntegration.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace HbfKickertoolIntegration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KickerTournamentController : ControllerBase
    {
        private TournamentService service;
        public KickerTournamentController()
        {
             service = new TournamentService();
        }

        // GET: api/KickerTournament
        [HttpGet]
        [Route("CurrentTournament")]
        public string GetCurrentTournament()
        {

            if(KickerToolManagement.IsKickertoolRunning())
            {
                return "{\"kickertoolrunning\" : true }";
            }
            //Check that kicker tool is not runnig, if it is send error
            return JsonConvert.SerializeObject(service.LoadCurrent());
        }

        [HttpGet]
        [Route("CurrentTournamentFinished")]
        public string GetCurrentTournamentFinished()
        {

            if (KickerToolManagement.IsKickertoolRunning())
            {
                return "{\"kickertoolrunning\" : true }";
            }

            var tour = service.LoadCurrent();

            return $"{{\"currenttournamentfinished\" : {service.TournamentFinished(tour)} }}";
        }



        // POST: api/KickerTournament
        [HttpPost]
        [Route("GenerateNewTournament")]
        public string GenerateNewTournament([FromBody] NewTournament newT)
        {
            if (KickerToolManagement.IsKickertoolRunning())
            {
                return "{\"kickertoolrunning\" : true }";
            }

            //TODO also make sure that we have it is finished and saved
            var currentFinished = service.CurrentTournamentFinished();

            if (!currentFinished)
            {
                KickerToolManagement.EnsureKickertoolRunning();
                return "{ \"tournamentfinished\" : false \"}";
            }

            var newTour = service.CreateNew(newT.Teams.Select(x => (x.Player1, x.Player2)).ToList());
            service.SaveTournementAsCurrent(newTour);
            KickerToolManagement.EnsureKickertoolRunning();


            return "{ \"ok\" : true}";
        }


        [HttpPost]
        [Route("StartKickerTool")]
        public void StartKickerTool()
        {
            KickerToolManagement.EnsureKickertoolRunning();
        }

        // DELETE: api/ApiWithActions/5
        [HttpPost]
        [Route("DeleteCurrentTournament")]
        public string DeleteCurrentTournament()
        {
            if (KickerToolManagement.IsKickertoolRunning())
            {
                return "{\"kickertoolrunning\" : true }";
            }

            service.DeleteCurrentTournament();

            return "{\"ok\" : true }";
        }
    }
}
