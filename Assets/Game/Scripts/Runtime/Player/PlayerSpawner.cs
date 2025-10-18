using System;
using Game.Scripts.Runtime;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
   #region Members

   [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
   [SerializeField] private GameObject _playerPrefab;
   [SerializeField] private Transform _spawnPoint;
   [SerializeField] private CinemachineCamera _camera;

   #endregion


   private GameObject _player;

   #region Unity Functions

   private void Start()
   {
      _player = Instantiate(_playerPrefab, _spawnPoint);
      _player.GetComponent<PlayerController>().CameraTransform = _camera.transform;
      _camera.Follow = _player.transform;
      _camera.LookAt = _player.transform;
   }

   #endregion
}
