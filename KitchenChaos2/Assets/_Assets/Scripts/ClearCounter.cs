using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private Transform tomatoPrefab;
    [SerializeField] private Transform counterTopPoint;
    
    public void Interact()
    {
        Debug.Log("Interact method called on ClearCounter.");
    
        // Ensure tomatoPrefab and counterTopPoint are assigned correctly
        if (tomatoPrefab == null)
        {
            Debug.LogWarning("TomatoPrefab is not assigned!");
        }
        if (counterTopPoint == null)
        {
            Debug.LogWarning("CounterTopPoint is not assigned!");
        }

        if (tomatoPrefab != null && counterTopPoint != null)
        {
            Transform tomatoTransform = Instantiate(tomatoPrefab, counterTopPoint);
            tomatoTransform.localPosition = Vector3.zero;
            Debug.Log("Tomato instantiated!");
        }
    }

}
