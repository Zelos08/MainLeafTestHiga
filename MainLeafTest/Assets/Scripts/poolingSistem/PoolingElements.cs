using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingElements : MonoBehaviour {

    public string type; 
	private bool inGame;

    void Start(){
		putInGame ();
	}

	public void putInGame()
	{
		if (!inGame) {
			inGame = true;
			gameObject.SetActive (true);

		}
	}

	public void removeFromGame()
	{
		
		if (inGame) {

			inGame = false;
			gameObject.SetActive (false);
		}
	}

	public bool getIngame(){
		return inGame;
	}
}
