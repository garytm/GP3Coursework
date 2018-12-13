using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class DBManager : MonoBehaviour
{
    //connection string which is used to tell the database where the files 
    //that are referenced in code are located
    private string connectionString;
    public GameObject scorePF;
    public Transform scoresParent;
    public InputField enterName;
    public int topFive;
    //an instance of the highscores 
    private List<Highscores> highScores = new List<Highscores>();
    void Start ()
    {
        //telling the connectionString that it is opening a file
        //and passing in the file path
        connectionString = "URI=file:" + Application.dataPath + "/highscores.db";
        CreateTable();
        DisplayScores();
    }

	void Update ()
    {
		
	}
    public void EnterName()
    {
        //do not allow empty strings to be entered as highscores
        if (enterName.text != string.Empty)
        {
            InsertScores(enterName.text, Player.score);
            enterName.text = string.Empty;
            DisplayScores();
        }
    }
    void CreateTable()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string SQLQuery = String.Format("CREATE TABLE IF NOT EXISTS highscores (PlayerID INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, Name TEXT, Score INTEGER)");
                dbCommand.CommandText = SQLQuery;
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
    void InsertScores(string playerName, int playerScore)
    {
        //a using statement allows the connection to be made without the need
        //to dispose of the connection every time it is no longer in use
        //think of it as having .Dispose by default
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //open to access the data
            dbConnection.Open();
            //by using the connection which was already created (dbConnection)
            //I've created a command which allows the connection to be used and
            //accessed for coding
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                //string.format allows variables to be passed into the string and changed
                string SQLQuery = String.Format("INSERT INTO highscores(Name, Score) VALUES ('{0}', '{1}')", playerName, playerScore);
                //give the command the command text for SQL queries
                dbCommand.CommandText = SQLQuery;
                //this is required when executing INTO the database
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
    void RemoveScores(int playerID)
    {
        //a using statement allows the connection to be made without the need
        //to dispose of the connection every time it is no longer in use
        //think of it as having .Dispose by default
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //open to access the data
            dbConnection.Open();
            //by using the connection which was already created (dbConnection)
            //I've created a command which allows the connection to be used and
            //accessed for coding
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                //string.format allows variables to be passed into the string and changed
                string SQLQuery = String.Format("DELETE FROM highscores WHERE PlayerID = '{0}'", playerID);
                //give the command the command text for SQL queries
                dbCommand.CommandText = SQLQuery;
                //this is required when executing INTO the database
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
    void GetHighScores()
    {
        //clear the highscores list initially
        highScores.Clear();
        //a using statement allows the connection to be made without the need
        //to dispose of the connection every time it is no longer in use
        //think of it as having .Dispose by default
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //open to access the data
            dbConnection.Open();
            //by using the connection which was already created (dbConnection)
            //I've created a command which allows the connection to be used and
            //accessed for coding
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string SQLQuery = "SELECT * FROM highscores";
                //give the command the command text for SQL queries
                dbCommand.CommandText = SQLQuery;
                //this is used to execute the previous command and adds the
                //result into the reader
                using (IDataReader reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //passing in highscores based on table positions in the DB
                        highScores.Add(new Highscores(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
                    }
                    //close the connections to avoid infinites
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
        highScores.Sort();
    }
    void DisplayScores()
    {
        //using an array with tags to stop scores being entered continuously
        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score"))
        {
            Destroy(score);
        }
        GetHighScores();
        //looping through highscore list
        for (int i = 0; i < topFive; i++)
        {
            if (i <= highScores.Count - 1)
            {
                GameObject obj = Instantiate(scorePF);
                //passing highscores from the DB into the Highscores script
                Highscores hScore = highScores[i];

                obj.GetComponent<HighScoreScript>().SetScore(hScore.Name, hScore.Score.ToString(), "#" + (i + 1).ToString());
                obj.transform.SetParent(scoresParent);
                obj.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
    }
}
