using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] public int Value;
    [SerializeField] public AudioClip PickedUpSound;

    private void Update()
    {
        transform.Rotate(Vector3.up, 60 * Time.deltaTime);
    }
}
