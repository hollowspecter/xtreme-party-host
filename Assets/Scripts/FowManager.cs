using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class FowManager : MonoBehaviour
{
   //Animation duration
   public float revealTime = 0.2f;
   
   //Wall for movement and fog for alpha blend
   public GameObject Wall;
   public GameObject Fog;
   
   
   private int _playersInside = 0;
   private Material fogMaterial;
   private Vector3 _initPos;

   private void Start()
   {
      //add as Trigger
      var boxCollider = gameObject.AddComponent<BoxCollider>();
      boxCollider.isTrigger = true;

      fogMaterial = Fog.GetComponent<Renderer>().material;
      
      _initPos = Wall.transform.localPosition;


   }
   
   //Player entered room
   private void OnTriggerEnter(Collider other)
   {
      if(!other.CompareTag("Player")) return;
      _playersInside++;
   }

   //Player left room
   private void OnTriggerExit(Collider other)
   {
      if(!other.CompareTag("Player")) return;
      _playersInside--;
      
   }


   private void Update()
   {
      if (_playersInside >= 1)
      {
         // Reveal room
         Wall.transform.DOMoveY(-2, revealTime);
         fogMaterial.DOFade(0, revealTime);
         return;
      }
      
      //hide room
      Wall.transform.DOMove(_initPos, revealTime);
      fogMaterial.DOFade(1, revealTime);

   }
   
}
