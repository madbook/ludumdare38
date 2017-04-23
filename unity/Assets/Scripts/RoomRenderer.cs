using UnityEngine;
using System.Collections.Generic;

public class RoomRenderer : MonoBehaviour {
	public GameObject roomModel;
	public GameObject farmScene;
	public GameObject rubbleScene;
	public GameObject powerScene;
	public GameObject constructionSprite;

	public GameObject popupContainer;
	public SpriteRenderer popupSpriteRenderer;
	public TextMesh popupText;

	public Sprite foodSprite;
	public Sprite powerSprite;

	RoomController room;

	static Dictionary<RoomType, Color> roomColors = new Dictionary<RoomType, Color>();

	static RoomRenderer() {
		roomColors.Add(RoomType.Power, new Color(.8f,.8f,0f,0f));
		roomColors.Add(RoomType.Farm, new Color(.5f,.6f,.5f,0f));
		// roomColors.Add(RoomType.Farm, new Color(0f,.8f,0f,0f));
		roomColors.Add(RoomType.Rubble, new Color(.1f,.1f,.1f,0f));
		roomColors.Add(RoomType.Empty, new Color(.5f,.5f,.5f,0f));
		roomColors.Add(RoomType.Filtration, new Color(0f,0f,.5f,0f));
		roomColors.Add(RoomType.Converter, new Color(.8f,.2f,0f,0f));
	}

	void Awake() {
		room = GetComponent<RoomController>();
	}

	public void Redraw() {
		RoomType key = room.type;

		Renderer[] children = roomModel.GetComponentsInChildren<Renderer>();

		foreach (Renderer rend in children) {
			if (roomColors.ContainsKey(key)) {
				rend.material.color = roomColors[key];
			}
		}

		if (room.type == RoomType.Farm && !farmScene.activeSelf) {
			farmScene.SetActive(true);
		} else if (room.type != RoomType.Farm && farmScene.activeSelf) {
			farmScene.SetActive(false);
		}

		if (room.type == RoomType.Rubble && !rubbleScene.activeSelf) {
			rubbleScene.SetActive(true);
		} else if (room.type != RoomType.Rubble && rubbleScene.activeSelf) {
			rubbleScene.SetActive(false);
		}

		if (room.type == RoomType.Power && !powerScene.activeSelf) {
			powerScene.SetActive(true);
		} else if (room.type != RoomType.Power && powerScene.activeSelf) {
			powerScene.SetActive(false);
		}

		if (room.assignment.assigned && !constructionSprite.activeSelf) {
			constructionSprite.SetActive(true);
		} else if (!room.assignment.assigned && constructionSprite.activeSelf) {
			constructionSprite.SetActive(false);
		}
	}

	public void RedrawOnFocus() {
		if (!ShouldPopupShow()) { return; }

		ResourceCalculator.Income currentIncome = room.GetTotalRoomIncome();

		if (room.type == RoomType.Farm) {
			popupSpriteRenderer.sprite = foodSprite;
			popupText.text = UIController.GetFoodString(currentIncome.food);
		} else if (room.type == RoomType.Power) {
			popupSpriteRenderer.sprite = powerSprite;
			popupText.text = UIController.GetPowerString(currentIncome.energy);
		}

		popupContainer.SetActive(true);
	}

	public void RedrawOnUnfocus() {
		popupContainer.SetActive(false);
	}

	bool ShouldPopupShow() {
		return (room.type == RoomType.Farm || room.type == RoomType.Power);
	}
}
