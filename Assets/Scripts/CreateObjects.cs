using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    Mushroom mushroomScript;
    MainMenu scenes;
    public GameObject ground;
    Vector3 bounds = new Vector3();
    List<GameObject> collectables;
    public GameObject mushroom;
    int maxCollectables = 60;
    int maxEnemies = 4;

    void Start()
    {
        //setting the bounds within which the mushrooms may spawn
        bounds = ground.GetComponent<Renderer>().bounds.size / 2;
        //ensuring mushrooms spawn using the lists below
        collectables = CreateCollectables();
        mushroomScript = GetComponent<Mushroom>();
        scenes = FindObjectOfType<MainMenu>();
    }
    void Update()
    {
    }
    private List<GameObject> CreateCollectables()
    {
        List<GameObject> collectables = new List<GameObject>();

        for (int i = 0; i < maxCollectables; i++)
        {
            collectables.Add(CreateCollectable(i));
        }
        return collectables;
    }

    private GameObject CreateCollectable(int id)
    {
        GameObject collectableCreated = Instantiate(mushroom);
        collectableCreated.transform.position = new Vector3(Random.Range(-bounds.x, bounds.x), 0.5f, (Random.Range(-bounds.z, bounds.z)));
        Renderer rend = collectableCreated.GetComponent<Renderer>();
        rend.material.color = Color.red;
        collectableCreated.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        collectableCreated.name = "collectable" + id;

        return collectableCreated;
    }
}