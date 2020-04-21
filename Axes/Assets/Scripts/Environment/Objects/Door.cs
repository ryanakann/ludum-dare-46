using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    Collider2D coll;
    bool open;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        foreach (Collider2D c in GetComponents<Collider2D>())
            if (!c.isTrigger)
            {
                coll = c;
                break;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Open()
    {
        if (open) return false;
        coll.enabled = false;
        open = true;
        anim.SetBool("open", true);
        return true;
    }

    public bool Close()
    {
        if (!open) return false;
        coll.enabled = true;
        anim.SetBool("open", false);
        return true;
    }

    public InteractType Interact()
    {
        return InteractType.NONE;
    }

    public InteractType Use()
    {
        return InteractType.NONE;
    }
}
