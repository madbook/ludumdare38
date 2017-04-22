using UnityEngine;
using System.Collections.Generic;

public static class PopulationSetup {
  public static PersonController[] CreatePopulation(int numPopulation, GameObject template, RoomController[] rooms, TowerController towerController) {
    PersonController[] population = new PersonController[numPopulation];
    List<RoomController> availableRooms = new List<RoomController>(rooms);

    for (int i = 0; i < numPopulation; i++) {
      while (true) {
        int roomIndex = Random.Range(0, availableRooms.Count);
        RoomController room = availableRooms[roomIndex];
        if (room.IsRoomFull) {
          availableRooms.Remove(room);
          if (availableRooms.Count == 0) {
            Debug.Log("Not enough space found in rooms for full population!");
            return population;
          }
        } else {
          // Vector3 position = new Vector3(0f, 0f, 0f);
          GameObject clone = GameObject.Instantiate(
            template,
            room.transform,
            false
          );
          PersonController person = clone.GetComponent<PersonController>();
          towerController.PutPersonInRoom(person, room);
          population[i] = person;
          break;
        }
      }
    }

    return population;
  }
}