using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour {

    private AudioSource _audioSource;
	// Use this for initialization
	void Start () {

        _audioSource = GetComponent<AudioSource>();
        //play explosion Sound FX
        _audioSource.Play();

        //Destroy the exploded object
        Destroy(this.gameObject, 4.0f);
	}
	
}
