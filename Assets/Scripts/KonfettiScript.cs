using UnityEngine;

public class KonfettiScript : MonoBehaviour
{
    private void Start()
    {
        RubbishSpawner.AddRubbishToObject(gameObject, 3, 7f);
    }
}
