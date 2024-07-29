using UnityEngine;

public class Gravityr : MonoBehaviour
{
    private string currentDirection = "";
    private string selectedDirection = "";

    public GameObject hologramPrefab;
    public Transform player; 
    public Camera mainCamera;

    private GameObject currentHologram;
    private Transform hologramPosition;

    // Positions for holograms based on direction
    private Vector3 positionLeft = new Vector3(-2.5f, 2.3f, 0); 
    private Vector3 positionRight = new Vector3(2.5f, 2.3f, 0); 
    private Vector3 positionUp = new Vector3(0, 4.7f, 0);

    // Rotations for holograms based on direction
    private Vector3 rotationLeft = new Vector3(-90, 0, -90); 
    private Vector3 rotationRight = new Vector3(-90, 0, 90); 
    private Vector3 rotationUp = new Vector3(0, 0, -180);

    private Vector3 gravityDirection = Vector3.down;

    void Start()
    {
        if (hologramPrefab == null || player == null || mainCamera == null)
        {
            Debug.LogError("Required references are not assigned!");
            return;
        }

        hologramPosition = GameObject.Find("HologramPosition")?.transform;

        if (hologramPosition == null)
        {
            Debug.LogError("HologramPosition GameObject not found in the scene!");
            return;
        }

        currentHologram = Instantiate(hologramPrefab, hologramPosition.position, Quaternion.identity);
        currentHologram.SetActive(false); 
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedDirection = "Up";
            ShowHologram(positionUp, rotationUp);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedDirection = "Left";
            ShowHologram(positionLeft, rotationLeft);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedDirection = "Right";
            ShowHologram(positionRight, rotationRight);
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyDirection();
        }


        Debug.Log("Current Direction: " + selectedDirection);
    }

    private void ShowHologram(Vector3 position, Vector3 rotation)
    {
        if (currentHologram != null)
        {
            Destroy(currentHologram);
        }


        currentHologram = Instantiate(hologramPrefab, hologramPosition.position + position, Quaternion.Euler(rotation));
        currentHologram.SetActive(true);
    }

    private void ApplyDirection()
    {
        Vector3 position = Vector3.zero;
        Vector3 rotation = Vector3.zero;
        Vector3 newGravityDirection = Vector3.down;

        switch (selectedDirection)
        {
            case "Up":
                position = positionUp;
                rotation = rotationUp;
                newGravityDirection = Vector3.up;
                break;
            case "Left":
                position = positionLeft;
                rotation = rotationLeft;
                newGravityDirection = Vector3.left;
                break;
            case "Right":
                position = positionRight;
                rotation = rotationRight;
                newGravityDirection = Vector3.right;
                break;
        }


        Physics.gravity = newGravityDirection * 9.81f;

        player.position = hologramPosition.position + position;
        player.rotation = Quaternion.Euler(rotation);

        AdjustCameraRotation(rotation);
    }

    private void AdjustCameraRotation(Vector3 playerRotation)
    {
        mainCamera.transform.rotation = Quaternion.Euler(playerRotation);
    }

    public string GetCurrentDirection()
    {
        return selectedDirection;
    }
}
