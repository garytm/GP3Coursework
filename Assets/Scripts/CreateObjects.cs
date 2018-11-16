using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour
{
    public GameObject ground;
    Vector3 bounds = new Vector3();
    List<GameObject> collectables;
    List<GameObject> enemies;
    List<GameObject> blackholes;
    int maxCollectables = 60;
    int maxEnemies = 4;
    int maxBlackholes = 8;

    void Start()
    {
        bounds = ground.GetComponent<Renderer>().bounds.size / 2;
        collectables = CreateCollectables();
        blackholes = CreateBlackholes();
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
        GameObject collectableCreated = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        collectableCreated.transform.position = new Vector3(Random.Range(-bounds.x, bounds.x), 0.5f, (Random.Range(-bounds.z, bounds.z)));
        Renderer rend = collectableCreated.GetComponent<Renderer>();
        rend.material.color = Color.red;
        collectableCreated.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        collectableCreated.name = "collectable" + id;

        return collectableCreated;
    }

    private List<GameObject> CreateBlackholes()
    {
        List<GameObject> blackholes = new List<GameObject>();

        for (int i = 0; i < maxBlackholes; i++)
        {
            blackholes.Add(CreateBlackhole(i));
        }

        return blackholes;
    }

    private GameObject CreateBlackhole(int id)
    {
        GameObject blackHoleCreated = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        blackHoleCreated.transform.position = new Vector3((-bounds.x + 50.0f), 0.0f, (bounds.z - 50.0f));
        Renderer rend = blackHoleCreated.GetComponent<Renderer>();
        rend.material.color = Color.black;
        blackHoleCreated.transform.localScale = new Vector3(15.0f, 1.0f, 15.0f);
        blackHoleCreated.name = "blackhole" + id;

        return blackHoleCreated;
    }
}