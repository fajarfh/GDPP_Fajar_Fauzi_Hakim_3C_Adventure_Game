using UnityEngine;

public class FailSafe : MonoBehaviour
{
    private Transform _respawnPoint;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _respawnPoint = transform;
        if (_collider == null)
        {
            Debug.LogError("FailSafe " + gameObject.name + " requires a Collider component on the same GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerMovement>())
        {
            other.transform.position = _respawnPoint.position;
        }
    }
}
