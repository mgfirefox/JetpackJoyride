using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject[] availableRooms;
    [SerializeField]
    private List<GameObject> currentRooms;

    [SerializeField]
    private GameObject[] availableObjects;
    [SerializeField]
    private List<GameObject> currentObjects;

    [SerializeField]
    private Vector2 objectsMinOffset = new Vector2(5.0f, -1.0f);
    [SerializeField]
    private Vector2 objectsMaxOffset = new Vector2(10.0f, 1.0f);

    private float screenWidth;

    private void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidth = height * Camera.main.aspect;
    }

    private void FixedUpdate()
    {
        GenerateRoomIfRequired();
        GenerateObjectIfRequired();
    }

    private void GenerateRoomIfRequired()
    {
        var roomsToRemove = new List<GameObject>();

        bool isRoomGeneratingRequired = true;

        float playerX = player.transform.position.x;

        float removedRoomX = playerX - screenWidth;
        float addedRoomX = playerX + screenWidth;

        float farthestRoomEndX = 0.0f;

        foreach (var room in currentRooms)
        {
            float roomWidth = room.transform.Find("Floor").localScale.x;
            float roomStartX = room.transform.position.x - roomWidth / 2;
            float roomEndX = roomStartX + roomWidth;
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);

            if (roomStartX > addedRoomX)
            {
                isRoomGeneratingRequired = false;
            }
            if (roomEndX < removedRoomX)
            {
                roomsToRemove.Add(room);
            }
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (isRoomGeneratingRequired)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    private void AddRoom(float farthestRoomEndX)
    {
        int randomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = Instantiate(availableRooms[randomIndex]);

        float roomWidth = room.transform.Find("Floor").localScale.x;
        float roomCenter = farthestRoomEndX + roomWidth / 2;
        room.transform.position = new Vector2(roomCenter, 0);

        currentRooms.Add(room);
    }

    private void GenerateObjectIfRequired()
    {
        var objectsToRemove = new List<GameObject>();
        
        float playerX = player.transform.position.x;

        float removedObjectX = playerX - screenWidth;
        float addedObjectX = playerX + screenWidth;

        float farthestObjectX = 0.0f;

        foreach (var theObject in currentObjects)
        {
            float objectX = theObject.transform.position.x;
            farthestObjectX = Mathf.Max(farthestObjectX, objectX);

            if (objectX < removedObjectX)
            {
                objectsToRemove.Add(theObject);
            }
        }

        foreach (var theObject in objectsToRemove)
        {
            currentObjects.Remove(theObject);
            Destroy(theObject);
        }

        if (farthestObjectX < addedObjectX)
        {
            AddObject(farthestObjectX);
        }
    }

    private void AddObject(float farthestObjectX)
    {
        int randomIndex = Random.Range(0, availableObjects.Length);
        GameObject theObject = Instantiate(availableObjects[randomIndex]);

        theObject.transform.position = new Vector2(farthestObjectX + Random.Range(objectsMinOffset.x, objectsMaxOffset.x), Random.Range(objectsMinOffset.y, objectsMaxOffset.y));
        
        currentObjects.Add(theObject);
    }
}
