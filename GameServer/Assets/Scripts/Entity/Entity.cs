using UnityEngine;
public class Entity : MonoBehaviour
{
    public uint id;
    public string description;

    private void Start()
    {
        Server.Send.SpawnEntity(this);
    }
}