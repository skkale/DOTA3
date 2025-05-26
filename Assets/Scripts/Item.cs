using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public GameObject itemGameObject;
    public virtual void StartFireEffect() { }
    public virtual void StopFireEffect() { }
    public abstract void Use();
}
