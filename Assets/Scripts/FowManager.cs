using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FowManager : MonoBehaviour
{

   public int playersInside = 0;
   public bool fogOfWarReveal = false;

   private void Start()
   {
      //add as Trigger
      var boxCollider = gameObject.AddComponent<BoxCollider>();
      boxCollider.isTrigger = true;
   }

   private void OnTriggerEnter(Collider other)
   {
      playersInside++;
   }

   private void OnTriggerExit(Collider other)
   {
      playersInside--;
   }



   private void Update()
   {
      if (playersInside >= 1)
      {
         // Reveal room
         fogOfWarReveal = true;
         return;
      }
      
      //hide room

      fogOfWarReveal = false;

   }
   
}
