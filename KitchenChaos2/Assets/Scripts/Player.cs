using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{

 
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    private KitchenObject kitchenObject;
    
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public static Player Instance { get; private set; }
    
    float rotateSpeed = 20f;
    float playerRadius = .7f;
    float playerHeight = 2f;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
  
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one instance of a player");
        }
        
        Instance = this;
    }

    private void Update()
  {
      HandleMovement();
      HandleInteractions();
  }


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }


    private void HandleMovement()
  {
    
      float moveDistance = moveSpeed * Time.deltaTime;
    
      Vector2 inputVector = gameInput.GetMovementVectorNormalized(); 
      Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
      bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,  playerRadius, moveDir, moveDistance );

      if(!canMove){
          //Cannot move towards moveDir

          //Attemt only X movement 
          Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
          canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

          if(canMove){
              // Can move only on the X 
              moveDir = moveDirX; 
          }else {
              //Cannot move only on the X 

              //Attemt only Z movement
              Vector3 moveDirZ
                  = new Vector3(0, 0, moveDir.z).normalized;
              canMove = moveDir.z !=0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

              if(canMove){
                  //Can move only on the Z
                  moveDir = moveDirZ;
              } else {
                  //Cannot move in any direction
              }	
          }
      }
      if (canMove)
      {
          transform.position += moveDir * moveDistance;
      }
    
      transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
  }

  private void HandleInteractions()
  {
      
      Vector2 inputVector = gameInput.GetMovementVectorNormalized();
      
      Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

      if (moveDir != Vector3.zero)
      {
          lastInteractDir = moveDir;
      }

      float interactDistance = 2f;
      if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactDistance, countersLayerMask))
      {
          if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
          {
              if (baseCounter != selectedCounter)
              {
                  SetSelectedCounter(baseCounter);
              }
          }
          else
          {
              SetSelectedCounter(null);
          }
      }
      else
      {
          SetSelectedCounter(null);
      }
  }

  private void SetSelectedCounter(BaseCounter selectedCounter)
  {
      
      this.selectedCounter = selectedCounter;
      
      OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
      {
          selectedCounter = selectedCounter
      });
  }

  public Transform GetKitchenObjectFollowTransform()
  {
      return kitchenObjectHoldPoint;
  }

  public void SetKitchenObject(KitchenObject kitchenObject)
  {
      this.kitchenObject = kitchenObject;
  }

  public KitchenObject GetKitchenObject()
  {
      return kitchenObject;
  }

  public void ClearKitchenObject()
  {
      kitchenObject = null;
  }

  public bool HasKitchenObject()
  {
      return kitchenObject != null;
  }
}
