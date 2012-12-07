using System;
using Microsoft.Xna.Framework;
using Shared;

namespace Client
{
	public class OfflineWindow : MainWindow
	{
		public OfflineWindow (MapCompiled map) : base(map) {}
		
		public override bool IsLeftTurning { set { Ship.IsLeftTurning = value; } }
		public override bool IsRightTurning { set { Ship.IsRightTurning = value; } }
		public override bool IsBraking { set { Ship.IsBraking = value; } }
		public override bool IsThrusting { set { Ship.IsThrusting = value; } }
		public override bool IsBoosting { set { Ship.IsBoosting = value; } }
		public override bool IsReversing { set { Ship.IsReversing = value; } }
		public override bool IsShooting { set { Ship.IsShooting = value; } }
		public override Vector2 ShotVector { set { Ship.ShotVector = value; } }
	}
}

