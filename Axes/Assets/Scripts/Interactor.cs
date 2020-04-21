using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{

    Transform interactable_point, interactable_parent;
    [HideInInspector] public Transform interactable;
    List<Transform> potential_interactables = new List<Transform>();

    Vector2 throw_force = new Vector2(10f, 5f);

    // Start is called before the first frame update
    void Start()
    {
        interactable_point = transform.FindDeepChild("InteractablePoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (interactable)
            {
                Drop();   
            }
            else if (potential_interactables.Count > 0)
            {
                switch (potential_interactables[0].GetComponent<Interactable>().Interact())
                {
                    case InteractType.PICKUP:
                        PickUp(potential_interactables[0]);
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
        interactable_object.position = interactable_point.position;
        interactable_parent = interactable_object.parent;
        interactable_object.parent = interactable_point;
        interactable = interactable_object;

    }

    void Throw()
    {
        Transform throwable = interactable;
        Drop();
        throwable.GetComponent<Rigidbody2D>().AddForce(new Vector2(throw_force.x * transform.localScale.x, throw_force.y), ForceMode2D.Impulse);
    }

    void Drop()
    {
        if (interactable)
        {
            interactable.parent = interactable_parent;
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
            foreach (Transform t in potential_interactables)
            {
                if (t.GetComponent<Door>() && t.GetComponent<Door>().Open())
                {
                    Drop();
                    Destroy(key.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!potential_interactables.Contains(collision.transform) && collision.GetComponent<Interactable>() != null)
        {
            potential_interactables.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (potential_interactables.Contains(collision.transform))
        {
            potential_interactables.Remove(collision.transform);
        }
    }
}
