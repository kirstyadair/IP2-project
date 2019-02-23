using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LineAnimator : MonoBehaviour
{
    public GameObject lineSegment;
    public float animationSpeed = 5;
    public float offsetBeforeWrapping = 200;
    RectTransform rectTransform;
    float segmentWidth;
    float offset = 0;

    // Current width of the line sprite
    float currentLineWidth;

    // Start is called before the first frame update
    void Start()
    {
        // We're only using lineSegment as a template here so hide it
        lineSegment.SetActive(false);

        rectTransform = GetComponent<RectTransform>();
        segmentWidth = lineSegment.GetComponent<RectTransform>().rect.width;
    }


    // Clone the segments enough times to cover the window
    void CreateSegments()
    {
        // First delete the old segments by deleting the children that aren't active
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf) Destroy(t.gameObject);
        }

        int segmentsNeeded = Mathf.CeilToInt(rectTransform.rect.width / segmentWidth);

        for (int x = 0; x < segmentsNeeded; x++)
        {
            GameObject newSegment = Instantiate(lineSegment, transform);
            newSegment.SetActive(true);
            newSegment.transform.localPosition = new Vector2(-offsetBeforeWrapping + (x * segmentWidth), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the screen has been resized (which then resizes the sprite itself) then update the segments
        if (currentLineWidth != rectTransform.rect.width)
        {
            CreateSegments();
            currentLineWidth = rectTransform.rect.width;
        }

        // Animate the line rightwards
        int x = 0;
        foreach (Transform t in transform)
        {
            x++;

            if (t.gameObject.activeSelf)
            {
                Vector2 segmentPos = t.position;

                segmentPos.x += Time.deltaTime * animationSpeed;

                // Wrap this line segment if it passed the right boundary
                if (segmentPos.x > (rectTransform.rect.width - offsetBeforeWrapping)) {
                    segmentPos.x = -offsetBeforeWrapping;
                }


                t.position = segmentPos;
            }
        }
    }
}
