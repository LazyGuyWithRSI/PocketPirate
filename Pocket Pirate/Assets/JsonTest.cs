using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class JsonTest : MonoBehaviour
{
    public TextAsset jsonFile;
    public FloatReference Score;

    // Start is called before the first frame update
    void Start()
    {
        TestClass testClass = JsonUtility.FromJson<TestClass>(jsonFile.text);

        if (testClass.TestString.Equals("Hello there!"))
            Score.Value = 666;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class TestClass
{
    public int ID;
    public string TestString;
}
