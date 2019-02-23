using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAIScript : MonoBehaviour
{
    public Transform[] enemyPoints;
    public int point = -1;
    Vector2 direction;
    float distance;
    float speed = 0.01f;
    float AIfireCooldown = 0f;

    public PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        NextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        AIfireCooldown -= Time.deltaTime;

        // move in direction of point
        transform.Translate(direction * speed);

        // find distance between enemy and point
        distance = Vector3.Distance(enemyPoints[point].position, transform.position);

        //Debug.Log("Distance = " + distance);

        if (distance < 0.1) NextPoint();

        if (AIfireCooldown <= 0)
        {
            playerScript.Shoot(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            AIfireCooldown += 1;
        }
        
    }

    void NextPoint()
    {
        if (point < enemyPoints.Length - 1)
        {
            point++;
        }
        else
        {
            point = 0;
        }


        // find the direction of the next point
        direction = (enemyPoints[point].position - transform.position);

        // cycle through array of points

    }
}
