using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    bool ketchupActive = false;
    bool spatulaActive = false;
    bool knivesActive = false;
    public KetchupBeamScript ketchupBeamScript;
    public KnivesScript knivesScript;
    // public SpatulaScript spatulaScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!ketchupActive)
            {
                Debug.Log("KetchupActive");
                ketchupBeamScript.isActive = true;
                // spatulaScript.isActive = false;
                knivesScript.isActive = false;

                ketchupActive = true;
                spatulaActive = false;
                knivesActive = false;
            }
            else
            {
                ketchupBeamScript.isActive = false;
                // spatulaScript.isActive = false;
                knivesScript.isActive = false;

                ketchupActive = false;
                spatulaActive = false;
                knivesActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!spatulaActive)
            {
                Debug.Log("SpatulaActive");
                ketchupBeamScript.isActive = false;
                // spatulaScript.isActive = true;
                knivesScript.isActive = false;

                ketchupActive = false;
                spatulaActive = true;
                knivesActive = false;
            }
            else
            {
                ketchupBeamScript.isActive = false;
                // spatulaScript.isActive = false;
                knivesScript.isActive = false;

                ketchupActive = false;
                spatulaActive = false;
                knivesActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!knivesActive)
            {
                Debug.Log("KnivesActive");
                ketchupBeamScript.isActive = false;
                // spatulaScript.isActive = false;
                knivesScript.isActive = true;

                ketchupActive = false;
                spatulaActive = false;
                knivesActive = true;
            }
            else
            {
                ketchupBeamScript.isActive = false;
                // spatulaScript.isActive = false;
                knivesScript.isActive = false;

                ketchupActive = false;
                spatulaActive = false;
                knivesActive = false;
            }
        }

    }
}
