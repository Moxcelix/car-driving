using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CarMirror : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _reflectionMaterial;

    private MeshRenderer _meshRenderer;
    private RenderTexture _texture;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _texture = new RenderTexture(128, 128, 1);
        _meshRenderer.material.mainTexture = _texture;
    }

    public void SetActive(bool active)
    {
        _camera.targetTexture = active ? _texture : null;
        _camera.enabled = active;

        _meshRenderer.material = active ? _reflectionMaterial : _defaultMaterial;
    }
}
