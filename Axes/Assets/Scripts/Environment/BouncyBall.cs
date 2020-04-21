using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : MonoBehaviour, Interactable
{

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InteractType Interact()
    {
        return InteractType.PICKUP;
    }

    public InteractType Use()
    {
        return InteractType.THROW;
    }
}
