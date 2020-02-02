using Newtonsoft.Json;
using System.Data.SQLite;
using System.Text;

namespace HbfKickertoolIntegration.Standalone.Datalayer
{
    public class TournamentRepository : ITournamentRepository
    {
        string dbpath = @"Data source=C:\Users\PC\AppData\Local\Kickertool\Local Storage\file__0.localstorage";
        public TournamentRepository()
        {
        }

        public Tournament LoadCurrentTournement()
        {
            using var con = new SQLiteConnection(dbpath, true);
            con.Open();

            var stm = "select value from ItemTable where key = 'kickern.game'; ";

            byte[] val = new byte[10];
            using (var cmd = new SQLiteCommand(stm, con))
            {
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    val = (byte[])reader[0];
                }

                con.Close();
            }

            var data = Encoding.GetEncoding("UTF-16").GetString(val);
            
            return JsonConvert.DeserializeObject<Tournament>(data);
        }

       public void DeleteCurrentTournament()
        {
            var writeSql = "DELETE FROM ItemTable where key = 'kickern.game'; ";
            using (var con = new SQLiteConnection(dbpath, true))
            using (var cmd = new SQLiteCommand(writeSql, con))
            {
                con.Open();
                var updated = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void SaveTournementAsCurrent(Tournament tournament)
        {
            var binData = Encoding.GetEncoding("UTF-16").GetBytes(JsonConvert.SerializeObject(tournament));
            
            var writeSql = "DELETE FROM ItemTable where key = 'kickern.game'; INSERT INTO ItemTable VALUES('kickern.game', @data);";
            using ( var con = new SQLiteConnection(dbpath, true))
            using (var cmd = new SQLiteCommand(writeSql, con))
            {
                con.Open();
                cmd.Parameters.Add(new SQLiteParameter("data", binData));
                var updated = cmd.ExecuteNonQuery();
                con.Close();
            }
        }      
    }
}
