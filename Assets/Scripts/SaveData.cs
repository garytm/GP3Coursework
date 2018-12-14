using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using Mono.Data.Sqlite;
/// <summary>
/// THIS SCRIPT IS NON-FUNCTIONAL. RAN OUT OF TIME WHILE DEVELOPING THE GAME. LEFT IN TO SHOW WHAT I WAS AIMING FOR.
/// </summary>
public class SaveData : MonoBehaviour
{
    //connection string which is used to tell the database where the files 
    //that are referenced in code are located
    private string connectionString;
    //Connection to the SQLite DB
    private GameObject player;
    private GameObject enemy;
    //The input field for saving data
    public GameObject inputField;
    public Text enterText;
    void Start()
    {
        //telling the connectionString that it is opening a file
        //and passing in the file path
        connectionString = "URI=file:" + Application.dataPath + "/savedata.db";
        CreateTables();
    }
    public void SaveGame()
    {
        SavePlayerData();
        SaveEnemyData();
    }
    private void CreateTables()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand())
            {
                string SQLQuery = String.Format("CREATE TABLE IF NOT EXISTS playerData (id INTEGER PRIMARY KEY AUTOINCREMENT, name STRING, energy FLOAT, posX FLOAT, posY FLOAT, posZ FLOAT);");
                string SQLQuery2 = String.Format("CREATE TABLE IF NOT EXISTS enemyData (id INTEGER, energy FLOAT, posX FLOAT, posY FLOAT, posZ FLOAT);");
                string SQLQuery3 = String.Format("CREATE TABLE IF NOT EXISTS mushroomData (id INTEGER, maxDeathAge FLOAT, age FLOAT, posX FLOAT, posY FLOAT, posZ FLOAT, scaleX FLOAT, scaleY FLOAT, scaleZ FLOAT);");
                string SQLQuery4 = String.Format("CREATE TABLE IF NOT EXISTS blackHoleData (id INTEGER, posX FLOAT, posY FLOAT, posZ FLOAT);");
                dbCommand.CommandText = SQLQuery;
                dbCommand.CommandText = SQLQuery2;
                dbCommand.CommandText = SQLQuery3;
                dbCommand.CommandText = SQLQuery4;
                dbCommand.ExecuteScalar();
                dbConnection.Close();
            }
        }  
    }

    public void SavePlayerData()
    {
        //get the save file name from the input field
        string saveName = enterText.text;
        //get the energy of player
        float energy = GetComponent<Player>().energy;
        Vector3 position = player.transform.position;
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            IDbCommand cmd = dbConnection.CreateCommand();
            cmd.CommandText = "INSERT INTO playerData (name, energy, posX, posY, posZ) VALUES (@Name, @Energy, @PosX, @PosY, @PosZ);";
            cmd.Parameters.Add(new SqliteParameter("@Name", saveName));
            cmd.Parameters.Add(new SqliteParameter("@Energy", energy));
            cmd.Parameters.Add(new SqliteParameter("@PosX", position.x));
            cmd.Parameters.Add(new SqliteParameter("@PosY", position.y));
            cmd.Parameters.Add(new SqliteParameter("@PosZ", position.z));
            cmd.ExecuteNonQuery();
            dbConnection.Close();
        }
    }
    public void SaveEnemyData()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            IDbCommand cmd = dbConnection.CreateCommand();
            int id = GetSaveID();
            float energy = GetComponent<Enemy>().energy;
            Vector3 position = enemy.transform.position;
            cmd.CommandText = "INSERT INTO enemyData (id, energy, posX, posY, posZ) VALUES (@Id, @Energy, @PosX, @PosY, @PosZ);";
            cmd.Parameters.Add(new SqliteParameter("@Id", id));
            cmd.Parameters.Add(new SqliteParameter("@Energy", energy));
            cmd.Parameters.Add(new SqliteParameter("@PosX", position.x));
            cmd.Parameters.Add(new SqliteParameter("@PosY", position.y));
            cmd.Parameters.Add(new SqliteParameter("@PosZ", position.z));
            cmd.ExecuteNonQuery();
            dbConnection.Close();
        }
    }

    public void SaveMushrooms(List<GameObject> mushrooms)
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();
            //create new command for querying
            IDbCommand cmd = dbConnection.CreateCommand();
            //get ID of the save file
            int id = GetSaveID();
            //will hold the max age of individual mushroom
            float maxAge;
            float currentAge;
            Vector3 position;
            Vector3 scale;
            Mushroom mushroom;

            foreach (GameObject i in mushrooms)
            {
                cmd.CommandText = "INSERT INTO mushroomData(id, maxAge, age, posX, posY, posZ, scaleX, scaleY, scaleZ) VALUES (@Id, @MaxAge, @Age, @PosX, @PosY, @PosZ, @ScaleX, @ScaleY, @ScaleZ);";
                mushroom = i.GetComponent<Mushroom>();
                //get the max death age of the current mushroom
                maxAge = mushroom.maxDeathAge;
                currentAge = mushroom.mushroomLife;
                position = i.transform.position;
                //Get scale of current game object
                scale = i.transform.localScale;
                //Add these parameters before executing SQLite command
                cmd.Parameters.Add(new SqliteParameter("@Id", id));
                cmd.Parameters.Add(new SqliteParameter("@MaxAge", maxAge));
                cmd.Parameters.Add(new SqliteParameter("@Age", currentAge));
                cmd.Parameters.Add(new SqliteParameter("@PosX", position.x));
                cmd.Parameters.Add(new SqliteParameter("@PosY", position.y));
                cmd.Parameters.Add(new SqliteParameter("@PosZ", position.z));
                cmd.Parameters.Add(new SqliteParameter("@ScaleX", scale.x));
                cmd.Parameters.Add(new SqliteParameter("@ScaleY", scale.y));
                cmd.Parameters.Add(new SqliteParameter("@ScaleZ", scale.z));
                //execute for this mushroom
                cmd.ExecuteNonQuery();
            }
            dbConnection.Close();
        }
    }
    //Return the key
    private int GetSaveID()
    {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            int id = -1;
            IDbCommand command = dbConnection.CreateCommand();
            //Get the latest key added, the save data currently being processed
            command.CommandText = "SELECT * FROM playerData WHERE id = (SELECT MAX(id)  FROM playerData); ";
            //Execute
            command.ExecuteNonQuery();
            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                id = (int)reader.GetFloat(0);
            }
            //Dispose reader
            reader.Dispose();
            //return the ID
            return id;
        }
    }
}
