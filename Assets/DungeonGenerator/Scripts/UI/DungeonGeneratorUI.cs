using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Animations;

public class DungeonGeneratorUI : MonoBehaviour
{
    public DungeonGeneratorRoomUI SingleRoomPrefab = null;
    [SerializeField] private Vector2 imgOffset = new Vector2(0f, 0f);
	public Vector2 scalingSize = new Vector2(1f, 1f);
	[SerializeField] public Transform elementsContainer = null;
    [SerializeField] public Transform maskContainer = null;
	private ScaleConstraint _myScale = null;

    //public RectTransform _myImgPref = null;
    private float _imgX = 0f;
    private float _imgY = 0f;
    private List<DungeonGeneratorRoomUI> _myRoomList = new List<DungeonGeneratorRoomUI>();

	public Vector2 GetTotalImgSize() {
		return new Vector2(this._imgX, this._imgY);
	}

    private void Awake() {
		this._myScale = this.elementsContainer.gameObject.GetComponent<ScaleConstraint>();
        var tempRect = this.SingleRoomPrefab.gameObject.GetComponent<RectTransform>();
        this._imgX = tempRect.rect.width + this.imgOffset.x;
        this._imgY = tempRect.rect.height + this.imgOffset.y;
		// this._myImgPref = this.SingleRoomPrefab.gameObject.GetComponent<RectTransform>();
        // this._myImgPref.gameObject.SetActive(true);
        // this._imgX = this._myImgPref.rect.width + this.imgOffset.x;
        // this._imgY = this._myImgPref.rect.height + this.imgOffset.y;
        //ChangeMapSize(2f); //TODO:
    }

	private void Start() {
		_myScale.scaleAtRest = (Vector3)scalingSize;
	}


    public void placeAllRooms(List<DungeonRoom> myRooms) { //TODO: PlaceAllRooms name
        this.Clear();
        foreach(DungeonRoom room in myRooms) {
            DungeonGeneratorRoomUI newRoom = Instantiate(this.SingleRoomPrefab, this.transform.position, Quaternion.identity, this.elementsContainer);
            newRoom.gameObject.SetActive(true);
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