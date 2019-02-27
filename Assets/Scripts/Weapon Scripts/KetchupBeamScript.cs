using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupBeamScript : MonoBehaviour
{
    public GameObject ketchupPrefab;
    Vector3 direction = new Vector3(0, 0, 1);

    public bool mouseDown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            mouseDown = true;
            GameObject ketchup = (GameObject)Instantiate(ketchupPrefab, transform.position, Quaternion.identity);
            ketchup.AddComponent<KetchupScript>();
            ketchup.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            mouseDown = false;
        }
    }
    
}
