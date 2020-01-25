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

    private List<GameObject> RemotePlayers;


    // Start is called before the first frame update
    void Start()
    {
        Player = Instantiate(PlayerPrefab);
        Player.transform.SetParent(transform);
        Camera.main.transform.SetParent(Player.transform);
    }

    void SpawnRemotePlayer() {
        var remotePlayerInstance = Instantiate(RemotePlayerPrefab);
        RemotePlayers.Add(remotePlayerInstance);
        remotePlayerInstance.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
