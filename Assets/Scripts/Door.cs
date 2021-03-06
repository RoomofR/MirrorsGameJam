﻿using UnityEngine;

public class Door : MonoBehaviour {

	public Colors color;
	public bool isOpen;
	//public Transform doorObject; //Actualy thing to be enabled and disabled

    private ParticleSystem doorParticleWhite;
    private ParticleSystem doorParticleColoured;

    void Start() {
        doorParticleWhite = transform.GetChild(1).GetComponent<ParticleSystem>();
        doorParticleColoured = transform.GetChild(2).GetComponent<ParticleSystem>();
        SetParticleColour();
    }


    #region TEMPORARY CODE

    void Update() {
        ToggleParticle();
        SetParticleColour();
    }
    #endregion



    public void trigger(){
		if(isOpen){
			//doorObject.gameObject.SetActive(true);
			isOpen=false;
		}else{
			//doorObject.gameObject.SetActive(false);
			isOpen=true;	
		}
        ToggleParticle();
    }

    private void ToggleParticle() {
        var mainW = doorParticleWhite.main;
        var mainC = doorParticleColoured.main;
        if (!isOpen) {
            mainW.loop = true;
            mainC.loop = true;
            doorParticleWhite.Play();
            doorParticleColoured.Play();
        }
        else {
            mainW.loop = false;
            mainC.loop = false;
        }
    }

    void SetParticleColour() {
        var main = doorParticleColoured.main;
        Color newParticleColour = Color.white;

        switch (color) {
            case Colors.Blue:
                newParticleColour = Color.blue;
                break;
            case Colors.Green:
                newParticleColour = Color.green;
                break;
            case Colors.Orange:
                newParticleColour = Color.magenta; //<===== "make me orange!"
                break;
            case Colors.Red:
                newParticleColour = Color.red;
                break;
            case Colors.Yellow:
                newParticleColour = Color.yellow;
                break;
        }
        main.startColor = newParticleColour;
    }
}
