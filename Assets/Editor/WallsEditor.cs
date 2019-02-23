using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Custom editor for walls

[CustomEditor(typeof(Walls))]
[CanEditMultipleObjects]
public class WallsEditor : Editor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnInspectorGUI()
    {
        Walls t = (target as Walls);
        serializedObject.Update();

        // Rotate wall by 90 degrees
        if (GUILayout.Button("Rotate"))
        {
            t.transform.Rotate(new Vector3(0, 0, 90));
        }


        // Flip the wall (flipY will be enabled)
        if (GUILayout.Button("Flip"))
        {
            t.flipped = !t.flipped;
            t.Update();
        }

        //DrawDefaultInspector();
    }

 

    public void OnSceneGUI()
    {
        Walls t = (target as Walls);

        // Scale size with zoom
        float size = HandleUtility.GetHandleSize(t.transform.position) * 1.3f;
        EditorGUI.BeginChangeCheck();
        float scale = Handles.ScaleSlider(t.scale, t.transform.position, t.transform.right, t.transform.rotation, size, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            // Adjust scale value of wall
            if (scale < 1) scale = 1;
            t.scale = (int)scale;
            Undo.RecordObject(target, "Scaled Wall");
            PrefabUtility.RecordPrefabInstancePropertyModifications(t.transform);
            t.Update();
        }
    }
}
