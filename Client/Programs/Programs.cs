using System;
namespace Client
{
	public static class Programs
	{
		public static MaterialProgram Material;
		public static RasterProgram Raster;
		public static TraceProgram Trace;
		public static GoalProgram Goal;
		
		public static void Load()
		{
			Material = new MaterialProgram();
			Raster = new RasterProgram();
			Trace = new TraceProgram();
			Goal = new GoalProgram();
		}
	}
}

