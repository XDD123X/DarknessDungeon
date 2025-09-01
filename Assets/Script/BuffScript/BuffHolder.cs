using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHolder : MonoBehaviour
{
    public List<Buff> buffs = new List<Buff>();
    private Dictionary<Buff, Coroutine> activeCoroutines = new Dictionary<Buff, Coroutine>();
    private Dictionary<Buff, float> buffStartTimes = new Dictionary<Buff, float>();

    public void ResetCD(Buff buff)
    {
        if (buffs.Contains(buff))
        {
            if (activeCoroutines.TryGetValue(buff, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                activeCoroutines[buff] = StartCoroutine(BuffDuration(buff));
                buffStartTimes[buff] = Time.time; 
            }
        }
    }

    public void AllCalculate()
    {
        foreach (Buff b in buffs)
        {
            b.calculateAgain();
        }
    }

    public void InsertNewBuff(Buff buff)
    {
        buffs.Add(buff);

        Coroutine coroutine = StartCoroutine(BuffDuration(buff));
        activeCoroutines[buff] = coroutine;
        buffStartTimes[buff] = Time.time;  
    }

    public void InsertNewBuff(Buff buff, int index)
    {
        buffs.Insert(index, buff);

        Coroutine coroutine = StartCoroutine(BuffDuration(buff));
        activeCoroutines[buff] = coroutine;
        buffStartTimes[buff] = Time.time;  
    }

    private IEnumerator BuffDuration(Buff buff)
    {
        float buffDuration = buff.Duration + (buff.DurationEL * (buff.GetLevel() - 1));

        yield return new WaitForSeconds(buffDuration);

        buffs.Remove(buff);
        buff.EndEffect();
        activeCoroutines.Remove(buff);
        buffStartTimes.Remove(buff);
    }

    public void RemoveBuff(Buff buff)
    {
        if (buffs.Contains(buff))
        {
            if (activeCoroutines.TryGetValue(buff, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                activeCoroutines.Remove(buff);
            }
            buffs.Remove(buff);
            buff.EndEffect();
            buffStartTimes.Remove(buff);
        }
    }

    public float GetRemainingDuration(Buff buff)
    {
        if (buffStartTimes.TryGetValue(buff, out float startTime))
        {
            float elapsed = Time.time - startTime;
            float totalDuration = buff.Duration + (buff.DurationEL * (buff.GetLevel() - 1));
            return Mathf.Max(0, totalDuration - elapsed);
        }
        return 0;
    }
}
