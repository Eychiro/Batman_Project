using System.Collections;
using TMPro;
using UnityEngine;

public class PorteBadge : MonoBehaviour
{
    public TextMeshProUGUI emptyPorteBadgeText;
    public Badge badge;
    private bool playerInRange = false;
    private bool _isClosed = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _isClosed)
        {
            playerInRange = true;
            
            if (emptyPorteBadgeText != null)
            {
                emptyPorteBadgeText.enabled = true;

                if (badge.badgeTaken)
                {
                    emptyPorteBadgeText.text = "Appuyez sur E pour ouvrir la porte";
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _isClosed)
        {
            playerInRange = false;
            
            if (emptyPorteBadgeText != null)
            {
                emptyPorteBadgeText.enabled = false;
            }
        }
    }

    IEnumerator MovePorte()
    {
        float time = 0;

        while(time <= 1)
        {
            time += Time.deltaTime;
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, Quaternion.Euler(0, 90, 0), time);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {
        if (playerInRange && _isClosed && badge.badgeTaken && Input.GetKeyDown(KeyCode.E))
        {
            _isClosed = false;
            emptyPorteBadgeText.enabled = false;

            StartCoroutine(MovePorte());
        }
    }
}
