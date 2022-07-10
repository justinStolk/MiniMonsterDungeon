using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private DungeonGeneratorV2 dungeonGenerator;
    [SerializeField] private float camMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * camMoveSpeed * Time.deltaTime;
        Vector3 targetPosition = transform.position + new Vector3(movement.x, movement.y, 0);

        float dWidth = dungeonGenerator.settings.MaxDungeonWidth * 0.5f * dungeonGenerator.settings.RoomWidth * dungeonGenerator.settings.RoomDistance;
        float dHeight = dungeonGenerator.settings.MaxDungeonHeight * 0.5f * dungeonGenerator.settings.RoomHeight * dungeonGenerator.settings.RoomDistance;

        float xVal = Mathf.Clamp(targetPosition.x, 0, dWidth);
        float yVal = Mathf.Clamp(targetPosition.y, 0, dHeight);

        transform.position = new Vector3(xVal, yVal, transform.position.z);
    }
}
