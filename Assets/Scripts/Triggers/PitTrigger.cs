﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PitTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other)
	{
        if (other.gameObject.GetComponent<Movement>().shaking)
        {
            return;
        }
        //We can eventually replace this with the level name when we make actual scenes
        GameManager.Instance.PlayerDied(other.gameObject);
	}
}
