using UnityEngine;

public interface IKitchenObjectParent {



    public Transform GetKitchenObjectFollowTransform();
    

    public KitchenObject GetKitchenObject();


    public void ClearKitchenObject();


    public bool HasKitchenObject();

    public void SetKitchenObject(KitchenObject kitchenObject);





}
