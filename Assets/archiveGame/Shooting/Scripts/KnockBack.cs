using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnockBack : MonoBehaviour
{
    private Rigidbody2D rb;
    

    [SerializeField]
    private float strength = 0.1f, delay = 0.15f;
    public UnityEvent OnBegin, OnDone;

    private AgentMover mover;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        mover = gameObject.GetComponent<AgentMover>();
    }

    public void PlayeFeedback(GameObject sender) 
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        Debug.Log(sender.name+" Dir: " + direction*strength);
        mover.ImpulseVector = direction * strength;
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        mover.ImpulseVector = Vector2.zero;
        OnDone?.Invoke();

    }
}
