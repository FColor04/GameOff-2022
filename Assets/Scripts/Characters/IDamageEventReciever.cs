using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageEventReciever
{
    public void OnHit(float damage);    
    public void OnCriticalHit(float damage);    
}