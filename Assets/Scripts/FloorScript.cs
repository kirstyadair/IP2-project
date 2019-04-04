using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KetchupBlob")
        {
            Destroy(other.gameObject);
        }
    }
}
