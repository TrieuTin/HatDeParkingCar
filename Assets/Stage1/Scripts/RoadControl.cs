using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControl : MonoBehaviour
{
    public GameObject Road;
    public float roadLength = 20f;
    public float speed = 5f;


    public float maxTurnAngle = 60f;
    public float turnSpeed = 5f;
    public bool turnRight = true;
    private bool isTouching = false;

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

        if (Input.touchCount > 0) isTouching = true;
        else if (Input.touchCount == 0) isTouching = false;

        if (isTouching)
        {
            //di chuyen phia truoc mot doan
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //tinh toan goc xoay
            float turnStep = turnSpeed * Time.deltaTime;
            // xoay khi di chuyen
            float dir = turnRight ? 1f : -1f;
            transform.Rotate(Vector3.up * dir * turnSpeed * Time.deltaTime);
        }
    }

    void Spawn(float zPos)
    {
        
        GameObject newRoad = Instantiate(Road, new Vector3(0f, -0.37f, zPos), Quaternion.identity);

        instantRoad.Add(newRoad);

        Debug.Log("LEN:" + instantRoad[instantRoad.Count - 1].GetComponent<Collider>().bounds.size.z);

        roadLength = instantRoad[instantRoad.Count - 1].GetComponent<Collider>().bounds.size.z ;

        Debug.Log("roadLen " + roadLength);
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
