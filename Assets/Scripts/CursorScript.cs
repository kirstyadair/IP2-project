using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    public InputDevice controller;
    Canvas canvas;
    Rigidbody2D rb;
    ParticleSystem ps;
    Image image;
    Animator animator;
    public int playerNumber;

    Vector3 startPosition;

    bool isYeeting = false;
    bool isSelected = false;
    Vector2 yeetingVelocity;

    public float yeetingSpeed;
    public float yeetingDrag;

    public ChoosePlayerSelector currentlyHoveredSelector = null;


    // Start is called before the first frame update
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        startPosition = transform.position;
        //canvas.GetComponent<RectTransform>().rect

       // Yeet();
    }


    // yeet the cursor into the game
    public void Yeet()
    {
        this.transform.position = startPosition;
        isYeeting = true;
        yeetingVelocity = new Vector2(0, yeetingSpeed);
    }

    public void FlashForAvailability()
    {
        ps.Emit(20);
        animator.Play("cursor jump");
    }

    void Hide()
    {
        Color clr = image.color;
        clr.a = 0f;
        image.color = clr;
    }

    void Show()
    {
        Color clr = image.color;
        clr.a = 1f;
        image.color = clr;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller == null) return;
        transform.position += (new Vector3(yeetingVelocity.x, yeetingVelocity.y, 0) * Time.deltaTime);
        yeetingVelocity *= yeetingDrag;
        if (yeetingVelocity.magnitude < 0.001f) yeetingVelocity = Vector2.zero;

        if (isYeeting)
        {

            if (yeetingVelocity.magnitude < 5f)
            {
                isYeeting = false;
                FlashForAvailability();
            }
        }
        else
        {
            if (!isSelected)
            {
                MoveCursor();

                Vector3 position = transform.localPosition;

                float canvasWidth = canvas.GetComponent<RectTransform>().rect.width;// * canvas.scaleFactor;
                float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;// * canvas.scaleFactor;

                //Debug.Log("Position: " + position + "/ width: " + canvasWidth + " / height: " + canvasHeight);
                // constrain within 
                if (position.x < -canvasWidth/2)
                {
                    position = new Vector3(-canvasWidth/2, position.y, position.z);
                }

                if (position.x > canvasWidth/2)
                {
                    position = new Vector3(canvasWidth/2, position.y, position.z);
                }

                if (position.y < -canvasHeight/2)
                {
                    position = new Vector3(position.x, -canvasHeight/2, position.z);
                }

                if (position.y > canvasHeight/2)
                {
                    position = new Vector3(position.x, canvasHeight/2, position.z);
                }

                transform.localPosition = position;
            }

            if (controller.Action1.WasPressed && !isSelected && !isYeeting && currentlyHoveredSelector != null)
            {
                if (!currentlyHoveredSelector.isSelected)
                {
                    isSelected = true;
                    currentlyHoveredSelector.Select(playerNumber);

                    Hide();
                }
            }

            if (controller.Action2.WasPressed && isSelected)
            {
                isSelected = false;

                currentlyHoveredSelector.Unselect();
                currentlyHoveredSelector.selectedPlayer = -1;
                Show();
            }
        }
    }

    public void MoveCursor()
    {
        if (controller == null) return; // :(((((
        GetComponent<RectTransform>().position += new Vector3(controller.LeftStick.Vector.x, controller.LeftStick.Vector.y, 0) * 10;
    }
}
