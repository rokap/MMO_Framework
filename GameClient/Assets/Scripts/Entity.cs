using UnityEngine; 
public class Entity : MonoBehaviour
{
    public int id;

    public void Initialize(int _id, string _name)
    {
        id = _id;
        name = _name;
        transform.SetParent(GameObject.Find("World/Scenery").transform);
    }
}