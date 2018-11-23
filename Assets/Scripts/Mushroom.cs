using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Player player;
    float maxDeathAge = 150.0f;
    float startTime;
    float mushroomLife;
    public bool edible = true;
	void Start ()
    {
        player = GetComponent<Player>();
        mushroomLife = Random.Range(0, maxDeathAge);
    }
	
	void Update ()
    {
        MushroomLifeSpan();
        MushroomRotation();
	}

    void MushroomLifeSpan()
    {  
        float halflife = mushroomLife / 2;
        mushroomLife -= Time.deltaTime;

        if (mushroomLife <= halflife)
        {
            edible = false;
            Vector4 colour = GetComponent<Renderer>().material.color = new Vector4(0, 1, 0, 1);
        }
        if (mushroomLife <= 0)
        {
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
        float rotationOffset = Random.Range(12.0f, 55.0f);

        transform.Rotate(0.0f, rotationOffset * Time.deltaTime, 0.0f);
    }
}
