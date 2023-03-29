using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField] private float _activeTime;
    [SerializeField] private float _meshRefreshRate;
    [SerializeField] private float _destroyTime;
    
    private bool _isActive;
    private SkinnedMeshRenderer[] _skinnedMeshRenderer;

    [Header("Shader")]
    [SerializeField] private Material _material;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.Space) && !_isActive)
        // {
        //     _isActive = true;
        //     StartCoroutine(ActivateTrail(_activeTime));
        // }
        
    }

    public IEnumerator ActivateTrail(float timeActive)
    {
        while (true)
        {
            timeActive -= _meshRefreshRate;

            if (_skinnedMeshRenderer == null)
                _skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < _skinnedMeshRenderer.Length; i++)
            {
                GameObject newMesh = new GameObject();
                newMesh.transform.SetPositionAndRotation(transform.position, transform.rotation);
                
                MeshRenderer meshRenderer = newMesh.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = newMesh.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                _skinnedMeshRenderer[i].BakeMesh(mesh );
                
                meshFilter.mesh = mesh;
                meshRenderer.material = _material;
                
                Destroy(newMesh, _destroyTime);
            }
            

            yield return new WaitForSeconds(_meshRefreshRate);
        }

        _isActive = false;
    }
}
