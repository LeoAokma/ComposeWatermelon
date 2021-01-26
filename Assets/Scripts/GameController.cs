using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static Dictionary<int,int> current = new Dictionary<int,int>();
    public static int score = 0;
    private Vector2 originPosition = new Vector2(0, 10);
    private GameObject tempObject;
    private int label;
    private bool control = false;

    // Start is called before the first frame update
    void Start()
    {
        label = Mathf.RoundToInt(Random.Range(1f, 5f));
        GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + label.ToString() + ".prefab",typeof(GameObject)) as GameObject;
        tempObject = Instantiate(prefab,new Vector2(0,10),quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        float inx = Input.mousePosition.x;
        if (Input.GetMouseButtonUp(0))
        {
            control = false;
            //Debug.Log(originPosition);
            label = Mathf.RoundToInt(Random.Range(1f, 5f));
            GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + label.ToString() + ".prefab",typeof(GameObject)) as GameObject;
            tempObject = Instantiate(prefab,originPosition,quaternion.identity);
        }
        if (control)
        {
            tempObject.transform.position = new Vector2((inx-540) / 100, 10);
        }
        if (Input.GetMouseButtonDown(0))
        {
            control = true;
            inx = Input.mousePosition.x;
            float iny = Input.mousePosition.y;
            tempObject.AddComponent<Rigidbody2D>();
            tempObject.AddComponent<ComponentController>();
            tempObject.GetComponent<ComponentController>().Mark=label;
            tempObject.AddComponent<CircleCollider2D>();
            //position是绝对坐标，localposition是相对父节点的坐标
        }
    }

    public static void setCurrent(GameObject a,GameObject b,int mark)
    {
        int key = Mathf.Max(a.GetHashCode(), b.GetHashCode());
        int value = Mathf.Min(a.GetHashCode(), b.GetHashCode());
        if (current.ContainsKey(key) && current[key] == value)
        {
            //已经进行了合并
            current.Remove(key);
        }
        else
        {
            current.Add(key,value);
            GameObject newPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + (mark + 1).ToString() + ".prefab",typeof(GameObject)) as GameObject;
            float newx = (a.transform.position.x + b.transform.position.x) / 2;
            float newy = (a.transform.position.y + b.transform.position.y) / 2;
            Destroy(a.gameObject);
            Destroy(b.gameObject);
            GameObject newObject = Instantiate(newPrefab,
                new Vector2(newx,newy)
                ,quaternion.identity);
            newObject.AddComponent<ComponentController>();
            newObject.GetComponent<ComponentController>().Mark = mark+1;
            newObject.AddComponent<CircleCollider2D>();
            newObject.AddComponent<Rigidbody2D>();
        }
    }
    
}
