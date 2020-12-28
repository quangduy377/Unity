using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class test : MonoBehaviour
{
    public class Student
    {
        public int age;
        public string name;
    }
    void Start()
    {
        Student t = new Student();
        t.age = 10;
        t.name = "Duy";

        Type type = typeof(Student);
        PropertyInfo[] properties = type.GetProperties();
        foreach (PropertyInfo property in properties)
        {
            Debug.Log("property name: " + property.Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
