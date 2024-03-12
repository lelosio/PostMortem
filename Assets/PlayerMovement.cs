using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 10f;
    public float momentumDamping = 5f;
    private CharacterController myCC;
    public Animator camAnim;
    private bool isWalking;

    private Vector3 inputVector;
    private Vector3 movementVector;
    private float myGravity = -10f;

    public float sensitivity = 1.5f;
    public float smoothing = 1.5f;
    private float xMousePos;
    private float yMousePos;
    private float smoothedMousePos;
    private float currentLookingPos;

    private float fov;
    private float fovSpeed;
    private float moveY;
    private float moveX;

    public Camera playerCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myCC = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        moveY = Input.GetAxisRaw("Horizontal");
        moveX = Input.GetAxisRaw("Vertical");

        #region Handles rotation
            xMousePos = Input.GetAxisRaw("Mouse X");
            xMousePos *= sensitivity * smoothing;
            smoothedMousePos = Mathf.Lerp(smoothedMousePos, xMousePos, 1f / smoothing);
            currentLookingPos += smoothedMousePos;
            transform.localRotation = Quaternion.AngleAxis(currentLookingPos, transform.up);

            yMousePos += -Input.GetAxis("Mouse Y") * sensitivity;
            yMousePos = Mathf.Clamp(yMousePos, -50, 50);
            playerCamera.transform.localRotation = Quaternion.Euler(yMousePos, 0, 0);

        #endregion

        // #region Handles HeadBob
        //     if (moveY > 0.1)
        //     {
        //         playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, Quaternion.Euler(playerCamera.transform.localRotation.x, playerCamera.transform.localRotation.y, -5f), Time.deltaTime / 0.1f);
        //     }
        //     else 
        //     {
        //         playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, Quaternion.Euler(playerCamera.transform.localRotation.x, playerCamera.transform.localRotation.y, 0), Time.deltaTime / 0.1f);
        //     }
        //     if (moveY < -0.1)
        //     {
        //         playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, Quaternion.Euler(playerCamera.transform.localRotation.x, playerCamera.transform.localRotation.y, 5f), Time.deltaTime / 0.1f);
        //     }
        //     else
        //     {
        //         playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation, Quaternion.Euler(playerCamera.transform.localRotation.x, playerCamera.transform.localRotation.y, 0), Time.deltaTime / 0.1f);
        //     }
        // #endregion

        #region Handles movement
        inputVector = new Vector3(moveY, 0f , moveX);
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);

            movementVector = (inputVector * playerSpeed) + (Vector3.up * myGravity);
            myCC.Move(movementVector * Time.deltaTime);

            if(myCC.velocity.magnitude >0.1f)
            {
                isWalking = true;
            }
            else
            {
                inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);
                isWalking = false;
            }

            camAnim.SetBool("isWalking", isWalking);
        #endregion
    
        #region Handles FOV
            fov = 90f;
            fovSpeed = 0.3f;

            if (moveX > 0.1 || moveX < -0.1 || moveY > 0.1 || moveY < -0.1)
            {
                fov = 110f;
            }

            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, Time.deltaTime / fovSpeed);
        #endregion

    }
}