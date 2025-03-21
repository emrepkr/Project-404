using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0f;
    private CharacterController characterController;

    [Header("Player Movement Status")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float gravity = 10f;

    [Header("Camera")]
    public Camera cam;
    public Transform cameraPivot;
    [SerializeField] private float camSpeed = 5f;
    [SerializeField] private float lookSpeed = 5f;
    [SerializeField] private float lookXLimit = 85f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        InputPlayerMovement();
    }

    void FixedUpdate()
    {
        ChangingPlayerPosition();
    }
    //Update
    #region Movement Input
    private void InputPlayerMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float currentSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * currentSpeedX) + (right * currentSpeedY);
        moveDirection.y = movementDirectionY;

        // Yalnızca Pivot objesi üzerinden dikey dönüş
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
    }
    #endregion
    //FIXED UPDATE
    #region Player Movement
    private void ChangingPlayerPosition()
    {
        //applying movement
        characterController.Move(moveDirection * Time.deltaTime);

        // Pivot objesinin X rotasyonunu ayarla (sadece yukarı-aşağı bakış için)
        cameraPivot.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Karakterin dönüşünü ayarla (sadece yatay dönüş için)
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
    #endregion
}