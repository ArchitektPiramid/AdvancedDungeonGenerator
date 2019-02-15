using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DungeonGeneratorUI : MonoBehaviour
{
    public DungeonGeneratorRoomUI SingleRoomPrefab = null;
    [SerializeField] private float imgOffset = 0f;
	[SerializeField] public Transform elementsContainer = null;
    [SerializeField] public Transform maskContainer = null;

    private RectTransform _myImgPref = null;
    private float _imgX = 0f;
    private float _imgY = 0f;
    private List<DungeonGeneratorRoomUI> _myRoomList = new List<DungeonGeneratorRoomUI>();

	public Vector2 GetTotalImgSize() {
		return new Vector2(this._imgX, this._imgY);
	}

    private void Awake() {
        this._myImgPref = this.SingleRoomPrefab.gameObject.GetComponent<RectTransform>();
        this._imgX = this._myImgPref.rect.width + this.imgOffset;
        this._imgY = this._myImgPref.rect.height + this.imgOffset;
        //ChangeMapSize(2f); //TODO:
    }

    public void placeAllRooms(List<DungeonRoom> myRooms) { //TODO: PlaceAllRooms name
        this.Clear();
        foreach(DungeonRoom room in myRooms) {
            DungeonGeneratorRoomUI newRoom = Instantiate(this.SingleRoomPrefab, this.transform.position, Quaternion.identity, this.elementsContainer);
            newRoom.SetRoomTxt(string.Format("{0}:{1}", room.GetPosX, room.GetPosY));
            newRoom.SetRoomData(room);
		    newRoom.transform.localPosition = new Vector2(_imgX * room.GetPosX, _imgY * room.GetPosY);
            this._myRoomList.Add(newRoom);
        }
    }

    private void Clear() { //TODO: ClearAllRooms name
        if(this._myRoomList.Count <= 0) { return; }
        for (int i = 0; i < this._myRoomList.Count; i++) {
            Destroy(this._myRoomList[i].gameObject);
        }
        this._myRoomList.Clear();
    }

    public void ChangeMapSize(float newSize) {
        RectTransform rt = (RectTransform)this.elementsContainer.transform;
        rt.localScale = new Vector2(newSize, newSize);
    }
}