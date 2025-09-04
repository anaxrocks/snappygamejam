using UnityEngine;
using System.Collections;

public class Hints : MonoBehaviour
{
    [Header("Hint GameObjects")]
    public GameObject _RESTART;
    public GameObject _E;
    public GameObject _Q;
    public GameObject _WASD;
    public GameObject _SHIFT;
    public GameObject _SPACE;

    [Header("Timing Settings")]
    public float interactionHintDelay = 5f;
    public float movementHintDelay = 3f;
    public float otherHintsDelay = 5f;

    public static Hints Instance;
    
    // Hint states
    private bool eHintShown = false;
    private bool wasdHintShown = false;
    private bool qHintShown = false;
    private bool shiftHintShown = false;
    private bool spaceHintShown = false;
    private bool restartHintShown = false;
    public bool isStart = true;

    // Coroutines for managing hints
    private Coroutine eHintCoroutine;
    private Coroutine wasdHintCoroutine;
    private Coroutine qHintCoroutine;
    private Coroutine shiftHintCoroutine;
    private Coroutine spaceHintCoroutine;
    private Coroutine restartHintCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // Hide all hints initially
        HideAllHints();
        
        // Start the E hint timer immediately on level 1
        if (ShouldShowHints()) // You can add level checking logic here
        {
            StartEHint();
        }
    }

    void Update()
    {
        if (!ShouldShowHints()) return;

        // Check for E press
        if (InputManager.interactionPressed && eHintShown)
        {
            HideEHint();
            if (isStart)
            {
                isStart = false;
                StartMovementHint();
            }
        }

        // Check for movement
        if (InputManager.isHoldingMovement && wasdHintShown)
        {
            HideWASDHint();
        }

        // Check for other inputs and hide their respective hints
        if (InputManager.throwPressed && qHintShown)
        {
            HideQHint();
        }

        if (InputManager.consumePressed && shiftHintShown)
        {
            HideShiftHint();
        }

        if (InputManager.attackPressed && spaceHintShown)
        {
            HideSpaceHint();
        }
    }

    #region Hint Management Methods

    public void StartEHint()
    {
        if (eHintCoroutine != null) StopCoroutine(eHintCoroutine);
        eHintCoroutine = StartCoroutine(ShowEHintAfterDelay());
    }

    public void StartMovementHint()
    {
        if (wasdHintCoroutine != null) StopCoroutine(wasdHintCoroutine);
        wasdHintCoroutine = StartCoroutine(ShowWASDHintAfterDelay());
    }

    public void StartQHint()
    {
        if (qHintCoroutine != null) StopCoroutine(qHintCoroutine);
        qHintCoroutine = StartCoroutine(ShowQHintAfterDelay());
    }

    public void StartShiftHint()
    {
        if (shiftHintCoroutine != null) StopCoroutine(shiftHintCoroutine);
        shiftHintCoroutine = StartCoroutine(ShowShiftHintAfterDelay());
    }

    public void StartSpaceHint()
    {
        if (spaceHintCoroutine != null) StopCoroutine(spaceHintCoroutine);
        spaceHintCoroutine = StartCoroutine(ShowSpaceHintAfterDelay());
    }

    public void StartRestartHint()
    {
        if (restartHintCoroutine != null) StopCoroutine(restartHintCoroutine);
        restartHintCoroutine = StartCoroutine(ShowRestartHintAfterDelay());
    }

    #endregion

    #region Coroutines

    private IEnumerator ShowEHintAfterDelay()
    {
        yield return new WaitForSeconds(interactionHintDelay);
        ShowEHint();
    }

    private IEnumerator ShowWASDHintAfterDelay()
    {
        yield return new WaitForSeconds(movementHintDelay);
        ShowWASDHint();
    }

    private IEnumerator ShowQHintAfterDelay()
    {
        yield return new WaitForSeconds(otherHintsDelay);
        ShowQHint();
    }

    private IEnumerator ShowShiftHintAfterDelay()
    {
        yield return new WaitForSeconds(otherHintsDelay);
        ShowShiftHint();
    }

    private IEnumerator ShowSpaceHintAfterDelay()
    {
        yield return new WaitForSeconds(otherHintsDelay);
        ShowSpaceHint();
    }

    private IEnumerator ShowRestartHintAfterDelay()
    {
        yield return new WaitForSeconds(otherHintsDelay);
        ShowRestartHint();
    }

    #endregion

    #region Show/Hide Hint Methods

    private void ShowEHint()
    {
        if (_E != null && !eHintShown)
        {
            _E.SetActive(true);
            eHintShown = true;
        }
    }

    private void HideEHint()
    {
        if (_E != null && eHintShown)
        {
            _E.SetActive(false);
            eHintShown = false;
            if (eHintCoroutine != null)
            {
                StopCoroutine(eHintCoroutine);
                eHintCoroutine = null;
            }
        }
    }

    private void ShowWASDHint()
    {
        if (_WASD != null && !wasdHintShown)
        {
            _WASD.SetActive(true);
            wasdHintShown = true;
        }
    }

    private void HideWASDHint()
    {
        if (_WASD != null && wasdHintShown)
        {
            _WASD.SetActive(false);
            wasdHintShown = false;
            if (wasdHintCoroutine != null)
            {
                StopCoroutine(wasdHintCoroutine);
                wasdHintCoroutine = null;
            }
        }
    }

    private void ShowQHint()
    {
        if (_Q != null && !qHintShown)
        {
            _Q.SetActive(true);
            qHintShown = true;
        }
    }

    private void HideQHint()
    {
        if (_Q != null && qHintShown)
        {
            _Q.SetActive(false);
            qHintShown = false;
            if (qHintCoroutine != null)
            {
                StopCoroutine(qHintCoroutine);
                qHintCoroutine = null;
            }
        }
    }

    private void ShowShiftHint()
    {
        if (_SHIFT != null && !shiftHintShown)
        {
            _SHIFT.SetActive(true);
            shiftHintShown = true;
        }
    }

    private void HideShiftHint()
    {
        if (_SHIFT != null && shiftHintShown)
        {
            _SHIFT.SetActive(false);
            shiftHintShown = false;
            if (shiftHintCoroutine != null)
            {
                StopCoroutine(shiftHintCoroutine);
                shiftHintCoroutine = null;
            }
        }
    }

    private void ShowSpaceHint()
    {
        if (_SPACE != null && !spaceHintShown)
        {
            _SPACE.SetActive(true);
            spaceHintShown = true;
        }
    }

    private void HideSpaceHint()
    {
        if (_SPACE != null && spaceHintShown)
        {
            _SPACE.SetActive(false);
            spaceHintShown = false;
            if (spaceHintCoroutine != null)
            {
                StopCoroutine(spaceHintCoroutine);
                spaceHintCoroutine = null;
            }
        }
    }

    private void ShowRestartHint()
    {
        if (_RESTART != null && !restartHintShown)
        {
            _RESTART.SetActive(true);
            restartHintShown = true;
        }
    }

    private void HideAllHints()
    {
        if (_E != null) _E.SetActive(false);
        if (_WASD != null) _WASD.SetActive(false);
        if (_Q != null) _Q.SetActive(false);
        if (_SHIFT != null) _SHIFT.SetActive(false);
        if (_SPACE != null) _SPACE.SetActive(false);
        //if (_RESTART != null) _RESTART.SetActive(false);
    }

    #endregion

    #region Public Utility Methods

    // Add your level checking logic here
    private bool ShouldShowHints()
    {
        // Example: return SceneManager.GetActiveScene().name == "Level1";
        // For now, return true - you can modify this based on your level system
        return true;
    }

    // Call this method to reset all hints (useful when restarting level)
    public void ResetAllHints()
    {
        // Stop all coroutines
        if (eHintCoroutine != null) StopCoroutine(eHintCoroutine);
        if (wasdHintCoroutine != null) StopCoroutine(wasdHintCoroutine);
        if (qHintCoroutine != null) StopCoroutine(qHintCoroutine);
        if (shiftHintCoroutine != null) StopCoroutine(shiftHintCoroutine);
        if (spaceHintCoroutine != null) StopCoroutine(spaceHintCoroutine);
        if (restartHintCoroutine != null) StopCoroutine(restartHintCoroutine);

        // Reset all states
        eHintShown = false;
        wasdHintShown = false;
        qHintShown = false;
        shiftHintShown = false;
        spaceHintShown = false;
        restartHintShown = false;

        // Hide all hints
        HideAllHints();

        // Restart the hint sequence if needed
        if (ShouldShowHints())
        {
            StartEHint();
        }
    }

    // Call these methods from other scripts when you want to trigger specific hints
    public void TriggerEHint() { ShowEHint(); }
    public void TriggerQHint() { ShowQHint(); }
    public void TriggerShiftHint() { ShowShiftHint(); }
    public void TriggerSpaceHint() { ShowSpaceHint(); }
    public void TriggerRestartHint() { StartRestartHint(); }

    #endregion
}