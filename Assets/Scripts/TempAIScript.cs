using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAIScript : MonoBehaviour
{
    public Transform[] enemyPoints;
    public Rigidbody rb;
    public int point = -1;
    public Vector3 direction;
    float distance;
    public float speed = 0.01f;
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

        Vector3 planarPosition = enemyPoints[point].position;
        planarPosition.z = transform.position.z;
        // find distance between enemy and point
        distance = Vector3.Distance(planarPosition, transform.position);

        //Debug.Log("Distance = " + distance);

        if (distance < 0.5) NextPoint();

        if (AIfireCooldown <= 0)
        {
            playerScript.Shoot(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));
            AIfireCooldown++;
        }


        // find the direction of the next point
        direction = (enemyPoints[point].position - transform.position);
        direction.Normalize();  
        direction.z = 0;

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


    }
}
