using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public NetworkManager Network { get; private set; }
    public PlayerManager Players { get; private set; }
    public AttackManager Attack { get; private set; }
    public CursorManager Cursor { get; private set; }
    public TerrainManager Terrain { get; private set; }
    public EnnemyManager Ennemies { get; private set; }
    public AuthenticationManager Auth { get; private set; }

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
        Attack = GetComponentInChildren<AttackManager>();
        Cursor = GetComponentInChildren<CursorManager>();
        Ennemies = GetComponentInChildren<EnnemyManager>();
        Terrain = GetComponentInChildren<TerrainManager>();
        Auth = GetComponentInChildren<AuthenticationManager>();
/*
        var debugUITransform = transform.Find("DebugUI");
        DebugUI = debugUITransform.gameObject;
        var go = debugUITransform.Find("Ping");
        PingText = debugUITransform.Find("Ping").GetComponentInChildren<Text>(true);*/
    }

    public void StartGame(){
        Network.Connect();
        Terrain.Load();
        Ennemies.Load();
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

    void FixedUpdate()
    {
        /*if (Time.time >= _nextTime)
        {
            PingText.text = "Ping : " + Network.Ping.ToString();
            _nextTime += _interval;
        }*/
    }
}
