using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public NetworkManager Network { get; private set; }
public PlayerManager Players { get; private set; }

    private GameObject DebugUI { get; set; }
    private bool showDebugUI = false;
    private Text PingText;
    private float _interval = 1f;
    private float _nextTime = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        Network = GetComponentInChildren<NetworkManager>();
        Players = GetComponentInChildren<PlayerManager>();

        var debugUITransform = transform.Find("DebugUI");
        DebugUI = debugUITransform.gameObject;
        var go = debugUITransform.Find("Ping");
        PingText = debugUITransform.Find("Ping").GetComponentInChildren<Text>(true);
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            showDebugUI = !showDebugUI;
            DebugUI.SetActive(showDebugUI);
        }
    }

    void FixedUpdate(){
        if (Time.time >= _nextTime)
        {
            PingText.text = "Ping : " + Network.Ping.ToString();
            _nextTime += _interval;
        }
    }
}
