using System;
using UnityEngine;

[Serializable]
public class CarMirror
{
    [SerializeField] private RenderTexture _texture;
    [SerializeField] private Camera _camera;

    public void SetActive(bool active)
    {
        _camera.targetTexture = active ? _texture : null;
    }
}
