using System;
using System.Collections.Generic;

namespace Shared
{
	public class MapCompiled
	{
		public List<WallCompiled> Walls;
		public int NumGoalTriangles;
		public List<GoalCompiled> Goals;
		public int NumSpeedupTriangles;
		public List<SpeedupCompiled> Speedups;		
		public SpawnData[] Spawns;
		public BallData[] Balls;
		public GoalieData[] Goalies;

		public MapCompiled (MapData data)
		{
			Walls = new List<WallCompiled>();
			foreach (var wallData in data.Walls) {
				var wall = new WallCompiled(wallData);
				Walls.Add(wall);
			}

			NumGoalTriangles = 0;
			Goals = new List<GoalCompiled>();
			foreach (var goalData in data.Goals) {
				var goal = new GoalCompiled(goalData);
				Goals.Add(goal);
				NumGoalTriangles += goal.Triangles.Count;
			}
			
			
			NumSpeedupTriangles = 0;
			Speedups = new List<SpeedupCompiled>();
			foreach (var speedupData in data.Speedups) {
				var speedup = new SpeedupCompiled(speedupData);
				Speedups.Add(speedup);
				NumSpeedupTriangles += speedup.Triangles.Count;
			}
	
			Spawns = data.Spawns;
			Balls = data.Balls;
			Goalies = data.Goalies;
		}
	}
}

