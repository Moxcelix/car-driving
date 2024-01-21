using System;
using UnityEngine;

[Serializable]
public class CarMirror
{
    [SerializeField] private Camera _camera;

    private RenderTexture _texture;

    public void SetActive(bool active)
    {
        if(_texture == null)
        {
            _texture = new RenderTexture(128, 128, 1);
        }

        _camera.targetTexture = active ? _texture : null;
        _camera.enabled = active;
    }
}
