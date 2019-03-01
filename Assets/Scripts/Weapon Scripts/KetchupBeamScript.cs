using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    public GameObject gun;
    Vector3 direction = new Vector3(0, 0, 1);
    public bool isActive;
    //public bool mouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        gun.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            gun.GetComponent<MeshRenderer>().enabled = true;
            if (Input.GetKey(KeyCode.Space))
            {
                //mouseDown = true;
                GameObject ketchup = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
                ketchup.AddComponent<KetchupScript>();
                ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                //mouseDown = false;
            }
        }
        else
        {
            gun.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
