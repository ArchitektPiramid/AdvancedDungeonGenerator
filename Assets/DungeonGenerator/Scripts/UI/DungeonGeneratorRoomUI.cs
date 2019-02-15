using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DungeonGeneratorRoomUI : MonoBehaviour
{
    public Image ImgTop    = null;
    public Image ImgDown   = null;
    public Image ImgLeft   = null;
    public Image ImgRight  = null;
	public Image RoomTypeStart	= null;
	public Image RoomTypeBoss	= null;
	[Space(10f)]
	public Image _roomTypeIMG = null;
    private Image _myIMG = null;
    [Space(10)]
    public TMPro.TextMeshProUGUI txtRoom = null;

	private DungeonRoom _myRoomData = null;

    private void Awake() {
        this._myIMG = this.gameObject.GetComponent<Image>();
		this._roomTypeIMG.gameObject.SetActive(false);
    }

	public Vector2 GetRealPosition() {
		return Vector2.zero;
	}

	public void SetRoomData(DungeonRoom data) {
		this._myRoomData = data;
		this.SetDoorsOpen(data.doors);
		this.SetRoomType(data.roomType);
	}

    private void SetDoorsOpen(DungeonRoom.RoomDoors doors) {
        this.ImgTop.gameObject.SetActive(doors.HasFlag(DungeonRoom.RoomDoors.DOWN));
        this.ImgDown.gameObject.SetActive(doors.HasFlag(DungeonRoom.RoomDoors.UP));
        this.ImgLeft.gameObject.SetActive(doors.HasFlag(DungeonRoom.RoomDoors.LEFT));
        this.ImgRight.gameObject.SetActive(doors.HasFlag(DungeonRoom.RoomDoors.RIGHT));
    }

    private void SetRoomType(DungeonRoom.RoomType type) {
        switch (type)
        {
            case DungeonRoom.RoomType.Start:
                this._myIMG.color = Color.green;
				//this.SetRoomTypeIMG(this.RoomTypeStart);
                break;
            case DungeonRoom.RoomType.Boss:
                this._myIMG.color = Color.red;
				//this.SetRoomTypeIMG(this.RoomTypeBoss);
                break;
            case DungeonRoom.RoomType.Normal:
				this._myIMG.color = Color.white;
				this.SetRoomTypeIMG(null);
                break;
            case DungeonRoom.RoomType.Key:
				this._myIMG.color = Color.magenta;
				this.SetRoomTypeIMG(null);
                break;
            default:
				this.SetRoomTypeIMG(null);
				break;
        }
    }

	private void SetRoomTypeIMG(Image IMG) {
		if(IMG == null) { 
			this._roomTypeIMG.sprite = null;
			return; 
		}
		this._roomTypeIMG.sprite = IMG.sprite;
	}

    public void SetRoomTxt(string txt) {
        this.txtRoom.text = txt;
    }
}