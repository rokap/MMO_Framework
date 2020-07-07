using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public static Register instance;

    [SerializeField] InputField username;
    [SerializeField] InputField password;
    [SerializeField] InputField email;
    [SerializeField] Text status;

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

    public void OnValidate()
    {
        InputField[] fields = GetComponentsInChildren<InputField>();
        username = fields[0];
        password = fields[1];
        email = fields[2];
        status = transform.Find("Status").GetComponent<Text>();
    }

    public void SendRegistration()
    {
        ClientSend.SendRegistration(username.text, CreateMD5Hash(password.text), email.text);
    }

    public void SetStatus(string message)
    {
        status.text = message;
    }

    public string CreateMD5Hash(string input)
    {
        // Step 1, calculate MD5 hash from input
        MD5 md5 = MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        // Step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
