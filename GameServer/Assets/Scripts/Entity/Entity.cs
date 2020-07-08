using UnityEngine;
public class Entity : MonoBehaviour
{
    public int id;
    public string description;


    private void Start()
    {
        Server.Send.SpawnEntity(this);
    }
}