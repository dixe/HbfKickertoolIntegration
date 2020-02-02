using HbfKickertoolIntegration.Standalone.Datalayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HbfKickertoolIntegration.Core
{
	public class TournamentService
	{
		ITournamentRepository repo;
		public TournamentService()
		{
			repo = new TournamentRepository();
		}

		public Tournament CreateNew(List<(string p1, string p2)> teams)
		{

			var t = BlankTournament();
			t.id = Guid.NewGuid().ToString();
			t.name = "Tournament - " + DateTime.Now.ToShortDateString();
			t.ko = null;
			t.numRounds = 0;
			t.groups = null;
			t.plays = null;

			foreach (var team in teams)
				t.teams = teams.Select(x =>

				   new Team3
				   {
					   id = Guid.NewGuid().ToString(),
					   name = x.p1 + " - " + x.p2,
					   players = new[]
					   {
						new Player
						{
							id = Guid.NewGuid().ToString(),
							name = x.p1
						},
						new Player
						{
							id = Guid.NewGuid().ToString(),
							name = x.p2
						}
					   }
				   }).ToArray();
			return t;
		}
		
		private Tournament BlankTournament()
		{
			return JsonConvert.DeserializeObject<Tournament>(BlankTournamentString);
		}

		public bool TournamentFinished(Tournament t)
		{
			if (null == t)
				return true;
			return t.ko?.levels?.FirstOrDefault(x => x.name == "1/1")?.plays.FirstOrDefault()?.valid ?? false;
		}

		public bool CurrentTournamentFinished()
		{
			var tour = repo.LoadCurrentTournement();

			return TournamentFinished(tour);
		}

		public Tournament LoadCurrent()
		{
			return repo.LoadCurrentTournement();
		}


		public void SaveTournementAsCurrent(Tournament t)
		{
			repo.SaveTournementAsCurrent(t);
		}
		
		public void DeleteCurrentTournament()
		{
			repo.DeleteCurrentTournament();
		}

		private string BlankTournamentString =
			"{\"id\":\"c06d80b0-cd20-4b9c-879b-b10d12666e41\",\"name\":\"GeneratedT 02-Feb-20\",\"created\":\"2020-02-02T13:07:27.334Z\",\"groups\":null,\"players\":null,\"teams\":null,\"plays\":null,\"ko\":null,\"mode\":{\"id\":5},\"numRounds\":0,\"options\":{\"id\":\"607d3998-73b1-42d7-b19b-ab97d6abf316\",\"pointsWin\":2,\"pointsDraw\":1,\"numTables\":5,\"twoAhead\":false,\"fairShuffle\":true,\"disciplines\":[{\"id\":\"efd43596-9056-4d40-d16a-69d9ab829fa0\",\"name\":\"Disziplin 1\",\"numPoints\":7,\"numSets\":1,\"twoAhead\":true}],\"tables\":[{\"id\":\"e5403a0d-d2df-43da-8b86-14f87d3ac6ac\",\"name\":\"1\"},{\"id\":\"3d278738-60ca-4a57-ccbc-54c34ba54bd1\",\"name\":\"2\"},{\"id\":\"6048b655-ca81-4928-ba62-c17a99f1c433\",\"name\":\"3\"},{\"id\":\"08daa41e-747d-42bd-935e-576813131610\",\"name\":\"4\"},{\"id\":\"c0841930-832f-43bd-bcd1-64c82ff4feb2\",\"name\":\"5\"}],\"hasDisciplines\":false,\"maxLostGames\":3,\"numPoints\":7,\"numSets\":1,\"tableMeta\":[{\"id\":21,\"header\":\"Leben\",\"property\":\"lives\",\"visible\":true,\"hidden\":true,\"renderer\":\"lms-lives\",\"ignoreSort\":false},{\"id\":20,\"header\":\"ØP*\",\"property\":\"corrected_points_per_game\",\"visible\":true,\"hidden\":true,\"renderer\":\"corrected\",\"ignoreSort\":false},{\"id\":19,\"header\":\"ØP\",\"property\":\"points_per_game\",\"visible\":false,\"hidden\":false,\"renderer\":\"float\",\"ignoreSort\":true},{\"id\":18,\"header\":\"Pkt\",\"property\":\"points\",\"visible\":true,\"hidden\":false,\"renderer\":null,\"ignoreSort\":false},{\"id\":17,\"header\":\"BH₁\",\"property\":\"bh1\",\"visible\":false,\"hidden\":false,\"renderer\":\"float\",\"ignoreSort\":true},{\"id\":16,\"header\":\"BH₂\",\"property\":\"bh2\",\"visible\":false,\"hidden\":false,\"renderer\":\"float\",\"ignoreSort\":true},{\"id\":15,\"header\":\"SB\",\"property\":\"sb\",\"visible\":false,\"hidden\":false,\"renderer\":null,\"ignoreSort\":true},{\"id\":14,\"header\":\"Lost\",\"property\":\"losts\",\"visible\":false,\"hidden\":false,\"renderer\":null,\"ignoreSort\":true},{\"id\":13,\"header\":\"Won\",\"property\":\"wins\",\"visible\":false,\"hidden\":false,\"renderer\":null,\"ignoreSort\":true},{\"id\":12,\"header\":\"D±\",\"property\":\"dis_diff\",\"visible\":true,\"hidden\":true,\"renderer\":null,\"ignoreSort\":false},{\"id\":11,\"header\":\"D+\",\"property\":\"dis_won\",\"visible\":false,\"hidden\":true,\"renderer\":null,\"ignoreSort\":true},{\"id\":10,\"header\":\"D-\",\"property\":\"dis_lost\",\"visible\":false,\"hidden\":true,\"renderer\":null,\"ignoreSort\":true},{\"id\":9,\"header\":\"S±\",\"property\":\"set_diff\",\"visible\":true,\"hidden\":true,\"renderer\":null,\"ignoreSort\":false},{\"id\":8,\"header\":\"S+\",\"property\":\"sets_won\",\"visible\":false,\"hidden\":true,\"renderer\":null,\"ignoreSort\":true},{\"id\":7,\"header\":\"S-\",\"property\":\"sets_lost\",\"visible\":false,\"hidden\":true,\"renderer\":null,\"ignoreSort\":true},{\"id\":6,\"header\":\"T±\",\"property\":\"goal_diff\",\"visible\":true,\"hidden\":false,\"renderer\":null,\"ignoreSort\":false},{\"id\":5,\"header\":\"T-\",\"property\":\"goals_in\",\"visible\":false,\"hidden\":false,\"renderer\":null,\"ignoreSort\":true},{\"id\":4,\"header\":\"T+\",\"property\":\"goals\",\"visible\":false,\"hidden\":false,\"renderer\":null,\"ignoreSort\":true},{\"id\":3,\"header\":\"Sp.\",\"property\":\"games\",\"visible\":true,\"hidden\":false,\"renderer\":null,\"ignoreSort\":true},{\"id\":2,\"header\":\"Team\",\"property\":\"getName\",\"visible\":true,\"hidden\":false,\"renderer\":\"name\",\"ignoreSort\":true},{\"id\":1,\"header\":\"Spieler\",\"property\":\"name\",\"visible\":true,\"hidden\":true,\"renderer\":\"name\",\"ignoreSort\":true}]},\"nameType\":\"team\",\"user\":\"\",\"container\":\"\",\"version\":\"1.0\"}";
	}
}
