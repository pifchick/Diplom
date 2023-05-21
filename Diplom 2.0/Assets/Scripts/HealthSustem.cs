using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSustem : MonoBehaviour
{
    [SerializeField] private GameObject[] _hearts;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].SetActive(true);
           
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void SetHealth(int amount)
    {
        Debug.Log($"CURRENT HEALT {amount}");
        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].SetActive(i < amount);
        }
    }
}
