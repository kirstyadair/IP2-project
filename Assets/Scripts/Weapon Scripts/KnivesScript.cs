using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesScript : MonoBehaviour
{
    public GameObject knifePrefab;
    Vector3 direction = new Vector3(0, 0, 1);
    public bool isActive;
    float throwCooldown = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        throwCooldown -= Time.deltaTime;

        if (isActive)
        {
            if (Input.GetKey(KeyCode.Space) && throwCooldown <= 0)
            {
                GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);
                knife.AddComponent<ThrowingKnifeScript>();
                knife.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
                throwCooldown = 0.5f;
            }
        }
    }

    
}
