using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasHealth
{
    public Health Health { get; set; }
    public Collider CriticalHitBox { get; }
    public Collider RegularHitBox { get; }
    
    public void OnHit(float damage)
    {
        var copy = Health;
        copy.current -= damage;
        Health = copy;
    }
    
    public void OnCriticalHit(float damage)
    {
        var copy = Health;
        copy.current -= damage * 2;
        Health = copy;
    }
}