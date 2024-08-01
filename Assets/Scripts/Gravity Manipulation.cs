using UnityEngine;

public class GravityManipulation : MonoBehaviour
{
    public GameObject hologramPrefab;
    public Transform hologramPosition;
    public Transform environment;  
    public Transform player;      


    private Vector3 rotationLeft = new Vector3(0, 0, -90);
    private Vector3 rotationRight = new Vector3(0, 0, 90);
    private Vector3 rotationFront = new Vector3(-90, 0, 0);
    private Vector3 rotationBack = new Vector3(90, 0, 0);
    private Vector3 rotationUp = new Vector3(0, 0, 180);


    [Header("Cube1 Rotations")]
    public Transform Cube1Up;
    public Transform Cube1Left;
    public Transform Cube1Right;
    public Transform Cube1Front;
    public Transform Cube1Back;

    [Header("Cube2 Rotations")]
    public Transform Cube2Up;
    public Transform Cube2Left;
    public Transform Cube2Right;
    public Transform Cube2Front;
    public Transform Cube2Back;

    [Header("Cube3 Rotations")]
    public Transform Cube3Up;
    public Transform Cube3Left;
    public Transform Cube3Right;
    public Transform Cube3Front;
    public Transform Cube3Back;

    [Header("Cube4 Rotations")]
    public Transform Cube4Up;
    public Transform Cube4Left;
    public Transform Cube4Right;
    public Transform Cube4Front;
    public Transform Cube4Back;

    [Header("Cube5 Rotations")]
    public Transform Cube5Up;
    public Transform Cube5Left;
    public Transform Cube5Right;
    public Transform Cube5Front;
    public Transform Cube5Back;

    [Header("Cube6 Rotations")]
    public Transform Cube6Up;
    public Transform Cube6Left;
    public Transform Cube6Right;
    public Transform Cube6Front;
    public Transform Cube6Back;

    [Header("Cube7 Rotations")]
    public Transform Cube7Up;
    public Transform Cube7Left;
    public Transform Cube7Right;
    public Transform Cube7Front;
    public Transform Cube7Back;

    [Header("Cube8 Rotations")]
    public Transform Cube8Up;
    public Transform Cube8Left;
    public Transform Cube8Right;
    public Transform Cube8Front;
    public Transform Cube8Back;

    private GameObject currentHologram;
    private string selectedDirection = "";
    private string currentCube = "";


    void Start()
    {
        if (hologramPrefab == null || hologramPosition == null || environment == null || player == null)
        {
            Debug.LogError("Required references are not assigned!");
            return;
        }

        InstantiateHologram();
    }

    void Update()
    {
        DetectCurrentCube();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedDirection = "Left";
            ShowHologram(rotationLeft);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedDirection = "Right";
            ShowHologram(rotationRight);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
        {
            selectedDirection = "Front";
            ShowHologram(rotationFront);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedDirection = "Back";
            ShowHologram(rotationBack);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
        {
            selectedDirection = "Up";
            ShowHologram(rotationUp);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyRotation();
        }
    }

    private void ShowHologram(Vector3 rotation)
    {
        if (currentHologram == null)
        {
            InstantiateHologram();
        }

        currentHologram.transform.rotation = Quaternion.Euler(rotation);
        currentHologram.SetActive(true);

        Debug.Log($"Showing hologram with rotation: {rotation} and direction: {selectedDirection}");
    }

    private void InstantiateHologram()
    {
        currentHologram = Instantiate(hologramPrefab, hologramPosition.position, Quaternion.identity, hologramPosition);
        currentHologram.SetActive(false);
    }

    private void ApplyRotation()
    {
        Vector3 newRotation = Vector3.zero;
        Transform spawnPoint = null;

        switch (currentCube)
        {
            case "Cube1":
                newRotation = GetAdjustedRotation(GetCube1Rotation());
                spawnPoint = GetSpawnPointForCube1(selectedDirection);
                break;
            case "Cube2":
                newRotation = GetAdjustedRotation(GetCube2Rotation());
                spawnPoint = GetSpawnPointForCube2(selectedDirection);
                break;
            case "Cube3":
                newRotation = GetAdjustedRotation(GetCube3Rotation());
                spawnPoint = GetSpawnPointForCube3(selectedDirection);
                break;
            case "Cube4":
                newRotation = GetAdjustedRotation(GetCube4Rotation());
                spawnPoint = GetSpawnPointForCube4(selectedDirection);
                break;
            case "Cube5":
                newRotation = GetAdjustedRotation(GetCube5Rotation());
                spawnPoint = GetSpawnPointForCube5(selectedDirection);
                break;
            case "Cube6":
                newRotation = GetAdjustedRotation(GetCube6Rotation());
                spawnPoint = GetSpawnPointForCube6(selectedDirection);
                break;
            case "Cube7":
                newRotation = GetAdjustedRotation(GetCube7Rotation());
                spawnPoint = GetSpawnPointForCube7(selectedDirection);
                break;
            case "Cube8":
                newRotation = GetAdjustedRotation(GetCube8Rotation());
                spawnPoint = GetSpawnPointForCube8(selectedDirection);
                break;
            default:
                Debug.LogWarning("Player is not standing on a cube with a known name.");
                return;
        }

        Debug.Log($"Applying rotation: {newRotation} to the environment.");

        environment.rotation = Quaternion.Euler(newRotation);

        if (spawnPoint != null)
        {
            player.position = spawnPoint.position;
            Debug.Log($"Player spawned at: {spawnPoint.position}.");
        }
        else
        {
            Debug.LogWarning("Spawn point not found for the current cube and direction.");
        }

        if (currentHologram != null)
        {
            Destroy(currentHologram);
            currentHologram = null;
        }
    }

    private Vector3 GetAdjustedRotation(Vector3 targetRotation)
    {
        Vector3 currentPlayerRotation = player.eulerAngles;
        Vector3 adjustedRotation = targetRotation - currentPlayerRotation;

        return adjustedRotation;
    }

    private Vector3 GetCube1Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(0, 0, 180);
            case "Left":
                return new Vector3(0, 0, 90);
            case "Right":
                return new Vector3(0, 0, -90);
            case "Front":
                return new Vector3(90, 0, 0);
            case "Back":
                return new Vector3(-90, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube2Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(-270, 0, 0);
            case "Left":
                return new Vector3(0, -90, 90);
            case "Right":
                return new Vector3(0, 90, -90);
            case "Front":
                return new Vector3(0, 0, 0);
            case "Back":
                return new Vector3(-180, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube3Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(-180, 0, 180);
            case "Left":
                return new Vector3(-180, 0, -90);
            case "Right":
                return new Vector3(-180, 0, 90);
            case "Front":
                return new Vector3(-90, 0, 0);
            case "Back":
                return new Vector3(90, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube4Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(90, 0, 0);
            case "Left":
                return new Vector3(0, -90, 90);
            case "Right":
                return new Vector3(0, 90, -90);
            case "Front":
                return new Vector3(0, 0, 0);
            case "Back":
                return new Vector3(180, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube5Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(270, 0, 0);
            case "Left":
                return new Vector3(0, 90, 90);
            case "Right":
                return new Vector3(0, -90, -90);
            case "Front":
                return new Vector3(180, 0, 0);
            case "Back":
                return new Vector3(0, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube6Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(-360, 0, 0);
            case "Left":
                return new Vector3(-180, 0, -90);
            case "Right":
                return new Vector3(-180, 0, 90);
            case "Front":
                return new Vector3(-90, 0, 0);
            case "Back":
                return new Vector3(90, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube7Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(180, 0, 0);
            case "Left":
                return new Vector3(0, 0, 90);
            case "Right":
                return new Vector3(0, 0, -90);
            case "Front":
                return new Vector3(-90, 0, 0);
            case "Back":
                return new Vector3(90, 0, 0);
            default:
                return Vector3.zero;
        }
    }

    private Vector3 GetCube8Rotation()
    {
        switch (selectedDirection)
        {
            case "Up":
                return new Vector3(-360, 0, -90);
            case "Left":
                return new Vector3(0, 0, 0);
            case "Right":
                return new Vector3(0, 0, -180);
            case "Front":
                return new Vector3(90, 0, -90);
            case "Back":
                return new Vector3(-90, 0, -90);
            default:
                return Vector3.zero;
        }
    }

    private Transform GetSpawnPointForCube1(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube1Up;
            case "Left":
                return Cube1Left;
            case "Right":
                return Cube1Right;
            case "Front":
                return Cube1Front;
            case "Back":
                return Cube1Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube2(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube2Up;
            case "Left":
                return Cube2Left;
            case "Right":
                return Cube2Right;
            case "Front":
                return Cube2Front;
            case "Back":
                return Cube2Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube3(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube3Up;
            case "Left":
                return Cube3Left;
            case "Right":
                return Cube3Right;
            case "Front":
                return Cube3Front;
            case "Back":
                return Cube3Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube4(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube4Up;
            case "Left":
                return Cube4Left;
            case "Right":
                return Cube4Right;
            case "Front":
                return Cube4Front;
            case "Back":
                return Cube4Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube5(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube5Up;
            case "Left":
                return Cube5Left;
            case "Right":
                return Cube5Right;
            case "Front":
                return Cube5Front;
            case "Back":
                return Cube5Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube6(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube6Up;
            case "Left":
                return Cube6Left;
            case "Right":
                return Cube6Right;
            case "Front":
                return Cube6Front;
            case "Back":
                return Cube6Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube7(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube7Up;
            case "Left":
                return Cube7Left;
            case "Right":
                return Cube7Right;
            case "Front":
                return Cube7Front;
            case "Back":
                return Cube7Back;
            default:
                return null;
        }
    }

    private Transform GetSpawnPointForCube8(string direction)
    {
        switch (direction)
        {
            case "Up":
                return Cube8Up;
            case "Left":
                return Cube8Left;
            case "Right":
                return Cube8Right;
            case "Front":
                return Cube8Front;
            case "Back":
                return Cube8Back;
            default:
                return null;
        }
    }

    private void DetectCurrentCube()
    {

        Ray ray = new Ray(player.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2.0f))
        {
            if (hit.collider != null && hit.collider.gameObject != null)
            {
                currentCube = hit.collider.gameObject.name;
            }
        }
    }

   


}