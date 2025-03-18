using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
   private void Start()
   {
      Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
   }

   private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
   {
      
   }
}
