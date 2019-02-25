using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAIScript : MonoBehaviour
{
    public Transform[] enemyPoints;
    public Rigidbody rb;
    public int point = -1;
    Vector3 direction;
    float distance;
    float speed = 0.01f;
    float AIfireCooldown = 0f;

    public PlayerScript playerScript;

    // Start is called before the first frame update
    void Start()
    {
        NextPoint();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        AIfireCooldown -= Time.deltaTime;

        // move in direction of point
        //rb.AddForce(new Vector2(direction.x, direction.y), ForceMode2D.Impulse);  THIS LINE ISN'T WORKING
        transform.Translate(direction * speed);

        // find distance between enemy and point
        distance = Vector3.Distance(enemyPoints[point].position, transform.position);

        //Debug.Log("Distance = " + distance);

        if (distance < 0.1) NextPoint();

        if (AIfireCooldown <= 0)
        {
            playerScript.Shoot(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            AIfireCooldown++;
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

    }
}
