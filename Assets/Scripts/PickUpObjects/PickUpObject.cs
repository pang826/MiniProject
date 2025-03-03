using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PickUpObject : MonoBehaviour
{
    protected void HideObject(PickUpObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected void ShowObject(PickUpObject obj)
    {
        obj.gameObject.SetActive(true);
    }
}
