using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
    private bool _logged = false;
    public User user;
    public void Authenticate(string username, string password)
    {
        GameManager.Instance.Network.Authenticate(new Authentication(username, password));
    }

    public void Register(string username, string password)
    {
        GameManager.Instance.Network.Register(new Authentication(username, password));
    }

    public bool IsLogged()
    {
        return this._logged;
    }

    public void SetLogin(User user)
    {
        this.user = user;
        this._logged = true;
        GameManager.Instance.StartGame();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


public class Authentication
{
    public string username;
    public string password;

    public Authentication(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class User
{
    public string _id;
    public string username;

    //Unixtime in milliseconds Date.now() js
    public long lastAuthentication;

    public User()
    {
    }
}