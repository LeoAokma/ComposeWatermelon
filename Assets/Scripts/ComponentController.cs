using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//这是挂载在每一个实例化的小西瓜上的脚本
public class ComponentController : MonoBehaviour
{
    private int mark;//标识了每一个小西瓜，比如1代表那个最小的紫色的小西瓜

    //这是C#的语法糖啊
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
        //为地面和两边的墙添加tag:"Obtacle"，这样就不会检测她们，可以节约性能开销
        if (collider.gameObject.tag.Equals("Obtacle"))
        {
            return;
        }
        int colliderMark = collider.gameObject.GetComponent<ComponentController>().Mark;
        //如果两个小西瓜编号相同，那么就可以合并
        if ( colliderMark==mark)
        {
            //这里我只切了5张图，所以上限就是5了，编号是5的两个西瓜不能合并了
            if (colliderMark==5)
            {
                return;
            }
            GameController.setCurrent(this.gameObject,collider.gameObject,mark);//通知主函数进行合并
            GameController.score += mark * 2;//分数就可以采用一定规则累计了
            GameObject.Find("Score").GetComponent<Text>().text = "Score" +GameController.score.ToString();
        }
    }
    
}
