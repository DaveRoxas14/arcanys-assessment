using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
   [SerializeField] private GameObject _playerPrefab;
   [SerializeField] private Transform _spawnPoint;
   [SerializeField] private CinemachineCamera _camera;


   private GameObject _player;
   
   private void Start()
   {
      _player = Instantiate(_playerPrefab, _spawnPoint);
      _player.GetComponent<PlayerController>().CameraTransform = _camera.transform;
      _camera.Follow = _player.transform;
      _camera.LookAt = _player.transform;
   }
}
