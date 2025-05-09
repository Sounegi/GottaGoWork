using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private GameObject approach;
    [SerializeField] private GameObject vehiclePrefab;
    [SerializeField] private Transform vehicleSpawnPoint;

    private void Start()
    {
        vehiclePrefab.SetActive(false);
        approach.SetActive(false);
    }
    public Vehicle SpawnVehicle()
    {
        
        vehiclePrefab.GetComponent<Vehicle>().ResetPosition();
        vehiclePrefab.transform.position = vehicleSpawnPoint.position;
        vehiclePrefab.SetActive(true);
        
        Vehicle vehicle = vehiclePrefab.GetComponent<Vehicle>();
        return vehicle;
    }

    public void ReloadVehicle()
    {
        Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            approach.SetActive(true);
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                if (player.onVehicle) return;
                Debug.Log(this.transform.name + " has Detect Player");
                player.GetStation(this.gameObject.GetComponent<Station>());
                player.HowtoRide();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                player.ResetStation();
                player.Shutit();
            }
            approach.SetActive(false);
        }
    }
}
