using UnityEditor;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("MainCamera"))
        {
            EditorApplication.isPlaying = false;
        }
    }
}
