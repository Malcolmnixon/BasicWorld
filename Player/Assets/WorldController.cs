using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicWorld;
using System;

public class WorldController : MonoBehaviour
{

    [Header("Resources")]

    public Transform WorldRoot;
    
    [Header("Prefabs")]
    
    [SerializeField]
    public GameObject PlayerPrefab;
    
    [Space]

    [SerializeField]
    public GameObject RemotePlayerPrefab;

    [SerializeField]
    public GameObject  MonsterPrefab;

    private GameObject Player;


    private Dictionary<Guid, GameObject> RemotePlayers;

    private Dictionary<Guid, GameObject> RemoteMonster;

    private IWorld World;


    // Start is called before the first frame update
    void Start()
    {
        CreateLocalWorld();
    }

    private void CreateLocalWorld()
    {
        World = new LocalWorld();
        SpawnPlayer(World.Player.Position);
        foreach (var remote in World.RemotePlayers) 
        {
            SpawnRemotePlayer(remote.Guid, remote.Position);
        }
        foreach (var mon in World.Monsters) 
        {
            SpawnMonster(mon.Guid, mon.Position);
        }
    }

    void SpawnPlayer(Vec2 pos) 
    {
        if (Player != null) 
        {
            Player.transform.SetParent(null);
            Player = null;
        }
        Player = Instantiate(PlayerPrefab);
        Player.transform.SetParent(WorldRoot);
        Camera.main.transform.SetParent(Player.transform);
        
        UpdatePosition(Player, pos);
    }
    void SpawnRemotePlayer(Guid id, Vec2 pos) 
    {
        if (RemotePlayers.ContainsKey(id)) 
        {
            RemotePlayers[id].transform.SetParent(null);
            RemotePlayers.Remove(id);
        }
        var remotePlayerInstance = Instantiate(RemotePlayerPrefab);
        RemotePlayers.Add(id, remotePlayerInstance);
        remotePlayerInstance.transform.SetParent(WorldRoot);
        UpdatePosition(remotePlayerInstance, pos);
    }
    void SpawnMonster(Guid id, Vec2 pos) 
    {
        if (RemoteMonster.ContainsKey(id)) 
        {
            RemoteMonster[id].transform.SetParent(null);
            RemoteMonster.Remove(id);
        }
        var remoteMonsterInstance = Instantiate(MonsterPrefab);
        RemoteMonster.Add(id, remoteMonsterInstance);
        remoteMonsterInstance.transform.SetParent(WorldRoot);
        UpdatePosition(remoteMonsterInstance, pos);
    }

    void UpdatePosition(GameObject gameObj, Vec2 pos) {
        gameObj.transform.position = new Vector3(pos.X, 0, pos.Y);
    }

    // Update is called once per frame
    void Update()
    {
        World.Player.Position = new Vec2(Player.transform.position.x, Player.transform.position.z);
        UpdatePosition(Player, World.Player.Position); // in case the engine disagrees
        foreach (var remote in World.RemotePlayers) 
        {
            UpdatePosition(RemotePlayers[remote.Guid], remote.Position);
        }
        foreach (var mon in World.Monsters) 
        {
            UpdatePosition(RemoteMonster[mon.Guid], mon.Position);
        }
    }
}
