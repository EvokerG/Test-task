using NUnit.Framework;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    public int Score;
    private int prevScore;
    [SerializeField] public GameObject PlayerModel;
    [SerializeField] private GameObject[] looks;

    private float TimeFromLastPickup;

    [SerializeField] GameObject Slider;
    [SerializeField] GameObject Title;

    private void OnTriggerEnter(Collider other)
    { 
        if (other.GetComponent<Pickup>() != null)
        {
            TimeFromLastPickup = 0;
            Score += other.GetComponent<Pickup>().Value;
            gameObject.GetComponent<AudioSource>().resource = other.GetComponent<Pickup>().PickedUpSound;
            gameObject.GetComponent<AudioSource>().Play();
            Destroy(other.gameObject);
        }
    }

    private void Start()
    {
        Score = 40;
        Change(1);
    }

    private void Change(int toLook)
    {
        PlayerModel.gameObject.SetActive(false);
        PlayerModel = looks[toLook].gameObject;
        PlayerModel.gameObject.SetActive(true);
    }

    void Update()
    {
        Slider.transform.localScale = new Vector3(Mathf.Clamp(Score / 120f * 2,0,2), 1, 1);
        if (Score <= 0)
        {
            Change(0);
            Debug.Log("Game over");
            Title.GetComponent<TMP_Text>().text = "Hobo";
        }
        else
        {
            if (Score != prevScore)
            {
                Change(1);
                Title.GetComponent<TMP_Text>().text = "Poor";
                if (Score >= 60)
                {
                    Change(2);
                    Title.GetComponent<TMP_Text>().text = "Wealthy";
                    if (Score >= 80)
                    {
                        Change(3);
                        Title.GetComponent<TMP_Text>().text = "Business";
                        if (Score >= 100)
                        {
                            Change(4);
                            Title.GetComponent<TMP_Text>().text = "Rich";
                            if (Score >= 120)
                            {
                                Change(5);
                                Title.GetComponent<TMP_Text>().text = "Millionaire";
                            }
                        }
                    }
                }
            }
        }
    }
}
