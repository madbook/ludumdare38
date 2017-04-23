using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController {
	public static void UpdateMusics(AudioSource[] musics, ResourceCalculator.Income income, ResourceCalculator.Income stockpile) {
		if(income.food > 0) {
			musics[0].volume = .35f;
		} else {
			musics[0].volume = .05f;
		}

		if(income.energy > 0) {
			musics[1].volume = .2f;
		} else {
			musics[1].volume = .0f;
		}
	}
/* 
	public static void FadeMusics(AudioSource[] musics) {
		Debug.Log(musics);
		int i=0;
		foreach(AudioSource music in musics) {
			music.volume = music.volume * .9f + musicTargetVolumes[i] * .1f;
			i++;
		}
	}*/
}
