using System;
using Unity.Mathematics;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float CharacterForwardSpeed;
    [SerializeField] private float WidthOfTrack;
    private float PositionOnTrack;
    private float Destination = 0;

    private bool Turning;
    private float TurningRadius;
    private float TurningTime;
    private int StartingRotation;

    [SerializeField] AudioClip[] Footsteps;
    private float LastStep;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<TurningTrigger>() != null) 
        {         
            Turning = true;
            TurningRadius = other.GetComponent<TurningTrigger>().RotationCenter - PositionOnTrack;
            TurningTime = MathF.Abs(TurningRadius) * Mathf.PI / 2 / CharacterForwardSpeed;
            StartingRotation = (int)gameObject.transform.rotation.y / 90;
            Debug.Log("Started turning with radius " + TurningRadius);
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (LastStep >= 1 / CharacterForwardSpeed * 0.6f)
        {
            LastStep = 0;
            System.Random rand = new System.Random();
            gameObject.GetComponent<AudioSource>().resource = Footsteps[rand.Next(5)];
            gameObject.GetComponent<AudioSource>().Play();
        }
        LastStep += Time.deltaTime;
        if (Turning)
        {            
            if (Mathf.Abs((StartingRotation * 90) - gameObject.transform.rotation.eulerAngles.y) < 90)
            {
                transform.Rotate(Vector3.up, 90 * Mathf.Sign(TurningRadius) * Time.deltaTime / TurningTime, Space.World);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, ((int)gameObject.transform.rotation.eulerAngles.y / 90) * 90, 0);                
                Turning = false;
            }
            Vector3 movement = Vector3.Normalize(new Vector3(transform.TransformVector(0,0,1).x,0, transform.TransformVector(0, 0, 1).z));
            movement.y = 0;
            this.GetComponent<CharacterController>().Move(movement * CharacterForwardSpeed * Time.deltaTime);
        }
        else
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Destination = Mathf.Clamp((Input.GetTouch(0).position.x / Screen.width - 0.5f) * 2, -1, 1) * WidthOfTrack;
            }
            else
            {
                if (Input.mousePresent && Input.GetKey(KeyCode.Mouse0))
                {
                    Destination = Mathf.Clamp((Input.mousePosition.x / Screen.width - 0.5f) * 2, -1, 1) * WidthOfTrack;
                }
            }
            this.GetComponent<CharacterController>().Move(transform.TransformVector(new Vector3(Destination - PositionOnTrack, 0, CharacterForwardSpeed * Time.deltaTime)));            
            PositionOnTrack = Destination;
        }
        this.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
    }
}
