using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class L_System : MonoBehaviour
{

    [SerializeField] GameObject turtle;
    [SerializeField] GameObject point;
    [SerializeField] float lineLength;
    [SerializeField] int iterations;
    [SerializeField] float lineAngle;
    [SerializeField] string axiom;
    [SerializeField] Dictionary<char, string> recursionRules = new Dictionary<char, string>();
    struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        
    }

    Stack<TransformData> stack = new Stack<TransformData>();
    // Start is called before the first frame update
    void Start()
    {
        recursionRules.Add('X', "F+[[X]-X]-F[-FX]+X");
        recursionRules.Add('F', "FF");
        recursionRules.Add('1', "11");
        recursionRules.Add('2', "22");
        recursionRules.Add('0', "1(0)0");
        recursionRules.Add('H', "H-G+H+G-H");
        recursionRules.Add('G', "GG");
        recursionRules.Add('A', "A+B");
        recursionRules.Add('B', "A-B");
        GenerateString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateString()
    {
        string tempString = axiom;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < iterations; i++)
        {
            foreach(char c in tempString)
            {
                if(recursionRules.ContainsKey(c))
                {
                    sb.Append(recursionRules[c]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            tempString= sb.ToString();
            Debug.Log(tempString);
            sb.Clear();
        }
        ApplyRules(tempString);
    }

    void ApplyRules(string axiom)
    {
        Vector3 initialPosition = new Vector3();
        GameObject TempTurtle = new GameObject();
        TransformData transformData = new TransformData();
        foreach (char c in axiom)
        {
            switch (c)
            {
                case 'X':
                    break;
                case 'A':
                case 'B':
                case 'H':
                case 'G':
                case '0':
                case '1':
                case 'F':
                    initialPosition = turtle.transform.position;
                    turtle.transform.Translate(Vector3.up*lineLength);
                    TempTurtle = Instantiate(turtle, turtle.transform.position, Quaternion.identity);
                    TempTurtle.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    TempTurtle.GetComponent<LineRenderer>().SetPosition(1, turtle.transform.position);
                    break;
                case '+':
                    turtle.transform.Rotate(Vector3.right, lineAngle);
                    break;

                case '2':
                    turtle.transform.Rotate(Vector3.forward,lineAngle);
                    break;
                case '3':
                    turtle.transform.Rotate(Vector3.back, lineAngle);
                    break;

                case '-':
                    turtle.transform.Rotate(Vector3.left, lineAngle);
                    break;

                case '[':
                    transformData.position = turtle.transform.position;
                    transformData.rotation = turtle.transform.rotation;
                    stack.Push(transformData);
                    break;

                case ']':
                    transformData = stack.Pop();
                    turtle.transform.position = transformData.position;
                    turtle.transform.rotation = transformData.rotation;
                    break;

                case '(':
                    transformData.position = turtle.transform.position;
                    transformData.rotation = turtle.transform.rotation;
                    stack.Push(transformData);
                    turtle.transform.Rotate(Vector3.left, lineAngle);

                    break;
                case ')':
                    transformData = stack.Pop();
                    turtle.transform.position = transformData.position;
                    turtle.transform.rotation = transformData.rotation;
                    turtle.transform.Rotate(Vector3.right, lineAngle);
                    break;

                default: break;
            }
        }
    }
}
