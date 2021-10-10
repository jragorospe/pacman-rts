using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishHandler : MonoBehaviour
{
    public GameManager gameManager;
    public ParticleSystem confetti;

    void OnTriggerEnter(Collider _col)
    {
        //Destroys Unit, updates score and plays particle effect.
        if(_col.tag == "Unit")
        {
            Destroy(_col.gameObject);
            gameManager.AddScore();
            confetti.Play();
        }
    }
}
