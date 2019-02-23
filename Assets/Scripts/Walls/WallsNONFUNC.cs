using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Walls : MonoBehaviour
{
    [HideInInspector]
    // Amount of center walls
    public int scale = 1;

    [HideInInspector]
    // The end wall sprite (must be facing left)
    public Sprite end;
    [HideInInspector]

    // The center wall sprite
    public Sprite center;

    // Where the wall is flipped on the axis
    [HideInInspector]
    public bool flipped = false;
    SpriteRenderer spriteRenderer;

    // The value of scale and flipped in the last update, used to cull pointless updates
    private int prevScale = 1;
    private bool prevFlipped = false;
    private Vector3 prevPosition;



    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    /*
    public void Snap()
    {
        for (int i = 0; i < Selection.transforms.Length; i++)
        {
            Vector3 t = Selection.transforms[i].transform.position;
            t.x = Round(t.x);
            t.y = Round(t.y);
            t.z = Round(t.z);
            Selection.transforms[i].transform.position = t;

            prevPosition = Selection.transforms[0].position;
        }
    }*/

    public float SnapWidth(float input)
    {
        float snapValue = center.texture.width / center.pixelsPerUnit;
        float snappedValue;
        snappedValue = snapValue * Mathf.Round(input / snapValue);
        return snappedValue;
    }

    public float SnapHeight(float input)
    {
        float snapValue = center.texture.height / center.pixelsPerUnit;
        float snappedValue;
        snappedValue = snapValue * Mathf.Round(input / snapValue);
        return snappedValue;
    }


    public void Update()
    {
        Vector3 position = this.transform.position;
        //position.x = SnapWidth(position.x);
        //position.y = SnapHeight(position.y);
        this.transform.position = position;

        // Don't continue if the values haven't changed
        if (prevScale == scale && prevFlipped == flipped) return;

        prevScale = scale;
        prevFlipped = flipped;

        // Destroy the wall pieces from the last update
        for (int i = this.transform.childCount; i > 0; --i)
            DestroyImmediate(this.transform.GetChild(0).gameObject);

        // Create the starting wall piece
        GameObject startWallPiece = new GameObject();
        startWallPiece.transform.parent = transform;
        startWallPiece.transform.localPosition = new Vector3(0, 0, 0);
        startWallPiece.transform.localRotation = Quaternion.identity;
        startWallPiece.AddComponent<SpriteRenderer>();
        startWallPiece.GetComponent<SpriteRenderer>().sprite = end;
        startWallPiece.name = "Left end wall piece";
        startWallPiece.GetComponent<SpriteRenderer>().flipY = flipped;

        // Create the center wall pieces
        for (int centerRectCount = 0; centerRectCount < scale; centerRectCount++)
        {
            GameObject wallPiece = new GameObject();
            wallPiece.transform.parent = transform;
            wallPiece.transform.localPosition = new Vector3((end.texture.width / end.pixelsPerUnit) + centerRectCount * (center.texture.width / center.pixelsPerUnit), 0, 0);
            wallPiece.transform.localRotation = Quaternion.identity;
            wallPiece.AddComponent<SpriteRenderer>();
            wallPiece.GetComponent<SpriteRenderer>().sprite = center;
            wallPiece.name = "Center wall piece";
            wallPiece.GetComponent<SpriteRenderer>().flipY = flipped;
        }

        // Create the end wall pieces
        GameObject endWallPiece = new GameObject();
        endWallPiece.transform.parent = transform;
        endWallPiece.transform.localPosition = new Vector3((end.texture.width / end.pixelsPerUnit) * 2 + scale * (center.texture.width / center.pixelsPerUnit), 0, 0);
        endWallPiece.transform.localRotation = Quaternion.identity;
        endWallPiece.AddComponent<SpriteRenderer>();
        endWallPiece.GetComponent<SpriteRenderer>().flipX = true;
        endWallPiece.GetComponent<SpriteRenderer>().sprite = end;
        endWallPiece.name = "Right end wall piece";
        endWallPiece.GetComponent<SpriteRenderer>().flipY = flipped;

        // Previous attempt, worked but was slow af
        /*
        Texture2D endTexture = end.texture;
        Texture2D centerTexture = center.texture;

        // End texture and center texture must be of the same height

        if (endTexture.height != centerTexture.height)
        {
            Debug.Log("End texture and center texture of wall must be the same height!");
            return;
        }

        Texture2D combinedTexture = new Texture2D((centerTexture.width * scale) + endTexture.width * 2, centerTexture.height);

        for (int x = 0; x < endTexture.width; x++)
        {
            for (int y = 0; y < endTexture.height; y++)
            {
                combinedTexture.SetPixel(x, y, endTexture.GetPixel(x, y));
            }
        }

        for (int rectCount = 0; rectCount < scale; rectCount++)
        {
            for (int x = 0; x < centerTexture.width; x++) { 
                {
                    for (int y = 0; y < centerTexture.height; y++)
                    {
                        int copiedX = endTexture.width + x + (rectCount * centerTexture.width);

                        combinedTexture.SetPixel(copiedX, y, centerTexture.GetPixel(x, y));
                    }
                }
            }
        }


        for (int x = 0; x < endTexture.width; x++)
        {
            for (int y = 0; y < endTexture.height; y++)
            {
                int copiedX = endTexture.width + x + (scale * centerTexture.width);

                combinedTexture.SetPixel((endTexture.width * 2) + (scale * centerTexture.width) - x, y, endTexture.GetPixel(x, y));
            }
        }

        combinedTexture.Apply();

        spriteRenderer.sprite = Sprite.Create(combinedTexture, new Rect(0f, 0f, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f));
        */
    }
}
