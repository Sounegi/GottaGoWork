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
    public int cost;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    [Header ("UI")]
    [SerializeField] private GameObject approach;
    [SerializeField] private GameObject approachText;
    [SerializeField] private GameObject hpBar;

    private void Start()
    {
        vehicleRB = GetComponent<Rigidbody2D>();
        vehicleCollider = GetComponent<Collider2D>();
        vehicleCurrentHP = vehicleMaxHP;
        approach.SetActive(false);
        //hpBar.SetActive(false);
        defaultPosition = this.transform.position;
        defaultRotation = this.transform.rotation;
        vehicleCollider.isTrigger = false;
    }

    private void TakeDamage()
    {
        if (vehicleRB.velocity.magnitude <= 2.0f)
            return;
        vehicleCurrentHP -= vehicleRB.velocity.magnitude;
        UpdateHealthBar();
        if (vehicleCurrentHP <= 0.0f)
        {
            PlayerController.playerInstance.DropFromVehicle();
            vehicleCollider.isTrigger = true;
        }
    }

    public void ShowHpBar()
    {
        hpBar.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        
        hpBar.transform.localScale = new Vector3(vehicleCurrentHP/vehicleMaxHP, 0.2f, 1f);
    }

    public void ResetPosition()
    {
        this.transform.position = defaultPosition;
        this.transform.rotation = defaultRotation;
        vehicleRB = GetComponent<Rigidbody2D>();
        vehicleCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            TakeDamage();
        }
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
                approachText.SetActive(true);
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
                    approachText.SetActive(false);
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
            approachText.SetActive(false);
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                player.ResetVehicle();
                player.Shutit();
            }
        }
    }

}
