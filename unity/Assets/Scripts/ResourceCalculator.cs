public static class ResourceCalculator  {
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

	public static Income CalculateIncome(RoomController[] rooms){
		Income totalIncome = new Income(0,0);

		foreach (RoomController room in rooms) {
			totalIncome += room.Income;
		}
		// Debug.Log("Food: " + totalIncome.food + " Energy: " + totalIncome.energy);
		return totalIncome;
	}
}
