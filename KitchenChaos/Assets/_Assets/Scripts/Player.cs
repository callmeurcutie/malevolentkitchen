
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player:MonoBehaviour 
{
	
	[SerializeField] private float moveSpeed = 8f;
	[SerializeField] private GameInput gameInput; 
	float rotateSpeed = 10f;
	private bool isWalking;
	

     private void Update() {
		Vector2 inputVector = gameInput.GetMovementVectorNormalized();

		Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

		float playerSize = .7f;
		bool canMove = !Physics.Raycast(transform.position, moveDir, playerSize);
	
		if(canMove){

		transform.position += moveDir * moveSpeed * Time.deltaTime;
		}		

		isWalking = moveDir != Vector3.zero;
		transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
}
		public bool IsWalking(){
			return isWalking;
		}
}