using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KetchupBlob")
        {
            Debug.Log("Wall collision");
            Destroy(other.gameObject);
        }
    }
}
