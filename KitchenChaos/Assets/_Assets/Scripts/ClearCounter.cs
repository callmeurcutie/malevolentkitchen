using KitchenObject;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private KitchenObjectSO kitchenObjectSo;
    
    public void Interact()
    {
        Debug.Log("Interact");
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSo.prefab, counterTopPoint) as Transform;
        kitchenObjectTransform.localPosition= Vector3.zero;

    }
    
}
