using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ComponentController : MonoBehaviour
{
    private int mark;

    public int Mark
    {
        get => mark;
        set => mark = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //碰撞与合并逻辑
    private void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag.Equals("Obtacle"))
        {
            return;
        }
        int colliderMark = collider.gameObject.GetComponent<ComponentController>().Mark;
        if ( colliderMark==mark)
        {
            if (colliderMark==5)
            {
                return;
            }
            GameController.setCurrent(this.gameObject,collider.gameObject,mark);
            GameController.score += mark * 2;
            GameObject.Find("Score").GetComponent<Text>().text = "Score" +GameController.score.ToString();
        }
    }
    
}
