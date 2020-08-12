using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    [SerializeField]
    private Transform interactiblePoint;
    private Transform interactableParent;
    
    [HideInInspector] public Transform interactable;
    List<Transform> potentialInteractables = new List<Transform>();

    Vector2 throwForce = new Vector2(10f, 5f);

    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (interactable)
            {
                Drop();   
            }
            else if (potentialInteractables.Count > 0)
            {
                switch (potentialInteractables[0].GetComponent<Interactable>().Interact())
                {
                    case InteractType.PICKUP:
                        PickUp(potentialInteractables[0]);
                        break;
                    case InteractType.NONE:
                        break;
                }
            }
        }
        if (Input.GetButtonDown("Use") && interactable)
        {
            switch (interactable.GetComponent<Interactable>().Use())
            {
                case InteractType.DROP:
                    Drop();
                    break;
                case InteractType.THROW:
                    Throw();
                    break;
                case InteractType.KEY:
                    Key();
                    break;
                case InteractType.NONE:
                    break;
            }
        }
    }

    void PickUp(Transform interactable_object)
    {
        if (interactable_object.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D r = interactable_object.GetComponent<Rigidbody2D>();
            r.velocity = Vector2.zero;
            r.angularVelocity = 0;
            r.bodyType = RigidbodyType2D.Kinematic;
        }
        foreach (Collider2D coll in interactable_object.GetComponents<Collider2D>())
            coll.enabled = false;
        // lerp this object to the interact point, set interactable
        interactable_object.position = interactiblePoint.position;
        interactableParent = interactable_object.parent;
        interactable_object.parent = interactiblePoint;
        interactable = interactable_object;

    }

    void Throw()
    {
        Transform throwable = interactable;
        Drop();
        throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForce.x * transform.localScale.x, throwForce.y), ForceMode2D.Impulse);
    }

    void Drop()
    {
        if (interactable)
        {
            interactable.parent = interactableParent;
            foreach (Collider2D coll in interactable.GetComponents<Collider2D>())
            {
                coll.enabled = true;
                if (!coll.isTrigger)
                {
                    float distance = GetComponent<Collider2D>().Distance(coll).distance;
                    if (distance < 0)
                    {
                        interactable.position = new Vector2(interactable.transform.position.x +
                            (distance * -2f * Mathf.Sign(transform.localScale.x)),
                            interactable.transform.position.y);
                        print(GetComponent<Collider2D>().Distance(coll).distance);
                    }
                }
            }
            if (interactable.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D r = interactable.GetComponent<Rigidbody2D>();
                r.bodyType = RigidbodyType2D.Dynamic;
                Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
                r.velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, 0, 10f));
            }
        }
        interactable = null;
    }

    void Key()
    {
        Transform key = interactable;
        if (interactable.GetComponent<Key>())
        {
            foreach (Transform t in potentialInteractables)
            {
                // if (t.GetComponent<Door>() && t.GetComponent<Door>().UseKey())
                // {
                //     Drop();
                //     Destroy(key.gameObject);
                // }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!potentialInteractables.Contains(collision.transform) && collision.GetComponent<Interactable>() != null)
        {
            potentialInteractables.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (potentialInteractables.Contains(collision.transform))
        {
            potentialInteractables.Remove(collision.transform);
        }
    }
}
