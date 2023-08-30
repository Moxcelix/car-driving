using System;
using UnityEngine;
using Core.Cameras;

[Serializable]
public class ViewSwitcher
{
    [SerializeField] private MovableCamera[] _cameras;
    [SerializeField] private int _mainCameraIndex;

    private AvatarController _avatarController;
    private int _currentCameraIndex;

    public void Initialize(AvatarController avatarController)
    {
        _avatarController = avatarController;
    }

    public void Enable(bool state)
    {
        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].SetMovable(state);
        }
    }

    public void Switch()
    {
        _currentCameraIndex++;

        if (_currentCameraIndex >= _cameras.Length)
        {
            _currentCameraIndex = 0;
        }
    }

    public void Update()
    {
        if(!_avatarController.EntityController.EntityBody.IsSitting)
        {
            _currentCameraIndex = _mainCameraIndex;
        }

        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].SetActive(_currentCameraIndex == i);
        }
    }
}
