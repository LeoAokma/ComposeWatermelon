using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

//这是控制整个游戏逻辑的脚本，挂载在根目录下的GameController对象下
public class GameController : MonoBehaviour
{
    public static Dictionary<int, int> current = new Dictionary<int, int>(); //存储需要合并的小西瓜的字典
    public static int score = 0; //得分
    private List<GameObject> prefabList = new List<GameObject>(); //预制体的链表，我们在创建场景时把她们加载到内存里
    private Vector2 originPosition = new Vector2(0, 10); //每次小西瓜的初始位置（正中上）
    private GameObject tempObject; //每次需要实例化的小西瓜
    private int label; //小西瓜的编号，这里随机产生，在Assets/Sprites文件夹中有这五个小西瓜
    private bool control = false; //这是判断是否处于鼠标按下但没有松开的状态，此时可以自由水平移动小西瓜
    private int minW = 0, maxW = 5;


    private void Awake()
    {
        for (int i = 0; i < maxW; i++)
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + i.ToString() + ".prefab", typeof(GameObject)) as
                    GameObject;
            prefabList.Add(prefab);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初始时先实例化一个Prefab
        label = Mathf.RoundToInt(Random.Range(minW, maxW));
        tempObject = Instantiate(prefabList[label], new Vector2(0, 10), quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //点击鼠标左键时的逻辑：为实例挂载刚体、碰撞体与脚本
        if (Input.GetMouseButtonDown(0))
        {
            control = true;
            float iny = Input.mousePosition.y;
            tempObject.AddComponent<Rigidbody2D>();
            tempObject.AddComponent<ComponentController>();
            tempObject.GetComponent<ComponentController>().Mark = label;
            tempObject.AddComponent<CircleCollider2D>();
        }

        //鼠标左键没有松开时的逻辑：可以自由移动
        if (control)
        {
            float inx = Input.mousePosition.x;
            //这是判断是否处于鼠标按下但没有松开的状态，此时可以自由水平移动小西瓜
            tempObject.transform.position = new Vector2((inx - 540) / 100, 10);
            //注意世界坐标和屏幕坐标的换算，一个世界坐标代表100像素
        }

        //松开鼠标左键的逻辑：实例化下一个小西瓜并把她放在初始位置
        if (Input.GetMouseButtonUp(0))
        {
            control = false;
            //Debug.Log(originPosition);
            label = Mathf.RoundToInt(Random.Range(minW, maxW));
            tempObject = Instantiate(prefabList[label], originPosition, quaternion.identity);
        }
    }

    public static void setCurrent(GameObject a, GameObject b, int mark)
    {
        //使用两个物体的哈希值存储这次碰撞，使得这次碰撞在全局中唯一，并只会生成一个碰撞后的大一点的小西瓜
        int key = Mathf.Max(a.GetHashCode(), b.GetHashCode());
        int value = Mathf.Min(a.GetHashCode(), b.GetHashCode());
        //如果已经进行了合并，就不再合并，并移除这个键值，这里并发处理的好像还是有问题的
        if (current.ContainsKey(key) && current[key] == value)
        {
            current.Remove(key);
        }
        //否则新实例化一个大一点的小西瓜，并销毁原来的两个小西瓜
        else
        {
            current.Add(key, value);
            GameObject newPrefab =
                AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + (mark + 1).ToString() + ".prefab", typeof(GameObject))
                    as GameObject;
            float newx = (a.transform.position.x + b.transform.position.x) / 2;
            float newy = (a.transform.position.y + b.transform.position.y) / 2;
            Destroy(a.gameObject);
            Destroy(b.gameObject);
            GameObject newObject = Instantiate(newPrefab,
                new Vector2(newx, newy)
                , quaternion.identity);
            newObject.AddComponent<ComponentController>();
            newObject.GetComponent<ComponentController>().Mark = mark + 1;
            newObject.AddComponent<CircleCollider2D>();
            newObject.AddComponent<Rigidbody2D>();
        }
    }
}