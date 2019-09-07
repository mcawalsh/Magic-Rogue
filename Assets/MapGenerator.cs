using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
	public GameObject grid;
	public GameObject floor;
	public GameObject wall;

	public int mapWidthX, mapHeightY;

	private List<Walker> listOfWalkers;

	public Tilemap floorMap, wallMap;
	private List<Vector2> listOfFloorTiles;
	private List<Vector2> listOfWallTiles;

	public TileBase floorTile;
	public TileBase wallTile;

	void Start()
	{
		grid.transform.position = new Vector3(-(mapWidthX / 2), -(mapHeightY / 2), 0);

		listOfWalkers = new List<Walker>();
		listOfFloorTiles = new List<Vector2>();
		listOfWallTiles = new List<Vector2>();

		ProcGenFloorWalkersSetup();
		ProcGenWallSetup();
		ProcGenWallFixtures(mapWidthX, mapHeightY);
	}

	/* Sets up the Walkers that will construct the floor grid later on */
	public void ProcGenFloorWalkersSetup()
	{
		int initialNumberOfWalkers = 10; // How many walkers we'll initially create
		int initialNumberOfWalkeriterations = 1000;

		// Create walkers
		for (int i = 0; i < initialNumberOfWalkers; i++)
        {
			Walker _walker = new Walker();
			_walker.walkerPosition = new Vector2((int)mapWidthX / 2, (int)mapHeightY / 2); // Spawn them at the center of the map
			listOfWalkers.Add(_walker);
		}

		// Runs WalkerBehaviour() a fixed number of times. Each time this functions run, each walker of the list moves, replicates, or die, building a path of floors in each movement
		for (int i = 0; i < initialNumberOfWalkeriterations; i++) 
        {
			WalkerBehaviour(listOfWalkers);
		}

	}

	void WalkerBehaviour(List<Walker> _inputListOfWalkers) {

        foreach (var _walker in _inputListOfWalkers)
        {

            _walker.walkerPosition = RandomDirection(_walker.walkerPosition); // Walker new position will be a new position in a random direction RandomDirection()
			wallMap.SetTile(new Vector3Int((int) _walker.walkerPosition.x, (int) _walker.walkerPosition.y, 0), null);
            floorMap.SetTile(new Vector3Int((int) _walker.walkerPosition.x, (int) _walker.walkerPosition.y, 0), floorTile);
            listOfFloorTiles.Add(_walker.walkerPosition);


            int _rand = UnityEngine.Random.Range(0, 100);
            if (_rand <= 30) // 30% chance of new walker being born
            {
                Walker _newWalker = new Walker();
				_newWalker.walkerPosition = _walker.walkerPosition; // New walker position is the same as the old walker position
                listOfFloorTiles.Add(_walker.walkerPosition);
                _inputListOfWalkers.Add(_newWalker); // Problem: this will not execute here because we're still within the loop, and the LIST listOfWalkers cannot be modified by adding a new item while is running. That's why we add break to get out of the loop.
                break;
            }
        }
    }

	Vector2 RandomDirection(Vector2 currentWalkerPosition)
	{

		float _rand = (int)Random.Range(0, 3.99f); // 0, 1, 2, 3
		switch (_rand)
		{
			case 0: //up x=0 y++
				currentWalkerPosition.y += 1.0f;
				break;
			case 1: //down x=0 y--
				currentWalkerPosition.y -= 1.0f;
				break;
			case 2: //left x-- y=0
				currentWalkerPosition.x -= 1.0f;
				break;
			case 3://right x++ y=0
				currentWalkerPosition.x += 1.0f;
				break;
			default:
				break;

		}

		/* Boundaries:
        BottomLeft: 0,0
        BottomRight: mapWidthX, 0
        TopLeft: 0, mapHeightY
        TopRight: mapWidthX, mapHeightY
        */
		if (currentWalkerPosition.x < 0) // Checks X-- and turns the walker around
        {
			currentWalkerPosition.x = currentWalkerPosition.x * (-1);

		} else if (currentWalkerPosition.y < 0) // Checks Y-- and turns the walker around
        {
			currentWalkerPosition.y = currentWalkerPosition.y * (-1);

		}

		return new Vector2(currentWalkerPosition.x, currentWalkerPosition.y);

	}

	public void ProcGenWallSetup()
	{

		int width = mapWidthX;
		int height = mapHeightY;
		for (int x = 0; x < width; x++)
        {
			for (int y = 0; y < height; y++)
            {
				if (floorMap.GetTile(new Vector3Int(x, y, 0)) == null)
				{
					Vector2 _wallTileLocation = new Vector2(x, y);
					wallMap.SetTile(new Vector3Int((int)_wallTileLocation.x, (int)_wallTileLocation.y, 0), wallTile);
					listOfWallTiles.Add(_wallTileLocation);
				}
			}
		}
	}

	void ProcGenWallFixtures(int width, int height)
	{
		// Fills with walls the bottom and ceiling map openings from the walkers
		for (int i = 0; i <  width; i++)
        {
			if (floorMap.GetTile(new Vector3Int(i, 0, 0)) != null)
			{
				floorMap.SetTile(new Vector3Int(i, 0, 0), null); // Clear potential previous floors
				wallMap.SetTile(new Vector3Int(i, 0, 0), wallTile);
				wallMap.SetColor(new Vector3Int(i, 0, 0), Color.black);


				// Add to our list of walls:
				Vector2 _wallTileLocation = new Vector2(i, 0);
				listOfWallTiles.Add(_wallTileLocation); // + list
				listOfFloorTiles.Remove(_wallTileLocation); // - list
			}
			if (floorMap.GetTile(new Vector3Int(i, height - 1, 0)) != null)
			{
				floorMap.SetTile(new Vector3Int(i, height - 1, 0), null); // Clear potential previous floors
				wallMap.SetTile(new Vector3Int(i, height - 1, 0), wallTile);
				wallMap.SetColor(new Vector3Int(i, height - 1, 0), Color.black);

				// Add to our list of walls:
				Vector2 _wallTileLocation = new Vector2(i, height - 1);
				listOfWallTiles.Add(_wallTileLocation);// + list
				listOfFloorTiles.Remove(_wallTileLocation); // - list
			}
		}
		// Fills with walls the right and left map openings from the walkers
		for (int i = 0; i < height; i++)
        {
			if (floorMap.GetTile(new Vector3Int(0, i, 0)) != null)
			{
				wallMap.SetTile(new Vector3Int(0, i, 0), wallTile);
				wallMap.SetColor(new Vector3Int(0, i, 0), Color.black);
				// Add to our list of walls:
				Vector2 _wallTileLocation = new Vector2(0, i);


				floorMap.SetTile(new Vector3Int(0, i, 0), null); // Clear potential previous floors
				listOfWallTiles.Add(_wallTileLocation);
				listOfFloorTiles.Remove(_wallTileLocation); // Remove them for the list as well so we can use flood algo
			}
			if (floorMap.GetTile(new Vector3Int(i, height - 1, 0)) != null)
			{
				wallMap.SetTile(new Vector3Int(height, i, 0), wallTile);
				wallMap.SetColor(new Vector3Int(height, i, 0), Color.black);
				// Add to our list of walls:
				Vector2 _wallTileLocation = new Vector2(height, i);

				floorMap.SetTile(new Vector3Int(height, i, 0), null); // Clear potential previous floors
				listOfWallTiles.Add(_wallTileLocation);
				listOfFloorTiles.Remove(_wallTileLocation); // Remove them for the list as well so we can use flood algo
			}
		}
	}
}

public class Walker
{
	public Vector2 walkerPosition;
	public Vector2 walkerDirection;
}
