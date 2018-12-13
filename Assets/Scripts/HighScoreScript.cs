using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{
    public GameObject score;
    public GameObject scoreName;
    public GameObject position;

    public void SetScore(string name, string score, string position)
    {
        this.position.GetComponent<Text>().text = position;
        this.scoreName.GetComponent<Text>().text = name;
        this.score.GetComponent<Text>().text = score;
    }
}
