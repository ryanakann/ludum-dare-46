using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType { PICKUP, DROP, THROW, NONE }

public interface Interactable
{
    InteractType Interact();

    InteractType Use();
}
