using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicWorld;
using System;
using System.Linq;

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


    private Dictionary<Guid, GameObject> RemotePlayers = new Dictionary<Guid, GameObject>();

    private Dictionary<Guid, GameObject> RemoteMonster = new Dictionary<Guid, GameObject>();

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
        RemoveRemotePlayer(id);
        var remotePlayerInstance = Instantiate(RemotePlayerPrefab);
        RemotePlayers.Add(id, remotePlayerInstance);
        remotePlayerInstance.transform.SetParent(WorldRoot);
        UpdatePosition(remotePlayerInstance, pos);
    }

    void RemoveRemotePlayer(Guid id) 
    {
        if (RemotePlayers.ContainsKey(id)) 
        {
            RemotePlayers[id].transform.SetParent(null);
            RemotePlayers.Remove(id);
        }
    }

    void SpawnMonster(Guid id, Vec2 pos) 
    {
        RemoveMonster(id);
        var remoteMonsterInstance = Instantiate(MonsterPrefab);
        RemoteMonster.Add(id, remoteMonsterInstance);
        remoteMonsterInstance.transform.SetParent(WorldRoot);
        UpdatePosition(remoteMonsterInstance, pos);
    }

    private void RemoveMonster(Guid id)
    {
        if (RemoteMonster.ContainsKey(id)) 
        {
            RemoteMonster[id].transform.SetParent(null);
            RemoteMonster.Remove(id);
        }
    }

    void UpdatePosition(GameObject gameObj, Vec2 pos) {
        gameObj.transform.position = new Vector3(pos.X, 0, pos.Y);
    }

    // Update is called once per frame
    void Update()
    {
        World.Player.Position = new Vec2(Player.transform.position.x, Player.transform.position.z);
        UpdatePosition(Player, World.Player.Position); // in case the engine disagrees

        foreach (var remote in World.RemotePlayers.Where(p => !RemotePlayers.ContainsKey(p.Guid))) {
            SpawnRemotePlayer(remote.Guid, remote.Position);
        }
        foreach (var remote in RemotePlayers.Where(p => World.RemotePlayers.All(q => q.Guid != p.Key))) {
            RemoveRemotePlayer(remote.Key);
        }

        foreach (var remote in World.RemotePlayers) 
        {
            UpdatePosition(RemotePlayers[remote.Guid], remote.Position);
        }

        
        foreach (var remote in World.Monsters.Where(p => !RemoteMonster.ContainsKey(p.Guid))) {
            SpawnMonster(remote.Guid, remote.Position);
        }
        foreach (var remote in RemoteMonster.Where(p => World.Monsters.All(q => q.Guid != p.Key))) {
            RemoveMonster(remote.Key);
        }

        foreach (var mon in World.Monsters) 
        {
            UpdatePosition(RemoteMonster[mon.Guid], mon.Position);
        }
    }
}
