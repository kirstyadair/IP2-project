using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnivesScript : MonoBehaviour
{
    public GameObject knifePrefab;
    Vector3 direction = new Vector3(0, 0, 1);
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                GameObject knife = (GameObject)Instantiate(knifePrefab, transform.position, Quaternion.identity);
                knife.transform.Rotate(90, 0, 0);
                knife.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            }
        }
        
    }
}
