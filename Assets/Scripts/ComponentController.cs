using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
            GameObject newPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + (mark + 1).ToString() + ".prefab",typeof(GameObject)) as GameObject;
            float newx = (this.transform.position.x + collider.transform.position.x) / 2;
            float newy = (this.transform.position.y + collider.transform.position.y) / 2;
            Destroy(collider.gameObject);
            GameObject newObject = Instantiate(newPrefab,
                new Vector2(newx,newy)
                    ,quaternion.identity);
            newObject.AddComponent<ComponentController>();
            newObject.GetComponent<ComponentController>().Mark = mark+1;
            Destroy(this.gameObject);
        }
    }
    
}
