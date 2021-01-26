using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public Transform transform;
    private ArrayList CurrentArray = new ArrayList();
    private Vector2 originPosition = new Vector2(0, 10);
    private int score = 0;

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
}
