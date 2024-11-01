using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animalese. See README.md for more information.
/// </summary>
public class Animalese : MonoBehaviour {

	public AudioClip A;
	public AudioClip B;
	public AudioClip C;
	public AudioClip D;
	public AudioClip E;
	public AudioClip F;
	public AudioClip G;
	public AudioClip H;
	public AudioClip I;
	public AudioClip J;
	public AudioClip K;
	public AudioClip L;
	public AudioClip M;
	public AudioClip N;
	public AudioClip O;
	public AudioClip P;
	public AudioClip Q;
	public AudioClip R;
	public AudioClip S;
	public AudioClip T;
	public AudioClip U;
	public AudioClip V;
	public AudioClip W;
	public AudioClip X;
	public AudioClip Y;
	public AudioClip Z;

	public AudioClip Zero;
	public AudioClip One;
	public AudioClip Two;
	public AudioClip Three;
	public AudioClip Four;
	public AudioClip Five;
	public AudioClip Six;
	public AudioClip Seven;
	public AudioClip Eight;
	public AudioClip Nine;

	public AudioClip Misc;

	public AudioSource[] channels;

	public float pitch = 1.0f;
	public float pitchVariance = 1.1f;
	public float multiplierExcite = 1.1f;
	public float letterDelayInSeconds = 0.1f;
	public float letterDelayVarianceInSeconds = 0.03f;

	private bool isExcited = false;

	private Dictionary<String, AudioClip> alphabet = new Dictionary<String, AudioClip> ();
	private int channelCurrentIndex = 0;
	private int channelCount;
	private float pitchCurrent;

	void Start () {
		channelCount = channels.Length;

		if (channelCount == 0) {
			Debug.LogError ("At least one channel to output sounds must be set.");
		}

		alphabet ["0"] = Zero;
		alphabet ["1"] = One;
		alphabet ["2"] = Two;
		alphabet ["3"] = Three;
		alphabet ["4"] = Four;
		alphabet ["5"] = Five;
		alphabet ["6"] = Six;
		alphabet ["7"] = Seven;
		alphabet ["8"] = Eight;
		alphabet ["9"] = Nine;
		alphabet ["A"] = A;
		alphabet ["B"] = B;
		alphabet ["C"] = C;
		alphabet ["D"] = D;
		alphabet ["E"] = E;
		alphabet ["F"] = F;
		alphabet ["G"] = G;
		alphabet ["H"] = H;
		alphabet ["I"] = I;
		alphabet ["J"] = J;
		alphabet ["K"] = K;
		alphabet ["L"] = L;
		alphabet ["M"] = M;
		alphabet ["N"] = N;
		alphabet ["O"] = O;
		alphabet ["P"] = P;
		alphabet ["Q"] = Q;
		alphabet ["R"] = R;
		alphabet ["S"] = S;
		alphabet ["T"] = T;
		alphabet ["U"] = U;
		alphabet ["V"] = V;
		alphabet ["W"] = W;
		alphabet ["X"] = X;
		alphabet ["Y"] = Y;
		alphabet ["Z"] = Z;
		alphabet ["Misc"] = Misc;

		pitchCurrent = pitch;
	}

	public void Speak (string text) {
		StartCoroutine (Say (text));
	}

	public void StopSpeaking () {
		StopAllCoroutines ();
	}
	
	private IEnumerator Say (string text) {
		foreach (char c in text) {

			string character = c.ToString ().ToUpper ();
			AudioClip clip = null;
			float delayTillNextCharInSeconds = letterDelayInSeconds;

			if (!isExcited) {
				pitchCurrent = UnityEngine.Random.Range (pitch, pitch * pitchVariance);
			}

			if (character == " " || character == "/" || character == "\\") {
				Debug.Log (" ");
			} else if (character == ",") {
				delayTillNextCharInSeconds *= 1.5f;
				Debug.Log (character);
			} else if (character == "." || character == "?" || character == "!") {
				delayTillNextCharInSeconds *= 2.0f;
				Debug.Log (character);
			} else if (character == "*" && !isExcited) {
				pitchCurrent = pitch * multiplierExcite;
				isExcited = true;
				Debug.Log ("<Excited>");
				continue;
			} else if (character == "*" && isExcited) {
				pitchCurrent = pitch;
				isExcited = false;
				Debug.Log ("</Excited>");
				continue;
			} else if (alphabet.ContainsKey (character)) {
				clip = alphabet [character];
				Debug.Log (character);
			} else {
				clip = alphabet ["Misc"];
			}

			if (clip != null) {
				channels [channelCurrentIndex].clip = clip;
				channels [channelCurrentIndex].pitch = pitchCurrent;
				channels [channelCurrentIndex].Play ();
			
				if (++channelCurrentIndex == channelCount) {
					channelCurrentIndex = 0;
				}
			}

			yield return new WaitForSeconds (UnityEngine.Random.Range (letterDelayInSeconds - letterDelayVarianceInSeconds, letterDelayInSeconds + letterDelayVarianceInSeconds));
		}
		pitchCurrent = pitch;
	}
}