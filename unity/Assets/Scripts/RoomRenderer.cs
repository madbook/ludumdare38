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
	public Sprite filtrationSprite;
	public Sprite converterSprite;

	public GameObject buildFarmPopup;
	public GameObject buildPowerPopup;
	public GameObject buildFiltrationPopup;
	public GameObject buildConverterPopup;
	public GameObject clearPopup;

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
		roomColors.Add(RoomType.Hospital, new Color(.8f,.6f,.6f,0f));
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

		SetSceneState(room.type ==RoomType.Farm, farmScene);
		SetSceneState(room.type ==RoomType.Rubble, rubbleScene);
		SetSceneState(room.type ==RoomType.Power, powerScene);
		SetSceneState(room.IsUnderConstruction, constructionSprite);
	}

	void SetSceneState(bool enable, GameObject scene) {
		if (enable && !scene.activeSelf) {
			scene.SetActive(true);
		} else if (!enable && scene.activeSelf) {
			scene.SetActive(false);
		}
	}

	public void RedrawOnFocus() {
		if (room.IsUnderConstruction) {
			return;
		} else if (ShouldShowResourcePopup()) {
			ResourceCalculator.Income currentIncome = room.GetTotalRoomIncome();

			if (room.type == RoomType.Farm) {
				popupSpriteRenderer.sprite = foodSprite;
				popupText.text = UIController.GetFoodString(currentIncome.food);
			} else if (room.type == RoomType.Power) {
				popupSpriteRenderer.sprite = powerSprite;
				popupText.text = UIController.GetPowerString(currentIncome.energy);
			} else if (room.type == RoomType.Converter) {
				popupSpriteRenderer.sprite = converterSprite;
				popupText.text = "+???";
			} else if (room.type == RoomType.Filtration) {
				popupSpriteRenderer.sprite = filtrationSprite;
				popupText.text = "+???";
			}

			popupContainer.SetActive(true);
			clearPopup.SetActive(true);
		} else if (room.type == RoomType.Empty) {
			buildFarmPopup.SetActive(true);
			buildPowerPopup.SetActive(true);
			buildConverterPopup.SetActive(true);
			buildFiltrationPopup.SetActive(true);
		} else {
			clearPopup.SetActive(true);
		}
	}

	public void RedrawOnUnfocus() {
		popupContainer.SetActive(false);
		buildFarmPopup.SetActive(false);
		buildPowerPopup.SetActive(false);
		buildConverterPopup.SetActive(false);
		buildFiltrationPopup.SetActive(false);
		clearPopup.SetActive(false);
	}

	public void RedrawUI() {
		RedrawOnUnfocus();
		RedrawOnFocus();
	}

	bool ShouldShowResourcePopup() {
		return (room.type != RoomType.Empty && room.type != RoomType.Rubble);
	}
}
