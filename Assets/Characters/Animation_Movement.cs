using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Animation_Movement : MonoBehaviour
{
    // declare refrence variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // varaibles to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;

    // variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;


    float rotationFactorPerFrame = 10.0f; // rotation factor per frame
    float runMultiplier = 15.0f;

// ////
//     private float ySpeed;
//     [SerializeField]
//     private float jumpButtonGracePeriod;

//     [SerializeField]
//     private Transform cameraTransform;

//     private Transform cameraTransfrom;

//     ySpeed += Physics.gravity.y * Time.deltaTime;
// ////

    // Awake is called earier than Start in Unity's event life cycle
    void Awake()
    {
        // initially set reference variables
        playerInput = new PlayerInput();  
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // with access to animator we can check and update animator state

        isWalkingHash = Animator.StringToHash("is walking");
        isRunningHash = Animator.StringToHash("is running");

        // set the player input reference variables
        playerInput.CharacterControls.Move.started += onMovementInput; // start the movment when revive key input   
        playerInput.CharacterControls.Move.canceled += onMovementInput; // stop the movment when letting go the key input
        playerInput.CharacterControls.Move.performed += onMovementInput; // update player input key
        playerInput.CharacterControls.Run.started += onRun; // start to run if player pressed assign key
        playerInput.CharacterControls.Run.canceled += onRun; // stop the running when letting go the key input
    }

    void onRun (InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void handleRotation()
    {
        /// 
        // Vector3 currentPosition = transform.position;

        // Vector3 newPosition = new Vector3(currrentMovement.x, 0, currentMovement.y);

        // Vector3 positionToLookAt = currentPosition + newPosition;
        ///

        Vector3 positionToLookAt;

        // the change in position our character should point to
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            // creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void onMovementInput (InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        // calculate normal working speed
        currentMovement.x = currentMovementInput.x * 5;
        currentMovement.z = currentMovementInput.y * 5;

        // calculate running speed
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleAnimation()
    {   
        // get parameter values from animator
        // swicthing animation state
        bool isWalking = animator.GetBool(isWalkingHash); // localy store current value
        bool isRunning = animator.GetBool(isRunningHash);

        // start walking if movement pressed is true and not already walking
        if (isMovementPressed && ! isWalking) // if the key pressed amd animator walking is false
        {
            animator.SetBool(isWalkingHash, true); // then the parameter become true so that character is walking
        }

        // stop walking if isMovementPressed is false and not already walking
        else if (!isMovementPressed && isWalking) // if walking is true but is not moving
        {
            animator.SetBool(isWalkingHash, false); // then character stop walking
        }

        // run if movement and run pressed are true and not currently running
        if ((isMovementPressed && isRunPressed) && ! isRunning) // check if the character is moveing and the run is pressed
        {
            animator.SetBool(isRunningHash, true);  // then run is true
        }

        // stop running if movement or run pressed are flase and currently running
        else if ((!isMovementPressed || ! isRunPressed) && isRunning) // check if the character stops moveing
        { 
            animator.SetBool(isRunningHash, false); //then trun off the running animation
        }
    }

    // apply proper gravity depending on if the character is grouned or not
    // this function is check and see if character is on the ground
    void handleGravity() 
    {
        if(characterController.isGrounded) { // if character is touching the ground
            float groundedGravity = -.1f;     // then give a little gravity apply to player
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        } 
        
        // if the character is not on the ground 
        else  {
            float gravity = -9.8f;  // the gravity factor will equal to Earth gravity force
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
            //currentRunMovement.x += gravity;
            //currentRunMovement.z += gravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleGravity();
        handleRotation();
        handleAnimation();

// /////
//         float horizontalInput = Input.GetAxis("Horizontal");
//         float verticalInput = Input.GetAxis("Vertical");

//         Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
//         float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

//         float speed = inputMagnitude * 3;
        
//         movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
//         movementDirection.Normalize();

//         Vector3 velocity = movementDirection * speed;
//         velocity.y = ySpeed;

//         characterController.Move(velocity * Time.deltaTime);
// //////
        if (isRunPressed) {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else{
            characterController.Move(currentMovement * Time.deltaTime);
        }
       
    }

    void OnEnable()
    {
        // enable the character controls action map
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        playerInput.CharacterControls.Disable();
    }

    // ////
    // private void OnApplicationFocus(bool focus)
    // {
    //     if (focus)
    //     {
    //         Cursor.lockState = CursorLockMode.Locked;
    //     }
    //     else
    //     {
    //         Cursor.lockState = CursorLockMode.None;
    //     }
    // }
    // ////

}
