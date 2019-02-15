using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;


public class DungeonGenerator : MonoBehaviour {

	public DungeonGeneratorUI generateUI = null;
	public enum dir { UP, DOWN, LEFT, RIGHT }
	public int seed = 10;

	[SerializeField]
	public List<DungeonRoom> myRooms = new List<DungeonRoom>(); //TODO: priv
	public Vector3 myStartRoomPos;

	public Vector2Int dungMaxSize = new Vector2Int(5, 5);	//TODO: PRIV
	public int maxRooms = 5; //TODO: PRIV

	public int GetMaxDungSizeX { get { return this.dungMaxSize.x; } }
	public int GetMaxDungSizeY { get { return this.dungMaxSize.y; } }


	void Awake() {
		UnityEngine.Random.InitState(this.seed);
	}

	void Start() {
		this.GenerateDungeon();
	}

	private void Update() {
		RectTransform rt = (RectTransform)this.generateUI.maskContainer.transform;
		foreach (var item in myRooms) {
			if(item.roomType == DungeonRoom.RoomType.Start) {
				this.generateUI.elementsContainer.position = (-this.GetCurrentRoomUIPos(item)
				- new Vector2(rt.rect.width / 2f, rt.rect.height /2f))
				+ new Vector2(this.generateUI.maskContainer.position.x, this.generateUI.maskContainer.position.y);
			}
		}
	}

	public void GenerateDungeon() {
		this.myRooms.Clear();
		this.PlaceFirstRoom();
		this.PlaceAllRooms();
		this.SetBoosRoom();

		this.generateUI.placeAllRooms(this.myRooms);
	}

	public Vector2 GetCurrentRoomUIPos(DungeonRoom room) {
		if(!this.myRooms.Contains(room)) {
			return Vector2.zero;
		}
		return(this.generateUI.GetTotalImgSize() * (Vector2)room.GetPosition);
	}


    private void PlaceFirstRoom() {
		int startX = System.Convert.ToInt32(UnityEngine.Random.Range(0, this.GetMaxDungSizeX));
		int startY = System.Convert.ToInt32(UnityEngine.Random.Range(0, this.GetMaxDungSizeY));

		var startRoom = new DungeonRoom(new Vector2Int(startX, startY), DungeonRoom.RoomType.Start);
		this.AddRoomToDungeon(startRoom);
	}

	public void PlaceAllRooms() {
		int max = this.GetMaxDungSizeX * this.GetMaxDungSizeY;
		if(this.maxRooms >= max) {
			Debug.LogError("TO MUUCH ROOMS");
			this.maxRooms = max - 1;
		}

		while(this.myRooms.Count < this.maxRooms) {
			DungeonRoom parentRoom = this.GetRandomRoomWithFreeEdges(); //to connect
			List<Vector2Int> freePos = new List<Vector2Int>();
			freePos = this.GetAvaibleRoomsPos(parentRoom.GetPosition);
			Vector2Int pos = freePos[UnityEngine.Random.Range(0, freePos.Count)];

			var aa = parentRoom.GetPosition - pos;

			var newRoom = new DungeonRoom(pos, DungeonRoom.RoomType.Normal); //to connect

			if(aa.x == 1) {
				parentRoom.doors |= DungeonRoom.RoomDoors.LEFT;
				newRoom.doors |= DungeonRoom.RoomDoors.RIGHT;

			}
			if(aa.x == -1) {
				parentRoom.doors |= DungeonRoom.RoomDoors.RIGHT;
				newRoom.doors |= DungeonRoom.RoomDoors.LEFT;
			}
			if(aa.y == 1) {
				parentRoom.doors |= DungeonRoom.RoomDoors.UP;
				newRoom.doors |= DungeonRoom.RoomDoors.DOWN;
			}
			if(aa.y == -1) {
				parentRoom.doors |= DungeonRoom.RoomDoors.DOWN;
				newRoom.doors |= DungeonRoom.RoomDoors.UP;
				
			}
			this.AddRoomToDungeon(newRoom);
		}
	}

	private void SetBoosRoom() {
		foreach (var room in this.myRooms) {
			if(room.HasOneDoor() && room.roomType != DungeonRoom.RoomType.Start) {
				room.roomType = DungeonRoom.RoomType.Boss;
				return;
			}
		}
	}

	private void AddRoomToDungeon(DungeonRoom room) {
		// if(this.rooms.ContainsKey(pos)) {
		// 	Debug.LogError("ERRRRRROR");
		// 	return;
		// }
		// this.rooms.Add(pos, room);

		this.myRooms.Add(room);
	}


	private DungeonRoom GetRandomRoomWithFreeEdges() {
		this.myRooms.Shuffle();
		foreach (var room in this.myRooms) {
			if(this.AnyFreeEdges(room))
				return room;
		}
		return null;
	}

	private bool AnyFreeEdges(DungeonRoom room) {
		var posAv = this.GetAvaibleRoomsPos(room.roomPos); //TODO: property for room pos
		if(posAv.Count > 0) {
			return true;
		} else {
			return false;
		}
	}

	private List<Vector2Int> GetAvaibleRoomsPos(Vector2Int pos) {
		List<Vector2Int> allPosAvaibles = new List<Vector2Int>();

		Vector2Int posUp = new Vector2Int(pos.x, pos.y + 1);
		if(this.IsRoomAtPosAvaible(posUp) && pos.y < 5) { allPosAvaibles.Add(posUp); } //TODO:

		Vector2Int posDown = new Vector2Int(pos.x, pos.y - 1);
		if(this.IsRoomAtPosAvaible(posDown) && pos.y > 0) { allPosAvaibles.Add(posDown); }

		Vector2Int posRight = new Vector2Int(pos.x + 1, pos.y);
		if(this.IsRoomAtPosAvaible(posRight) && pos.x < 5) { allPosAvaibles.Add(posRight); }

		Vector2Int posLeft = new Vector2Int(pos.x - 1, pos.y);
		if(this.IsRoomAtPosAvaible(posLeft) && pos.x > 0) { allPosAvaibles.Add(posLeft); }

		return allPosAvaibles;
	}

	private bool IsRoomAtPosAvaible(Vector2Int pos) {
		foreach (DungeonRoom room in this.myRooms) {
			if(room.roomPos == pos)
				return false;
		} return true;
	}

	public DungeonRoom GetRoom(Vector2Int pos) {
		foreach (var item in this.myRooms) {
			if(item.GetPosition == pos)
				return item;
		} return null;
	}
}

[System.Serializable]
public class DungeonRoom
{
	[System.Flags]
	public enum RoomDoors {
		None = 0, 
        UP = 1, 
        DOWN = 2, 
        LEFT = 4, 
        RIGHT = 8
	}

	public enum RoomType {
		None, Start, Boss, Normal, Key
	}

	public Vector2Int roomPos = Vector2Int.zero;
	public RoomType roomType = RoomType.None;
	public RoomDoors doors = RoomDoors.None;

	public Vector2Int GetPosition { get { return this.roomPos; } }
	public int GetPosX { get { return this.roomPos.x; } }
	public int GetPosY { get { return this.roomPos.y; } }
	public int GetRoomHash { get { return this.roomPos.x.GetHashCode() ^ this.roomPos.y.GetHashCode(); } }

	public DungeonRoom(Vector2Int pos, RoomType type) {
		this.roomPos = pos;
		this.roomType = type;
	}

	public bool CheckPosition(Vector2Int posToCheck) {
		if(posToCheck.x == this.roomPos.x && posToCheck.y == this.roomPos.y) { //TODO: in better way
			return true;
		} return false;
	}
	
	public bool HasOneDoor() {
		int temp = 0;
		if(doors.HasFlag(RoomDoors.UP)) { temp += 1; }
		if(doors.HasFlag(RoomDoors.DOWN)) { temp += 1; }
		if(doors.HasFlag(RoomDoors.LEFT)) { temp += 1; }
		if(doors.HasFlag(RoomDoors.RIGHT)) { temp += 1; }
		if(temp == 1) {
			return true;
		} else {
			return false;
		}
	}
}
