using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR

[CustomEditor(typeof(DungeonGenerator))]
public class DungeonGeneratorEditor : Editor
{
	private Color colInit = new Color(51, 255, 255, 255);
	private Color colNormal = new Color(0, 204, 0, 255);

	private DungeonGenerator generator;
	private bool _toggleList = false;

	private void OnEnable() {
		this.generator = target as DungeonGenerator;
		//this.generator.GenerateDungeon();
	}

	public override void OnInspectorGUI() {
		this.DrawDefaultInspector();

		//DungeonGenerator myScript = (DungeonGenerator)target;

		//this.GenRooms();
		var styleGenerate = new GUIStyle(GUI.skin.button);
		styleGenerate.fixedHeight = 100f;
		if(GUILayout.Button("GENERATE", style: styleGenerate)) {
			generator.GenerateDungeon();
		}


		if(this._toggleList) {
			foreach (var item in this.generator.myRooms) {
				GUILayout.BeginHorizontal();
				GUILayout.Label("ID: " + item.GetRoomHash.ToString());
				GUILayout.Label("pos: " + item.GetPosX.ToString() + ":" + item.GetPosY);
				GUILayout.Label("type: " + item.roomType.ToString());
				GUILayout.EndHorizontal();
			}
		}
	}

	public void GenRooms() {
		Color defaultCol = GUI.backgroundColor;

		for (int y = 0; y < generator.GetMaxDungSizeY; y++) {
			
			GUILayout.BeginHorizontal();
			for (int x = 0; x < generator.GetMaxDungSizeX; x++) {
				var style = new GUIStyle(GUI.skin.button);
				style.fixedHeight = 70f;
				style.fixedWidth = 70f;

				foreach (var room in generator.myRooms) {
					if(room.CheckPosition(new Vector2Int(x, y))) { 
						GUI.backgroundColor = Color.red;
						if(room.roomType == DungeonRoom.RoomType.Start) GUI.backgroundColor = Color.green; 
						if(room.roomType == DungeonRoom.RoomType.Normal) GUI.backgroundColor = Color.yellow;
					}
				}

				if(generator.GetRoom(new Vector2Int(x, y)) != null) {
					var room = generator.GetRoom(new Vector2Int(x, y));
					string txt = x + ":" + y;
					if((room.doors & DungeonRoom.RoomDoors.LEFT) == DungeonRoom.RoomDoors.LEFT) txt = "= " + txt;
					if((room.doors & DungeonRoom.RoomDoors.RIGHT) == DungeonRoom.RoomDoors.RIGHT) txt += " =";
					if((room.doors & DungeonRoom.RoomDoors.DOWN) == DungeonRoom.RoomDoors.DOWN) txt += "\n ||";
					if((room.doors & DungeonRoom.RoomDoors.UP) == DungeonRoom.RoomDoors.UP) txt = "||\n" + txt;
					GUILayout.Button(txt, style);
				} else {
					string txt = x + ":" + y;
					GUILayout.Button(txt, style);
				}

				GUI.backgroundColor = defaultCol;
			}
			GUILayout.EndHorizontal();
		}
	
		// custom ------------------
		GUILayout.BeginHorizontal();
		if(this._toggleList) {
			if(GUILayout.Button("Hide")) { this._toggleList = !this._toggleList; }
		} else {
			if(GUILayout.Button("Expand")) { this._toggleList = !this._toggleList; }
		}

		GUILayout.Label("Number of rooms:");
		EditorGUILayout.IntField(this.generator.myRooms.Count);
		GUILayout.EndHorizontal();
	}
}

#endif