using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        return InteractType.KEY;
    }
}
