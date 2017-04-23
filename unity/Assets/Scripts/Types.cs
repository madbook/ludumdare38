
public enum JobAssignment {
	Dead,
	// Person has no job
	Idle,
	// Person is in transit to a room, so not idle but not yet working
	GoingToRoom,
	// Person is operating the room they are in
	// We may want to split this out into explicit jobs if we have individual
	// skill or something, or multiple jobs per room, but for now we can keep it
	// simple
	OperatingRoom,
	BuildingRoom
};

public enum RoomType {
	Empty,
	Power,
	Farm,
	Rubble,
	Converter,
	Filtration
};

public struct ColorKey {
	public readonly RoomType type;
	public readonly bool isFocused;

	public ColorKey(RoomType type, bool isFocused) {
		this.type = type;
		this.isFocused = isFocused;
	}
};
