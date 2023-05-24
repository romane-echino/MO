using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AuthenticationBehavior : MonoBehaviour
{
    public GameObject LoginField;
    public GameObject PasswordField;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf && GameManager.Instance.Auth.IsLogged()){
            gameObject.SetActive(false);
        }
    }


    public void Login()
    {
        var username = LoginField.GetComponent<TMPro.TMP_InputField>().text;
        var password = PasswordField.GetComponent<TMPro.TMP_InputField>().text;

        GameManager.Instance.Auth.Authenticate(username, password);
    }

    public void Register()
    {
        var username = LoginField.GetComponent<TMPro.TMP_InputField>().text;
        var password = PasswordField.GetComponent<TMPro.TMP_InputField>().text;

        GameManager.Instance.Auth.Register(username, password);
    }
}
