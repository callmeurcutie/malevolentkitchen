using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    
    float rotateSpeed = 10f;
    float playerSize = .7f; 
   
  private void Update()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
    bool canMove = !Physics.Raycast(transform.position, moveDir, playerSize);

    if (canMove)
    {
      transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    
    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
  } 
  
}
