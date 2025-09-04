using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour
{
    public bool _hit = false;
    private bool inRange = false;
    public Animator _animator;
    private Coroutine despawnCoroutine = null; // Track the despawn coroutine
    
    IEnumerator MagicTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            SpawnMagic();
        }
    }

    IEnumerator DespawnMagic()
    {
        yield return new WaitForSeconds(3f);
        _hit = false;
        Debug.Log("despawn magic");
        despawnCoroutine = null; // Clear the reference when done
    }

    void Start()
    {
        StartCoroutine(MagicTimer());
    }

    void SpawnMagic()
    {
        _animator.SetTrigger("Magic");
        if (inRange)
        {
            SoundManager.Instance.PlaySound2D("WizardMagic");
            _hit = true;

            // Stop any existing despawn coroutine to prevent overlaps
            if (despawnCoroutine != null)
            {
                StopCoroutine(despawnCoroutine);
            }

            // Start the despawn coroutine
            despawnCoroutine = StartCoroutine(DespawnMagic());
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("inrange");
            inRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}