using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [Header("Movement")]
    public float vehicleMoveSpeed;
    public float vehicleTurnSpeed;
    public float vehicleMaxSpeed;
    public float vehicleDampFactor;
    public Rigidbody2D vehicleRB;
    public Collider2D vehicleCollider;

    [Header ("Attribute")]
    [SerializeField] Collider2D vehicleTrigger;
    [SerializeField] private float vehicleMaxHP;
    [SerializeField] private float vehicleCurrentHP;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    [Header ("UI")]
    [SerializeField] private GameObject approach;

    private void Start()
    {
        vehicleRB = GetComponent<Rigidbody2D>();
        vehicleCollider = GetComponent<Collider2D>();
        vehicleCurrentHP = vehicleMaxHP;
        approach.SetActive(false);
        defaultPosition = this.transform.position;
        defaultRotation = this.transform.rotation;
    }

    private void TakeDamage()
    {
        vehicleCurrentHP -= vehicleRB.velocity.magnitude / 100.0f;
    }

    public void ResetPosition()
    {
        this.transform.position = defaultPosition;
        this.transform.rotation = defaultRotation;
        vehicleRB = GetComponent<Rigidbody2D>();
        vehicleCollider = GetComponent<Collider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                Debug.Log(this.transform.name + " has Detect Player");
                if (player.onVehicle) return;
                player.GetVehicle(this.gameObject.GetComponent<Vehicle>());
                approach.SetActive(true);
                player.HowtoRide();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                if (player.onVehicle)
                {
                    approach.SetActive(false);
                    player.GetDown();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            approach.SetActive(false);
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                player.ResetVehicle();
                player.Shutit();
            }
        }
    }

}
