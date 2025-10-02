using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControl : MonoBehaviour
{
    public GameObject Road;
    public float roadLength = 20f;
    public float speed = 5f;


   

    private List<GameObject> instantRoad = new List<GameObject>();
    private void Awake()
    {
        
    }
    void Start()
    {
        // Spawn 
        for (int i = 0; i < 3; i++)
        {
            Spawn(i * roadLength);
        }

        StartCoroutine(SpawnLoop());
    }

    void Update()
    {
        // Move roads
        foreach (var road in instantRoad)
        {
            if (road != null)
            {
                road.transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
        }

       
    }

    void Spawn(float zPos)
    {
        
        GameObject newRoad = Instantiate(Road, new Vector3(0f, -0.37f, zPos), Quaternion.identity);

        instantRoad.Add(newRoad);

        

        roadLength = instantRoad[instantRoad.Count - 1].GetComponent<Collider>().bounds.size.z ;

       
    }

    void SpawnNext()
    {
        //get last position
        roadLength = 20;
        float newZ = instantRoad[instantRoad.Count - 1].transform.position.z + roadLength;
        Spawn(newZ);
    }

    void DestroyRoad()
    {
        if (instantRoad.Count > 0 && instantRoad[0].transform.position.z <= -20)
        {
            
            Destroy(instantRoad[0]);
            instantRoad.RemoveAt(0);
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            DestroyRoad();

            if (instantRoad.Count < 3) 
            {
                SpawnNext();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
