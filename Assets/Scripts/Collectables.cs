using UnityEngine;

public class Collectables : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {

        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if (gameManager != null)
            {
                gameManager.CollectCube();
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("GameManager reference is null!");
            }
        }
    }
}
