using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
	public GameObject floor;
	public GameObject wall;
	public int squareSize = 5;

    void Start()
    {
		// Spawn a 4x4 room
		Vector3 startPosition = new Vector3(0, 0, 0);

		Instantiate(floor, startPosition, Quaternion.identity);
		Instantiate(floor, new Vector3(-squareSize, 0, 0), Quaternion.identity);
		Instantiate(floor, new Vector3(squareSize, 0, 0), Quaternion.identity);

		Instantiate(floor, new Vector3(-squareSize, squareSize, 0), Quaternion.identity);
		Instantiate(floor, new Vector3(0, squareSize, 0), Quaternion.identity);
		Instantiate(floor, new Vector3(squareSize, squareSize), Quaternion.identity);

		Instantiate(floor, new Vector3(-squareSize, -squareSize, 0), Quaternion.identity);
		Instantiate(floor, new Vector3(0, -squareSize, 0), Quaternion.identity);
		Instantiate(floor, new Vector3(squareSize, -squareSize), Quaternion.identity);

		// Spawn walls around the room
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
