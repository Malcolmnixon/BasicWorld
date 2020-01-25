using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    [Header("Prefabs")]
    
    [SerializeField]
    public GameObject PlayerPrefab;
    
    [Space]

    [SerializeField]
    public GameObject RemotePlayerPrefab;

    private GameObject Player;

    private GameObject[] RemotePlayers;


    // Start is called before the first frame update
    void Start()
    {
        Player = Instantiate(PlayerPrefab);
        Player.transform.SetParent(transform);
        Camera.main.transform.SetParent(Player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
