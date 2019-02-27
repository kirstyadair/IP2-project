using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    Vector3 direction = new Vector3(0, 0, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            GameObject ketchup = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
            ketchup.AddComponent<KetchupScript>();
            ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }
    }
    
}
