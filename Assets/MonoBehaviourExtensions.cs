using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MonoBehaviourExtensions
{
    // Start is called before the first frame update
    public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay)
    {
        mono.StartCoroutine(CallWithDelayRoutine(method,delay));
    }

    // Update is called once per frame
    static IEnumerator CallWithDelayRoutine(Action method, float delay)
    {
        yield return new WaitForSeconds(delay);
        method();
    }
}
