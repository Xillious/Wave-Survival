using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    PlayerController thePlayer;

    private void Awake()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack()
    {

    }
}
