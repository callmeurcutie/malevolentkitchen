using System;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public void Interact()
    {

        Debug.Log("Interact");
    }
}
