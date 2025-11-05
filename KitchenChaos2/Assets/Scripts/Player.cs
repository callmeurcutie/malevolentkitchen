using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{

 
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    public static Player Instance { get; private set; }
    
    float rotateSpeed = 20f;
    float playerRadius = .7f;
    float playerHeight = 2f;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;
  
    

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
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
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
          canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

          if(canMove){
              // Can move only on the X 
              moveDir = moveDirX; 
          }else {
              //Cannot move only on the X 

              //Attemt only Z movement
              Vector3 moveDirZ
                  = new Vector3(0, 0, moveDir.z).normalized;
              canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

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
          if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
          {
              if (clearCounter != selectedCounter)
              {
                  SetSelectedCounter(clearCounter);
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

  private void SetSelectedCounter(ClearCounter selectedCounter)
  {
      
      this.selectedCounter = selectedCounter;
      
      OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
      {
          selectedCounter = selectedCounter
      });
  }
  
  

  
}
