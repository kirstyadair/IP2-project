using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeCrosshairScript : MonoBehaviour
{

    Vector3 lastPosition;

    Vector3 innerPosition;
    Vector3 innerMostPosition;

    Vector3 velocity = Vector3.zero;
    public GameObject innerRing;
    public GameObject innerMostRing;
    public Animator animator;
    public bool showing = false;
    public float wobbleFactor = 1f;

    public int currentZombies = 0;
    public int maxZombies = 0;
    public Color fullHealthColour;
    public Color emptyHealthColour;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = this.transform.position;
        innerPosition = innerRing.transform.localPosition;
        innerMostPosition = innerMostRing.transform.localPosition;
    }

    public void Show()
    {
        animator.Play("grow");
        showing = true;

        currentZombies = 0;
        maxZombies = 0;
    }

    public void Hide()
    {
        animator.Play("shrink");
        showing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!showing) return;
        velocity += (lastPosition - transform.position);

        innerRing.transform.localPosition = innerPosition - velocity / 2;
        innerMostRing.transform.localPosition = innerMostPosition - velocity / 3;
        lastPosition = transform.position;

        velocity *= wobbleFactor;

        innerRing.GetComponent<SpriteRenderer>().color = Color.Lerp(emptyHealthColour, fullHealthColour,  (float)currentZombies / (float)maxZombies);
    
        if (currentZombies <= 0 && maxZombies > 0)
        {
            Hide();
        }
    }
}
