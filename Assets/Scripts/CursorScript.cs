using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public InputDevice controller;
    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        //canvas.GetComponent<RectTransform>().rect.
    }

    // Update is called once per frame
    void Update()
    {
        MoveCursor();
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }

        if (transform.position.x > canvas.GetComponent<RectTransform>().rect.width * canvas.scaleFactor)
        {
            transform.position = new Vector3(canvas.GetComponent<RectTransform>().rect.width * canvas.scaleFactor, transform.position.y, transform.position.z);
        }

        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        if (transform.position.y > canvas.GetComponent<RectTransform>().rect.height * canvas.scaleFactor)
        {
            transform.position = new Vector3(transform.position.x, canvas.GetComponent<RectTransform>().rect.height * canvas.scaleFactor, transform.position.z);
        }
    }

    public void MoveCursor()
    {
        if (controller == null) return; // :(((((
        GetComponent<RectTransform>().position += new Vector3(controller.LeftStick.Vector.x, controller.LeftStick.Vector.y, 0) * 10;
        
    }
}
