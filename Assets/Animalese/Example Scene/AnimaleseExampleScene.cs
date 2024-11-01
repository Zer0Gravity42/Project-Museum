using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animalese example scene controller.
/// Simply switches between the characters and gives them text to read.
/// </summary>
public class AnimaleseExampleScene : MonoBehaviour {

	public GameObject Zip;
	public GameObject Zap;
	public GameObject Zoup;

	private Animalese zip;
	private Animalese zap;
	private Animalese zoup;

	private int currentSpeaker = 0;

	void Start () {
		zip = Zip.GetComponent<Animalese> ();
		zap = Zap.GetComponent<Animalese> ();
		zoup = Zoup.GetComponent<Animalese> ();
		InvokeRepeating ("ChangeWhoIsSpeaking", 0.0f, 3.0f);
	}

	private void ChangeWhoIsSpeaking () {

		switch (currentSpeaker) {
		case 0:
			zip.Speak ("Hello, world.");
			break;
		case 1:
			zap.Speak ("Mary had a little lamb.");
			break;
		case 2:
			zoup.Speak ("Why *walk* when you can *ride?*");
			break;
		}
		if (++currentSpeaker == 3) {
			currentSpeaker = 0;
		}
	}
}