using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameEvent();
public class Lever : MonoBehaviour, Interactable
{
    public GameEvent OnPress;

    bool right;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public InteractType Interact()
    {
        right = !right;
        anim.SetBool("right", right);
        OnPress?.Invoke();
        return InteractType.NONE;
    }

    public InteractType Use()
    {
        return InteractType.NONE;
    }
}
