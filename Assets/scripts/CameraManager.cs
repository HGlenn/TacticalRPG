using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    // Use this for initialization
    void Start()
    {
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        if (GameManager.instance.selectedPlayer != null)
        {
            offset = GameManager.instance.selectedPlayer.transform.position;
            transform.position = new Vector3(offset.x, 15.0f, offset.z - 10f);
        }else
        {
            offset = GameManager.instance.players[0].transform.position;
            transform.position = new Vector3(offset.x, 15.0f, offset.z-10f);
        }
    }
}