using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerInstance;
    [SerializeField] Transform startingPoint;
    [SerializeField] TMPro.TextMeshProUGUI how2;
    [SerializeField] SpriteRenderer playerSprite;

    [Header("Default Value")]
    public float moveSpeed = 1;
    public float angularSpeed = 3f;
    public float maxSpeed = 5;
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D playerCollider;
    private Quaternion defaultRotation;

    [Header("Current Value")]
    public float curMoveSpeed = 1;
    public float curAngularSpeed = 3f;
    public float curMaxSpeed = 5;
    public float dampFactor = 0.1f;
    [SerializeField] Rigidbody2D currentRB;
    [SerializeField] Transform currentTransform;
    [SerializeField] Collider2D currentCollider;
    public bool onVehicle = false;
    private Vehicle nearbyVehicle;
    private Station nearbyStation;
    public bool canWin = false;

    public bool PressForward { get; private set; }
    public bool TurnLeft { get; private set; }
    public bool TurnRight { get; private set; }
    public bool ChangeVehicle { get; private set; }

    private PlayerInput _playerInput;
    private InputAction _moveForwardAction;
    private InputAction _turnLeftAction;
    private InputAction _turnRightAction;
    private InputAction _changeAction;

    private void Start()
    {
        playerInstance = this;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        this.transform.position = startingPoint.position;
        playerRB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        _playerInput = GetComponent<PlayerInput>();
        defaultRotation = this.transform.rotation;
        SetupInputs();
        SetupDefaultValue();
        Shutit();
    }

    private void Update()
    {
        UpdateInputs();
        if (ChangeVehicle)
        {
            if (canWin)
            {
                //TODO: Win the Game
                GameManager.instance.CheckWin();
                return;
            }
            if (onVehicle)
            {
                DropFromVehicle();
            }
            else
            {
                if(nearbyVehicle != null)
                {
                    GetOnVehicle(nearbyVehicle);
                }
                else
                {
                    if(nearbyStation != null)
                    {
                        Vehicle newVehicle = nearbyStation.SpawnVehicle();
                        GetOnVehicle(newVehicle);
                    }
                }
            }
            Debug.Log("Do something");
        }
    }

    private void FixedUpdate()
    {
        if (PressForward)
        {
            currentRB.AddForce(transform.up * curMoveSpeed, ForceMode2D.Force);
            if(currentRB.velocity.magnitude > curMaxSpeed)
            {
                currentRB.velocity = currentRB.velocity.normalized * curMaxSpeed;
            }
            
        }
        else
        {
            if(currentRB == playerRB)
            {
                currentRB.velocity = Vector3.zero;
            }
            else
            {
                currentRB.velocity *= (1 - dampFactor);
            }
            
        }
        if (TurnLeft)
        {
            currentTransform.Rotate(Vector3.forward, curAngularSpeed);

        }
        if (TurnRight)
        {
            currentTransform.Rotate(Vector3.forward, -curAngularSpeed);
        }
        if (onVehicle)
        {
            playerSprite.enabled = false;
        }
        else
        {
            playerSprite.enabled = true;
        }

    }

    private void SetupDefaultValue()
    {
        curMoveSpeed = moveSpeed;
        curAngularSpeed = angularSpeed;
        curMaxSpeed = maxSpeed;
        currentRB = playerRB;
        currentCollider = playerCollider;
        currentTransform = this.transform;
        onVehicle = false;
        canWin = false;
    }

    private void GetOnVehicle(Vehicle selectedVehicle)
    {
        curMoveSpeed = selectedVehicle.vehicleMoveSpeed;
        curAngularSpeed = selectedVehicle.vehicleTurnSpeed;
        curMaxSpeed = selectedVehicle.vehicleMaxSpeed;
        dampFactor = selectedVehicle.vehicleDampFactor;
        currentRB = selectedVehicle.vehicleRB;
        playerRB.isKinematic = true;
        currentCollider = selectedVehicle.vehicleCollider;
        onVehicle = true;
        this.transform.position = selectedVehicle.transform.position;
        this.transform.rotation = selectedVehicle.transform.rotation;
        playerCollider.isTrigger = true;
        this.transform.SetParent(selectedVehicle.transform);
        currentTransform = selectedVehicle.transform;
        //TODO: highlight vehicle, change sprite
    }

    public void DropFromVehicle()
    {
        curMoveSpeed = moveSpeed;
        curAngularSpeed = angularSpeed;
        curMaxSpeed = maxSpeed;
        currentRB = playerRB;
        playerRB.isKinematic = false;
        currentCollider = playerCollider;
        onVehicle = false;
        playerCollider.isTrigger = false;
        this.transform.SetParent(null);
        currentTransform = this.transform;
        this.transform.position = this.transform.position + new Vector3(0.0f,-0.5f);
        //TODO: move position to outside of vehicle
    }

    private void SetupInputs()
    {
        _moveForwardAction = _playerInput.actions["Move"];
        _turnLeftAction = _playerInput.actions["TurnLeft"];
        _turnRightAction = _playerInput.actions["TurnRight"];
        _changeAction = _playerInput.actions["Action"];

    }

    private void UpdateInputs()
    {
        PressForward =　_moveForwardAction.IsPressed();
        TurnLeft = _turnLeftAction.IsPressed();
        TurnRight = _turnRightAction.IsPressed();
        ChangeVehicle = _changeAction.WasPressedThisFrame();

    }

    public void HowtoRide()
    {
        how2.text = "Press Space to Get on the ride.";
    }

    public void GetDown()
    {
        how2.text = "Press Space again to Get down.";
    }

    public void Shutit()
    {
        how2.text = "";
    }

    public void GetVehicle(Vehicle detectedVehicle)
    {
        nearbyVehicle = detectedVehicle;
    }

    public void ResetVehicle()
    {
        nearbyVehicle = null;
    }

    public void GetStation(Station detectedStation)
    {
        nearbyStation = detectedStation;
    }

    public void ResetStation()
    {
        nearbyStation = null;
    }

    public void ResetPosition()
    {
        DropFromVehicle();
        SetupDefaultValue();
        this.transform.rotation = defaultRotation;
        this.transform.position = startingPoint.position;

    }

}
