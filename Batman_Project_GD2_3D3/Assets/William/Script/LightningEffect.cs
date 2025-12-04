using System.Collections;
using UnityEngine;

public class LightningEffect : MonoBehaviour
{
    private Light sceneLight; 

    public float maxLightIntensity = 15000f; 
    public float preLightDuration = 0.02f;
    public float peakDuration = 0.05f;
    public float fadeDuration = 0.08f;
    public int minNumSubsequentStrokes = 2;
    public int maxNumSubsequentStrokes = 4;
    public float subsequentStrokeDelay = 0.08f;
    public float subsequentStrokeIntensityFactor = 0.4f;

    public Vector3[] strikeLocations = new Vector3[3];
    public float minDélai = 30f;
    public float maxDélai = 45f;

    private Coroutine autoTriggerCoroutine;

    void Awake()
    {
        sceneLight = GetComponent<Light>();
        if (sceneLight != null)
        {
            sceneLight.intensity = 0;
            sceneLight.enabled = false;
        }
    }

    void Start()
    {
        StartAutoTrigger();
    }
    
    public bool IsAutoTriggerRunning
    {
        get { return autoTriggerCoroutine != null; }
    }

    public void StopAutoTrigger()
    {
        if (autoTriggerCoroutine != null)
        {
            StopCoroutine(autoTriggerCoroutine);
            autoTriggerCoroutine = null;
        }
        
        if (sceneLight != null)
        {
            sceneLight.intensity = 0;
            sceneLight.enabled = false;
        }
    }

    public void StartAutoTrigger()
    {
        if (autoTriggerCoroutine == null)
        {
            autoTriggerCoroutine = StartCoroutine(AutoTriggerLoop());
        }
    }
    
    IEnumerator AutoTriggerLoop()
    {
        while (true)
        {
            float randomDelay = Random.Range(minDélai, maxDélai);
            
            yield return new WaitForSeconds(randomDelay);

            TeleportToRandomPosition();

            yield return StartCoroutine(DoLightningEffect());
        }
    }


    public void TeleportToRandomPosition()
    {
        if (strikeLocations.Length == 0) return;

        int randomIndex = Random.Range(0, strikeLocations.Length);
        
        transform.position = strikeLocations[randomIndex];
    }

    public IEnumerator DoLightningEffect()
    {
        if (sceneLight == null) yield break;

        sceneLight.enabled = true;

        sceneLight.intensity = maxLightIntensity * 0.01f;
        yield return new WaitForSeconds(preLightDuration);

        float timer = 0f;
        while (timer < peakDuration)
        {
            float intensity = Mathf.Lerp(maxLightIntensity * 0.01f, maxLightIntensity, timer / peakDuration);
            sceneLight.intensity = intensity;
            timer += Time.deltaTime;
            yield return null;
        }

        sceneLight.intensity = maxLightIntensity;
        yield return new WaitForSeconds(0.005f);

        timer = 0f;
        while (timer < fadeDuration)
        {
            float intensity = Mathf.Lerp(maxLightIntensity, 0, timer / fadeDuration);
            sceneLight.intensity = intensity;
            timer += Time.deltaTime;
            yield return null;
        }

        sceneLight.intensity = 0;

        int randomEclairAddi = Random.Range(minNumSubsequentStrokes, maxNumSubsequentStrokes+1);
        
        for (int i = 0; i < randomEclairAddi; i++)
        {
            yield return new WaitForSeconds(subsequentStrokeDelay);

            float subIntensity = maxLightIntensity * subsequentStrokeIntensityFactor;
            float subDuration = peakDuration * 0.5f; 

            timer = 0f;
            while (timer < subDuration)
            {
                sceneLight.intensity = Mathf.Lerp(0, subIntensity, timer / subDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            timer = 0f;
            while (timer < subDuration)
            {
                sceneLight.intensity = Mathf.Lerp(subIntensity, 0, timer / subDuration);
                timer += Time.deltaTime;
                yield return null;
            }
            sceneLight.intensity = 0; 
        }

        sceneLight.enabled = false;
    }

    public IEnumerator TriggerAndResumeSequence()
    {
        if (IsAutoTriggerRunning)
        {
            StopAutoTrigger();
        }
        
        TeleportToRandomPosition();
        yield return StartCoroutine(DoLightningEffect());
        
        StartAutoTrigger();
    }
}