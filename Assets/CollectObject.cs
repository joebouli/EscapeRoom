using UnityEngine;

public class Collector : MonoBehaviour
{
    public string collectibleTag = "Collectible";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(collectibleTag))
        {
            CollectObject(other.gameObject);
        }
    }

    private void CollectObject(GameObject collectible)
    {

        Destroy(collectible);

    }
}
