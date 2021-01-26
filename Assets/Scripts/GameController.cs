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
    private Vector2 originPosition = new Vector2(0, 10);
    public static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     float inx = Input.mousePosition.x;
        //     float iny = Input.mousePosition.y;
        //     
        // }
        if (Input.GetMouseButtonUp(0))
        {
            float inx = Input.mousePosition.x;
            float iny = Input.mousePosition.y;
            originPosition = new Vector2((inx-540) / 100, 10);
            Debug.Log(originPosition);
            int label = Mathf.RoundToInt(Random.Range(1f, 5f));
            GameObject prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + label.ToString() + ".prefab",typeof(GameObject)) as GameObject;
            GameObject tempObject = Instantiate(prefab,originPosition,quaternion.identity);
            //tempObject.transform.SetParent(transform);
            tempObject.AddComponent<ComponentController>();
            tempObject.GetComponent<ComponentController>().Mark=label;
            //position是绝对坐标，localposition是相对父节点的坐标
            //CurrentArray.Add(tempObject);
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
        }
    }
    
}
