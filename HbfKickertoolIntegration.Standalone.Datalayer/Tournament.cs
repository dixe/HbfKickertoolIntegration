using System;
using System.Collections.Generic;
using System.Text;

namespace HbfKickertoolIntegration.Standalone.Datalayer
{

    public class Tournament
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime created { get; set; }
        public Group[] groups { get; set; }
        public Player[] players { get; set; }
        public Team3[] teams { get; set; }
        public Play2[] plays { get; set; }
        public Ko ko { get; set; }
        public Mode mode { get; set; }
        public int numRounds { get; set; }
        public Options1 options { get; set; }
        public string nameType { get; set; }
        public string user { get; set; }
        public string container { get; set; }
        public string version { get; set; }
    }

    public class Ko
    {
        public string id { get; set; }
        public Level[] levels { get; set; }
        public Third third { get; set; }
        public int size { get; set; }
        public bool thirdPlace { get; set; }
        public bool _double { get; set; }
        public bool singlePlayer { get; set; }
        public Options options { get; set; }
    }

    public class Third
    {
        public string id { get; set; }
        public Play[] plays { get; set; }
        public string name { get; set; }
    }

    public class Play
    {
        public string id { get; set; }
        public int round { get; set; }
        public bool valid { get; set; }
        public bool firstOfRound { get; set; }
        public bool deactivated { get; set; }
    }

   

    public class Discipline
    {
        public string id { get; set; }
        public string name { get; set; }
        public int numPoints { get; set; }
        public int numSets { get; set; }
        public bool twoAhead { get; set; }
    }

    public class Table
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Tablemeta
    {
        public int id { get; set; }
        public string header { get; set; }
        public string property { get; set; }
        public bool visible { get; set; }
        public bool hidden { get; set; }
        public string renderer { get; set; }
        public bool ignoreSort { get; set; }
    }

    public class Level
    {
        public string id { get; set; }
        public Play1[] plays { get; set; }
        public string name { get; set; }
    }

    public class Play1
    {
        public string id { get; set; }
        public int round { get; set; }
        public bool valid { get; set; }
        public bool firstOfRound { get; set; }
        public Team1 team1 { get; set; }
        public Team2 team2 { get; set; }
        public long timeStart { get; set; }
        public bool deactivated { get; set; }
    }

    public class Team1
    {
        public string id { get; set; }
    }

    public class Team2
    {
        public string id { get; set; }
    }

    public class Mode
    {
        public int id { get; set; }
    }

    public class Options1
    {
        public string id { get; set; }
        public int pointsWin { get; set; }
        public int pointsDraw { get; set; }
        public int numTables { get; set; }
        public bool twoAhead { get; set; }
        public bool fairShuffle { get; set; }
        public Discipline1[] disciplines { get; set; }
        public Table1[] tables { get; set; }
        public bool hasDisciplines { get; set; }
        public int maxLostGames { get; set; }
        public int numPoints { get; set; }
        public int numSets { get; set; }
        public Tablemeta1[] tableMeta { get; set; }
    }


    public class Options
    {
        public string id { get; set; }
        public int pointsWin { get; set; }
        public int pointsDraw { get; set; }
        public int numTables { get; set; }
        public bool twoAhead { get; set; }
        public bool fairShuffle { get; set; }
        public Discipline[] disciplines { get; set; }
        public Table[] tables { get; set; }
        public bool hasDisciplines { get; set; }
        public int maxLostGames { get; set; }
        public int numPoints { get; set; }
        public int numSets { get; set; }
        public Tablemeta[] tableMeta { get; set; }
    }
    public class Discipline1
    {
        public string id { get; set; }
        public string name { get; set; }
        public int numPoints { get; set; }
        public int numSets { get; set; }
        public bool twoAhead { get; set; }
    }

    public class Table1
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Tablemeta1
    {
        public int id { get; set; }
        public string header { get; set; }
        public string property { get; set; }
        public bool visible { get; set; }
        public bool hidden { get; set; }
        public string renderer { get; set; }
        public bool ignoreSort { get; set; }
    }

    public class Group
    {
        public string id { get; set; }
        public Team[] teams { get; set; }
    }

    public class Team
    {
        public string id { get; set; }
    }

    public class Player
    {
        public string id { get; set; }
        public int weight { get; set; }
        public string name { get; set; }
        public bool removed { get; set; }
        public bool addedLater { get; set; }
        public int maxLives { get; set; }
        public int lastGameIndex { get; set; }
    }

    public class Team3
    {
        public string id { get; set; }
        public string name { get; set; }
        public object[] players { get; set; }
    }

    public class Play2
    {
        public string id { get; set; }
        public int round { get; set; }
        public bool valid { get; set; }
        public bool firstOfRound { get; set; }
        public Team11 team1 { get; set; }
        public Team21 team2 { get; set; }
        public long timeStart { get; set; }
        public bool deactivated { get; set; }
    }

    public class Team11
    {
        public string id { get; set; }
    }

    public class Team21
    {
        public string id { get; set; }
    }
}
