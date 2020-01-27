using System.Collections.Generic;
using UnityEngine;
using BasicWorld;
using System;
using System.Linq;
using BasicWorld.LanGame;
using BasicWorld.Math;
using BasicWorld.WebGame;
using BasicWorld.WorldRunner;

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

    private GameObject _player;

    private PlayerController _playerController;


    private readonly Dictionary<Guid, GameObject> _remotePlayers = new Dictionary<Guid, GameObject>();

    private readonly Dictionary<Guid, GameObject> _remoteMonster = new Dictionary<Guid, GameObject>();

    private IWorld _world;

    // Start is called before the first frame update
    void Start()
    {
        CreateLocalWorld();
    }

    private void CreateLocalWorld()
    {
        _world = new LocalWorld();
        //_world = new LanServerWorld();
        //_world = new LanClientWorld(IPAddress.Loopback);
        _world = new WebClientWorld();
        _world.CreateLocalPlayer();
        _world.Start();

        SpawnPlayer(_world.Player.Position);
        foreach (var remote in _world.RemotePlayers) 
        {
            SpawnRemotePlayer(remote.Guid, remote.Position);
        }
        foreach (var mon in _world.Monsters) 
        {
            SpawnMonster(mon.Guid, mon.Position);
        }
    }

    void SpawnPlayer(Vec2 pos) 
    {
        // Destroy old player
        if (_player != null) 
            Destroy(_player);

        _player = Instantiate(PlayerPrefab);
        _playerController = _player.GetComponent<PlayerController>();
        _player.transform.SetParent(WorldRoot);
        Camera.main.transform.SetParent(_player.transform);

        UpdatePosition(_player, pos);
    }
    void SpawnRemotePlayer(Guid id, Vec2 pos) 
    {
        RemoveRemotePlayer(id);
        var remotePlayerInstance = Instantiate(RemotePlayerPrefab);
        _remotePlayers.Add(id, remotePlayerInstance);
        remotePlayerInstance.transform.SetParent(WorldRoot);
        UpdatePosition(remotePlayerInstance, pos);
    }

    void RemoveRemotePlayer(Guid id) 
    {
        if (_remotePlayers.ContainsKey(id)) 
        {
            Destroy(_remotePlayers[id]);
            _remotePlayers.Remove(id);
        }
    }

    void SpawnMonster(Guid id, Vec2 pos) 
    {
        RemoveMonster(id);
        var remoteMonsterInstance = Instantiate(MonsterPrefab);
        _remoteMonster.Add(id, remoteMonsterInstance);
        remoteMonsterInstance.transform.SetParent(WorldRoot);
        UpdatePosition(remoteMonsterInstance, pos);
    }

    private void RemoveMonster(Guid id)
    {
        if (_remoteMonster.ContainsKey(id)) 
        {
            Destroy(_remoteMonster[id]);
            _remoteMonster.Remove(id);
        }
    }

    void UpdatePosition(GameObject gameObj, Vec2 pos) {
        gameObj.transform.position = new Vector3(pos.X, 0, pos.Y);
    }

    // Update is called once per frame
    void Update()
    {
        // Update player position in world
        _world.Player.Position = _playerController.Position;
        _world.Player.Velocity = _playerController.Velocity;
        UpdatePosition(_player, _world.Player.Position); // in case the engine disagrees

        // Add new remote players
        foreach (var remote in _world.RemotePlayers.Where(p => !_remotePlayers.ContainsKey(p.Guid)))
            SpawnRemotePlayer(remote.Guid, remote.SmoothPosition);

        // Remove old remote player
        foreach (var remote in _remotePlayers.Where(p => _world.RemotePlayers.All(q => q.Guid != p.Key)))
            RemoveRemotePlayer(remote.Key);

        // Update all remote player positions
        foreach (var remote in _world.RemotePlayers) 
            UpdatePosition(_remotePlayers[remote.Guid], remote.SmoothPosition);

        // Add new monsters
        foreach (var remote in _world.Monsters.Where(p => !_remoteMonster.ContainsKey(p.Guid)))
            SpawnMonster(remote.Guid, remote.SmoothPosition);

        // Remove old monsters
        foreach (var remote in _remoteMonster.Where(p => _world.Monsters.All(q => q.Guid != p.Key)))
            RemoveMonster(remote.Key);

        // Update all monster positions
        foreach (var mon in _world.Monsters) 
            UpdatePosition(_remoteMonster[mon.Guid], mon.SmoothPosition);
    }
}
