using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private CubeColor color;
    public CubeFactory OriginFactory { get; set; }
    [SerializeField]
    private LayerMask _pickUpLayers;
    public void Init(Vector3 position, Quaternion orientation)
    {
        transform.position = position;
        transform.rotation = orientation;
        StartCoroutine(Rotate());
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((_pickUpLayers | (1 << other.gameObject.layer)) == _pickUpLayers)
        {
            other.GetComponent<ICubePicker>()?.PickUpCube(color);
            DeSpawn();
        }
    }
    private void DeSpawn()
    {
        OriginFactory.Reclaim(this);
        StopAllCoroutines();
    }
    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, 1f);
            yield return null;
        }
    }
}
