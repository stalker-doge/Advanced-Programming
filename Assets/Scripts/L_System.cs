using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class L_System : MonoBehaviour
{

    [SerializeField] GameObject turtle;
    [SerializeField] Transform parent;
    [SerializeField] float lineLength;
    [SerializeField] int iterations;
    [SerializeField] float lineAngle;
    [SerializeField] float lineAngleDeviation;
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
        recursionRules.Add('3', "33");
        recursionRules.Add('4', "44");
        recursionRules.Add('5', "55");
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
                    initialPosition = parent.position;
                    parent.Translate(Vector3.up*lineLength);
                    TempTurtle = Instantiate(turtle, parent.position, Quaternion.identity, parent);
                    TempTurtle.GetComponent<LineRenderer>().SetPosition(0, initialPosition);
                    TempTurtle.GetComponent<LineRenderer>().SetPosition(1, parent.position);
                    break;
                case '+':
                    parent.Rotate(Vector3.right,Random.Range(lineAngle-lineAngleDeviation,lineAngle+ lineAngleDeviation));
                    break;

                case '2':
                    parent.Rotate(Vector3.forward, Random.Range(lineAngle - lineAngleDeviation, lineAngle + lineAngleDeviation));
                    break;
                case '3':
                    parent.Rotate(Vector3.back, Random.Range(lineAngle - lineAngleDeviation, lineAngle + lineAngleDeviation));
                    break;

                case '4':
                        parent.Rotate(Vector3.up, Random.Range(lineAngle - lineAngleDeviation, lineAngle + lineAngleDeviation));
                    break;

                case '5':
                    parent.Rotate(Vector3.down, Random.Range(lineAngle - lineAngleDeviation, lineAngle + lineAngleDeviation));
                    break;

                case '-':
                    parent.Rotate(Vector3.left, Random.Range(lineAngle - lineAngleDeviation , lineAngle + lineAngleDeviation));
                    break;

                case '[':
                    transformData.position = parent.position;
                    transformData.rotation = parent.rotation;
                    stack.Push(transformData);
                    break;

                case ']':
                    transformData = stack.Pop();
                    parent.position = transformData.position;
                    parent.rotation = transformData.rotation;
                    break;

                case '(':
                    transformData.position = parent.position;
                    transformData.rotation = parent.rotation;
                    stack.Push(transformData);
                    parent.Rotate(Vector3.left, Random.Range(lineAngle - lineAngleDeviation, lineAngle + lineAngleDeviation));

                    break;
                case ')':
                    transformData = stack.Pop();
                    parent.position = transformData.position;
                    parent.rotation = transformData.rotation;
                    parent.Rotate(Vector3.right, Random.Range(lineAngle - lineAngleDeviation, lineAngle + lineAngleDeviation));
                    break;

                default: break;
            }
        }
    }
}
