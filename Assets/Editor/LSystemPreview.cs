using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class LSystemPreview : EditorWindow
{

    float lineLength = 1;
    int iterations = 1;
    float lineAngle = 1;
    float lineAngleDeviation = 0;
    string axiom = "";
    L_System lSystem;
    GameObject lSystemObject;
    GameObject turtle;

    [MenuItem("Tools/L System Preview")]



    public static void ShowWindow()
    {
        GetWindow(typeof(LSystemPreview));
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Configure L System", EditorStyles.largeLabel);

        lineLength = EditorGUILayout.FloatField("Line Length", lineLength);
        iterations = EditorGUILayout.IntField("Iterations", iterations);
        lineAngle = EditorGUILayout.FloatField("Line Angle", lineAngle);
        lineAngleDeviation = EditorGUILayout.FloatField("Line Angle Deviation", lineAngleDeviation);
        axiom = EditorGUILayout.TextField("Axiom", axiom);
        turtle = (GameObject)EditorGUILayout.ObjectField("Turtle", turtle, typeof(GameObject), true);


        EditorGUILayout.Space();
        if (GUILayout.Button("Create L System"))
        {
            if (!lSystemObject)
            {
                lSystem = CreateLSystem();
            }
            else
            {
                DestroyImmediate(lSystemObject);
                lSystem = CreateLSystem();
            }
        }
        if (GUILayout.Button("Save Rules"))
        {
            //finds all the L_Systems in the scene, then applies the settings to them 
            L_System[] l_Systems = FindObjectsOfType<L_System>();
            foreach (L_System l_System in l_Systems)
            {
                l_System.SaveSettings(lineLength, iterations, lineAngle, lineAngleDeviation, axiom, lSystemObject.transform, turtle);
            }
        }
    }

    L_System CreateLSystem()
    {
        lSystemObject = new GameObject("L System");
        lSystem = lSystemObject.AddComponent<L_System>();
        if (!turtle)
        {
            turtle = lSystem.CreateTurtle();
        }
        lSystem.Initialise(lineLength, iterations, lineAngle, lineAngleDeviation, axiom, lSystemObject.transform, turtle);
        lSystem.GenerateString();
        return lSystem;
    }
}
