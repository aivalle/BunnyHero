using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public enum EffectT
    {
        None,
        Particles,
        Trail,
    }
    public EffectT EffectType;
    public GameObject EffectGObject;
    public GameObject BunnyGObject;
    public GameObject BoxSpriteGObject;

    public void ActivateEffect(bool opc) {
        if (EffectType == EffectT.Particles)
        {
            ParticleSystem.EmissionModule emBox = EffectGObject.GetComponent<ParticleSystem>().emission;
            emBox.enabled = opc;
        }
        else if (EffectType == EffectT.Trail) {

        }
    }
   
}
