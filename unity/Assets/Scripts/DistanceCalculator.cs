using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator {

	public struct Location {
		public int floor;
		public int face;
		public int position;
		
		public Location (int floor, int face, int position) {
			this.floor = floor;
			this.face = face;
			this.position = position;
		}
	}

	public static int DistanceBetween (Location loc_a, Location loc_b) {
		int totalRoomsPerFloor = TowerController.numRoomsPerFloor * TowerController.numFaces;
		int theta_a = loc_a.position + (TowerController.numFaces-loc_a.face) * TowerController.numRoomsPerFloor;
		int theta_b = loc_b.position + (TowerController.numFaces-loc_b.face) * TowerController.numRoomsPerFloor;
		int floor_distance = Mathf.Abs(loc_a.floor - loc_b.floor);
		int simple_distance = Mathf.Abs(theta_a - theta_b);
		int seam_distance = Mathf.Abs(totalRoomsPerFloor - theta_a - theta_b); // the distance if you traveled across the seam
		//Debug.Log("ta " + theta_a + "tb " + theta_b + "fd " + floor_distance + "sd " + simple_distance + "sd " + seam_distance );
		return floor_distance + Mathf.Min(simple_distance, seam_distance);
	}

	public static int NearestToLocations (Location loc_a, Location[] target_list) {
		int shortest_distance = 99999;
		foreach(Location target in target_list) {
			shortest_distance = Mathf.Min(shortest_distance, DistanceBetween(loc_a, target));
		}
		return shortest_distance;
	}
}
