using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType { PICKUP, DROP, THROW, KEY, NONE }

public interface Interactable
{
    InteractType Interact();

    InteractType Use();
}
