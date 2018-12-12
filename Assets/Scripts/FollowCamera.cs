using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // The transform that the camera will be looking at
    public Transform lookAt;
    // The transform of the camera
    public Transform cameraTransform;
    // This makes a private instance of the camera class named myCamera
    Camera myCamera;
    GameStates gameStates;

    // This float represents the distance between the camera and the player
    private float distance = 10.0f;
    // These are used to hold the cameras current positions and the movement sensitivities
    private float currentX = 0.1f;
    private float currentY = 0.1f;
    private float sensitivityX = 5.0f;
    private float sensitivityY = 1.0f;

    //These will be used to clamp the rotation of the camera around the player
    private const float Y_MIN = 0.0f;
    private const float Y_MAX = 45.0f;

    private void Start()
    {
        /// cameraTransform can be simply set to transform as the script will be
        /// attached to the main camera and will be able to use its transform
        cameraTransform = transform;
        gameStates = FindObjectOfType<GameStates>();
        myCamera = Camera.main;
    }
    private void Update()
    {
        currentX += Input.GetAxis("Horizontal") * sensitivityX;
        currentY += Input.GetAxis("Vertical") * sensitivityY;

        currentY = Mathf.Clamp(currentY, Y_MIN, Y_MAX);  
    }
    /// <summary>
    /// Using LateUpdate here allows the camera position to be calculated
    /// and updated after the players positions changes in the Update method
    /// </summary>
    private void LateUpdate()
    {
        /// The distance variable is set to negative to ensure it follows
        /// at that distance behind the lookAt target.
        /// Quaternion.Euler returns a rotation in the order of Z, X, Y
        Vector3 direction = new Vector3(0.0f, 0.0f, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0.0f);
        cameraTransform.position = lookAt.position + rotation * direction;
        cameraTransform.LookAt(lookAt.position);
    }
}
