using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : IComparable<Highscores>
{
    public int Score { get; set; }
    public string Name { get; set; }
    public int ID { get; set; }

    //constructor for highscores
    public Highscores (int id, string name, int score)
    {
        this.ID = id;
        this.Name = name;
        this.Score = score;
    }
    void Start ()
    {
		
	}
	void Update ()
    {
		
	}
    //used by icomparable to ensure different elements from the list are ordered correctly
    public int CompareTo(Highscores other)
    {
        if (other.Score < this.Score)
        {
            return -1;
        }
        else if (other.Score > this.Score)
        {
            return 1;
        }
        return 0;
    }
}
