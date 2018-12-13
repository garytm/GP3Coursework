using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Player player;
    //setting the maximum life of a mushroom to 150
    public float maxDeathAge = 150.0f;
    float startTime;
    //used for individual mushroom lives
    public float mushroomLife;
    //to check if a mushroom is edible or not
    public bool edible;
	void Start ()
    {
        player = GetComponent<Player>();
        mushroomLife = Random.Range(0, maxDeathAge);
        edible = true;
    }
	
	void Update ()
    {
        MushroomLifeSpan();
        MushroomRotation();
	}

    void MushroomLifeSpan()
    {  
        //each individual mushrooms half life will be based on its max life
        float halflife = mushroomLife / 2;
        mushroomLife -= Time.deltaTime;

        if (mushroomLife <= halflife)
        {
            //if a mushroom has surpassed its half life, it is no longer edible and changes colour
            edible = false;
            Vector4 colour = GetComponent<Renderer>().material.color = new Vector4(0, 1, 0, 1);
        }
        if (mushroomLife <= 0)
        {
            //if a mushroom is dead, it floats away and shrinks to 50% its original size
            edible = false;
            transform.Translate(Vector3.up * Time.deltaTime);
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
            if(transform.localScale.x < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }

    void MushroomRotation()
    {
        //mushrooms rotate at different randomised amounts
        float rotationOffset = Random.Range(12.0f, 55.0f);

        transform.Rotate(0.0f, rotationOffset * Time.deltaTime, 0.0f);
    }
}
