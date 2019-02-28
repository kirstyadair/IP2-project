using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    bool ketchupActive = false;
    bool spatulaActive = false;
    bool knivesActive = false;
    public KetchupBeamScript ketchupBeamScript;
    public KnivesScript knivesScript;
    public SpatulaScript spatulaScript;
    public Text weaponText;

    // Start is called before the first frame update
    void Start()
    {
        weaponText.text = "Press 1 for Ketchup Beam, 2 for Spatula, or 3 for Knives";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!ketchupActive)
            {
                weaponText.text = "Ketchup beam active - press space to shoot";
                ketchupBeamScript.isActive = true;
                spatulaScript.isActive = false;
                knivesScript.isActive = false;

                ketchupActive = true;
                spatulaActive = false;
                knivesActive = false;
            }
            else
            {
                ketchupBeamScript.isActive = false;
                spatulaScript.isActive = false;
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
                weaponText.text = "Spatula active - press space to hit";
                ketchupBeamScript.isActive = false;
                spatulaScript.isActive = true;
                knivesScript.isActive = false;

                ketchupActive = false;
                spatulaActive = true;
                knivesActive = false;
            }
            else
            {
                ketchupBeamScript.isActive = false;
                spatulaScript.isActive = false;
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
                weaponText.text = "Knives active - press space to throw";
                ketchupBeamScript.isActive = false;
                spatulaScript.isActive = false;
                knivesScript.isActive = true;

                ketchupActive = false;
                spatulaActive = false;
                knivesActive = true;
            }
            else
            {
                ketchupBeamScript.isActive = false;
                spatulaScript.isActive = false;
                knivesScript.isActive = false;

                ketchupActive = false;
                spatulaActive = false;
                knivesActive = false;
            }
        }

    }
}
