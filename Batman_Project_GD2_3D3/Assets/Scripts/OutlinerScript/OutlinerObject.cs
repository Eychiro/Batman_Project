using UnityEngine;

public class OutlinerObject : MonoBehaviour
{
    private Outline _outline;
    
    void Start()
    {
        _outline = gameObject.AddComponent<Outline>();
        _outline.enabled = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _outline.enabled = true;
            _outline.OutlineWidth = 5.0f;
            Debug.Log("l'objet devient lumière !");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _outline.enabled = false;
            Debug.Log("L'objet devient ténèbres !");
        }
    }
}
