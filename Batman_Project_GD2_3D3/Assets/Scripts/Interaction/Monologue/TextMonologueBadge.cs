using System.Collections;
using TMPro;
using UnityEngine;

public class TextMonologueBadge : MonoBehaviour
{
    public GameObject BackgroundMonologue;
    public TextMeshProUGUI monologueTextSansBadge;

    public string monologueString;
    public Badge badge;

    private string monologueTextAvecBadge = "Héhé... Facile !";

    void Start()
    {
        if (monologueTextSansBadge != null)
        {
            monologueTextSansBadge.ForceMeshUpdate(true); 
            monologueTextSansBadge.enabled = false;
            BackgroundMonologue.SetActive(false);
        }
    }

    IEnumerator RemoveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        monologueTextSansBadge.enabled = false;
        BackgroundMonologue.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!badge.badgeTaken)
            {
                BackgroundMonologue.SetActive(true);
                monologueTextSansBadge.text = monologueString;
                monologueTextSansBadge.enabled = true;
                StartCoroutine(RemoveTextAfterDelay(4.0f));
            }
            else if (badge.badgeTaken)
            {
                BackgroundMonologue.SetActive(true);
                monologueTextSansBadge.text = monologueTextAvecBadge;
                monologueTextSansBadge.enabled = true;
                StartCoroutine(RemoveTextAfterDelay(2.0f));
                Destroy(gameObject.GetComponent<Collider>());
            }
        }
    }
}
