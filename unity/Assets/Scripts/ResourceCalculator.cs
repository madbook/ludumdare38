using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCalculator  {

	private static float elapsed;

	public struct Income{
		public float food;
		public float energy;
		public Income(float food, float energy) {
			this.food = food;
			this.energy = energy;
		}

		public static Income operator + (Income a, Income b) {
			return new Income(a.food + b.food, a.energy + b.energy);
		}
	}

	TowerController towerController;

	public static void Update (TowerController towerController) {
		if (Mathf.Floor(elapsed + Time.deltaTime) > Mathf.Floor(elapsed)) {

			CalculateIncome(towerController);
		}
		elapsed += Time.deltaTime;
	}
	
	private static void CalculateIncome(TowerController towerController){
		Income totalIncome = new Income(0,0);
		foreach (RoomController room in towerController.rooms) {
			totalIncome += room.Income;
		}
		Debug.Log("Food: " + totalIncome.food + " Energy: " + totalIncome.energy);
	}
}
