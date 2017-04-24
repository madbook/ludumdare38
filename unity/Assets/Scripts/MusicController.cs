using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MusicController {

	private const int chill_synth = 0; //always on
	private const int dirty_bass_synth = 1; // electricity
	private const int solo_synth = 2; // food
	private const int simple_drums = 3; // construction
	private const int bass_kick = 4; // military
	private const int hopeful_synth = 5; // top floor cleared

	private static float[] musicTargetVolumes;
	private static float[] musicCurrentVolumes;

	private const float very_loud = .15f;
	private const float loud = .15f;
	private const float medium = .1f;	
	private const float quiet = .05f;	
	private const float silent = 0;

	private static bool everConstructed = false;

	public static void Start(AudioSource[] musics) {
		int i=0;
		foreach (AudioSource music in musics ) {
			//musicTargetVolumes[i] = silent;
			//musicCurrentVolumes[i] = silent;
			music.volume = silent;
			music.Play();
		}
		Debug.Log ("MUSIC CONTROLLER STARTED ###########");
	}

	public static void UpdateMusics(AudioSource[] musics, ResourceCalculator.Income income, ResourceCalculator.Income stockpile, RoomController[] rooms) {
		if(income.food > 0) {
			musics[solo_synth].volume = quiet;
		} else {
			musics[solo_synth].volume = silent;
		}

		if(income.energy > 0) {
			musics[dirty_bass_synth].volume = loud;
		} else {
			musics[dirty_bass_synth].volume = silent;
		}

		bool constructing = false;
		int highestCleared = 0;
		bool hasHospital = false;

		int i=0;
		foreach(RoomController room in rooms) {
			if (room.assignment.assigned) {
				constructing = true;
			}
			if (room.type == RoomType.Hospital) {
				hasHospital = true;
			}
			if (room.type != RoomType.Rubble) {
				highestCleared = i;
			}
			i++;
		}

		if (constructing) {
			everConstructed = true;
			musics[bass_kick].volume = loud;
			musics[simple_drums].volume = loud;
		} else {
			musics[bass_kick].volume = quiet;
			if(everConstructed) {
				musics[simple_drums].volume = quiet;
			} else {
				musics[simple_drums].volume = silent;
			}
		}

		if (hasHospital) {

		} else {

		}
		Debug.Log("hopeful:" + very_loud * ((float)highestCleared / (float)i) + " " + highestCleared + " " + i);
		musics[hopeful_synth].volume = very_loud * ((float)highestCleared / (float)i) - quiet;

	}
/* 
	public static void FadeMusics(AudioSource[] musics) {
		//Debug.Log(musics);
		int i=0;
		foreach(AudioSource music in musics) {
			music.volume = music.volume * .9f + musicTargetVolumes[i] * .1f;
			i++;
		}
	}
*/
}