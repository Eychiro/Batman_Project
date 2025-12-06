using System.Collections;
using UnityEngine;

public class ChangementCouleur : MonoBehaviour
{
    public RandomMovementV2test batmanClassique;
    public BatmanCouloirIA batmanCouloir; 

    public Color safeColor = Color.white;
    public Color dangerColor = Color.red;

    public int nombreDeFlashs = 7;
    public float minVitesseFlash = 0.04f;
    public float maxVitesseFlash = 0.07f;
    public float dureeCoupure = 0.1f;

    private Light _flashlight;
    private bool _isDangerMode = false;
    private Coroutine changementCouleur;

    private bool flashlightOn = false;

    void Start()
    {
        _flashlight = GetComponent<Light>();

        if (_flashlight != null)
        {
            _flashlight.color = safeColor;
        }
    }

    void Update()
    {
        if (_flashlight == null) 
        return;

        bool dangerCouloir = (batmanCouloir != null && batmanCouloir.IsAgentActive);
        bool dangerClassique = (batmanClassique != null && batmanClassique.IsAgentActive);
        
        bool batmanEstLa = dangerCouloir || dangerClassique;

        if (batmanEstLa != _isDangerMode)
        {
            _isDangerMode = batmanEstLa;

            if (changementCouleur != null)
            {
                StopCoroutine(changementCouleur);
            }

            flashlightOn = _flashlight.enabled;
            changementCouleur = StartCoroutine(SequenceScintillement(_isDangerMode));
        }
    }

    private IEnumerator SequenceScintillement(bool versLeDanger)
    {
        Color couleurFinale = versLeDanger ? dangerColor : safeColor;
        Color couleurOpposee = versLeDanger ? safeColor : dangerColor;

        _flashlight.enabled = false;
        yield return new WaitForSeconds(dureeCoupure*3f);
        _flashlight.enabled = true;

        for (int i = 0; i < nombreDeFlashs; i++)
        {
            float vitesseFlashAleatoire = Random.Range(minVitesseFlash, maxVitesseFlash);

            _flashlight.color = couleurOpposee;
            yield return new WaitForSeconds(vitesseFlashAleatoire);

            if (Random.Range(0, 3) == 0) 
            {
                Color couleurAvantCoupure = _flashlight.color;
                
                _flashlight.enabled = false;
                yield return new WaitForSeconds(dureeCoupure); 
                
                _flashlight.enabled = true;
                _flashlight.color = couleurAvantCoupure;
            }

            if (Random.Range(0, 2) == 0)
            {
                _flashlight.color = couleurFinale;
                yield return new WaitForSeconds(vitesseFlashAleatoire);
            }
        }

        _flashlight.color = couleurFinale;
        changementCouleur = null;
        
        if (!flashlightOn)
            _flashlight.enabled = false;
    }

    public void StopAllFlashEffects()
{
    if (changementCouleur != null)
    {
        StopCoroutine(changementCouleur);
        changementCouleur = null;
    }
    
    if (_flashlight != null)
    {
        _flashlight.color = _isDangerMode ? dangerColor : safeColor;
    }
}
}
