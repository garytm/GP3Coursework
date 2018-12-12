using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;

    public class GameStates : MonoBehaviour
    {
        private string dbPath;
        public Text nameText;
        public Text score;
        public Text displayHighScore;
    public static bool activate;
    private Rigidbody rigidBody;

    private void Start()
        {
            dbPath = "URI=file:" + Application.persistentDataPath + "/exampleDatabase.db";
            CreateSchema();
            
            GetHighScores(10);
        }

        public void CreateSchema()
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "CREATE TABLE IF NOT EXISTS 'high_score' ( " +
                                      "  'id' INTEGER PRIMARY KEY, " +
                                      "  'name' TEXT NOT NULL, " +
                                      "  'score' INTEGER NOT NULL" +
                                      ");";

                    var result = cmd.ExecuteNonQuery();
                    Debug.Log("create schema: " + result);
                }
            }
        }

        public void InsertScore()
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO high_score (name, score) " +
                                      "VALUES (@Name, @Score);";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Name",
                        Value = nameText
                    });

                cmd.Parameters.Add(new SqliteParameter
                {
                    ParameterName = "Score",
                    Value = Player.score
                    });

                    var result = cmd.ExecuteNonQuery();
                    Debug.Log("insert score: " + result);
                }
            }
        }

        public void GetHighScores(int limit)
        {
            using (var conn = new SqliteConnection(dbPath))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM high_score ORDER BY score DESC LIMIT @Count;";

                    cmd.Parameters.Add(new SqliteParameter
                    {
                        ParameterName = "Count",
                        Value = limit
                    });

                    Debug.Log("scores (begin)");
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var highScoreName = reader.GetString(1);
                        var score = reader.GetInt32(2);
                        var text = string.Format("{0}: {1} [#{2}]", nameText, score, id);
                        Debug.Log(text);
                    }
                    Debug.Log("scores (end)");
                }
            }
        }
}
    