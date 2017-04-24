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

	public static Income stockpile;

	public static Income CalculateRoomIncome(RoomController[] rooms){
		Income totalIncome = new Income(0,0);

		int filters = 0;
		int converters = 0;
		
		foreach (RoomController room in rooms) {
			if(room.type == RoomType.Filtration && room.WorkerCount > 0){
				filters++;
			}
			if(room.type == RoomType.Converter && room.WorkerCount > 0){
				converters++;
			}
		}

		foreach (RoomController room in rooms) {
			totalIncome += room.GetIncome(filters, converters);
		}
		return totalIncome;
	}

	public static Income CalculatePeopleIncome(PersonController[] people){
		Income totalIncome = new Income(0,0);
		foreach (PersonController peop in people) {
			totalIncome += new Income(-0.25f,0);
		}
		return totalIncome;
	}
}
