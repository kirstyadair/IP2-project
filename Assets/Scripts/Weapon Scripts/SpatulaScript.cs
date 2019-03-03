using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatulaScript : MonoBehaviour
{
    float hitCountdown = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitCountdown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && hitCountdown <= 0)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 0.5f);
            Debug.Log("Number of hit enemies: " + hitEnemies.Length);
            foreach (Collider sushi in hitEnemies)
            {
                if (sushi.tag == "Zombie")
                {
                    sushi.gameObject.GetComponent<ZombieScript>().Hit();
                }
            }
            hitCountdown = 2.0f;
        }
        
    }
}
