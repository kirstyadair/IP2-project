using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    // this script is here because ketchupScript isn't around anymore - this would be in OnTriggerEnter of that script
    // it only exists to stop ketchup from travelling through walls and inflicting damage

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KetchupBlob")
        {
            Destroy(other.gameObject);
        }
    }
}
