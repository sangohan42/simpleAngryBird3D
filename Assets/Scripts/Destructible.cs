using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyedVersion; // Reference to the shattered version of the object

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                // Spawn a shattered object
                Instantiate(destroyedVersion, transform.position, transform.rotation);
                // Remove the current object
                Destroy(gameObject);
                break;
        }
    }
}