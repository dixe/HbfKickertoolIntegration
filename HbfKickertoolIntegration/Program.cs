// HbfKickertoolIntegration.Program
using HbfKickertoolIntegration.Standalone.Datalayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

internal class Program
{
	private static void Main(string[] args)
	{
		ITournamentRepository repo = new TournamentRepository();
		Tournament tournament2 = repo.LoadCurrentTournement();
		if (TournamentFinished(tournament2))
		{
			tournament2 = CreateNew(tournament2, GeneratePlayers());
			repo.SaveTournementAsCurrent(tournament2);
		}
		Process started = Process.Start("C:\\Program Files (x86)\\Kickertool\\Kickertool.exe");
		bool watch = true;
		bool turnamentRunning2 = false;
		while (watch)
		{
			Process[] processes = Process.GetProcesses();
			turnamentRunning2 = ((IEnumerable<Process>)processes).Any((Func<Process, bool>)((Process x) => x.ProcessName.ToLower() == "kickertool"));
			if (turnamentRunning2)
			{
				Thread.Sleep(1000);
			}
			if (!turnamentRunning2)
			{
				tournament2 = repo.LoadCurrentTournement();
				if (tournament2.ko != null && ((tournament2.ko?.levels?.FirstOrDefault((Func<Level, bool>)((Level x) => x.name == "1/1"))?.plays.FirstOrDefault())?.valid ?? false))
				{
					int debug = 2;
				}
			}
		}
	}

	private static void EnsureKickertoolRunning()
	{
		Process[] processes = Process.GetProcesses();
		if (!((IEnumerable<Process>)processes).Any((Func<Process, bool>)((Process x) => x.ProcessName.ToLower() == "kickertool")))
		{
			Process started = Process.Start("C:\\Program Files (x86)\\Kickertool\\Kickertool.exe");
		}
	}

	private static bool TournamentFinished(Tournament t)
	{
		return (t?.ko?.levels?.FirstOrDefault((Func<Level, bool>)((Level x) => x.name == "1/1"))?.plays.FirstOrDefault())?.valid ?? false;
	}

	private static List<(string, string)> GeneratePlayers()
	{
		List<(string, string)> res = new List<(string, string)>();
		for (int i = 0; i < 16; i++)
		{
			res.Add(($"{i}-a", $"{i}-b"));
		}
		return res;
	}

	public static Tournament CreateNew(Tournament t, List<(string p1, string p2)> teams)
	{
		t.name = "GeneratedT " + DateTime.Now.ToShortDateString();
		t.ko = null;
		t.numRounds = 0;
		t.groups = null;
		t.plays = null;
		t.teams = ((IEnumerable<(string, string)>)teams).Select((Func<(string, string), Team3>)delegate ((string p1, string p2) x)
		{
			Team3 team = new Team3
			{
				id = Guid.NewGuid().ToString(),
				name = x.p1 + " - " + x.p2
			};
			object[] array2 = team.players = new Player[2]
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
			};
			return team;
		}).ToArray();
		return t;
	}

	private static void test()
	{
		string dbpath = "Data source=C:\\Users\\PC\\AppData\\Local\\Kickertool\\Local Storage\\file__0.localstorage";
		using (SQLiteConnection con = new SQLiteConnection(dbpath, parseViaFramework: true))
		{
			con.Open();
			string stm = "select value from  ItemTable where key = 'kickern.game'; ";
			byte[] val = new byte[10];
			using (SQLiteCommand sQLiteCommand = new SQLiteCommand(stm, con))
			{
				SQLiteDataReader reader = sQLiteCommand.ExecuteReader();
				while (reader.Read())
				{
					val = (byte[])reader[0];
				}
			}
			string data2 = Encoding.GetEncoding("UTF-16").GetString(val);
			data2 = data2.Replace("localtest", "LocalCODETest");
			byte[] binData = Encoding.GetEncoding("UTF-16").GetBytes(data2);
			string writeSql = "UPDATE ItemTable SET value = @data where key = 'kickern.game'; ";
			using (SQLiteCommand cmd = new SQLiteCommand(writeSql, con))
			{
				cmd.Parameters.Add(new SQLiteParameter("data", binData));
				int updated = cmd.ExecuteNonQuery();
			}
			Console.WriteLine("Hello World!");
		}
	}
}
