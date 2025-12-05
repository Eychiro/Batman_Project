using System.Collections;
using TMPro;
using UnityEngine;

public class TextMonologue : MonoBehaviour
{
    public GameObject BackgroundMonologue;
    public TextMeshProUGUI monologueText;
    public string monologueString;

    void Start()
    {
        if (monologueText != null)
        {
            monologueText.ForceMeshUpdate(true); 
            monologueText.enabled = false;
            BackgroundMonologue.SetActive(false);
        }
    }

    IEnumerator RemoveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        monologueText.enabled = false;
        BackgroundMonologue.SetActive(false);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BackgroundMonologue.SetActive(true);
            monologueText.text = monologueString;
            monologueText.enabled = true;
            StartCoroutine(RemoveTextAfterDelay(5.0f));
            Destroy(gameObject.GetComponent<Collider>());
        }
    }
}
