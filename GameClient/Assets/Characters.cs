using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public static Characters instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SelectCharacter(int id)
    {
        Debug.Log(id);
    }
}
