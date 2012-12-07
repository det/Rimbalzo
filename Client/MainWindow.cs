using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Shared;

namespace Client
{
	public abstract class MainWindow
	{
		public GameWindow GameWindow;
		public Fps Fps;
		public Starfield Starfield;
		public MapMesh MapMesh;
		public SensorMesh<GoalCompiled> GoalMesh;
		public SensorMesh<SpeedupCompiled> SpeedupMesh;
		public ObjMesh ShipMesh;
		public ObjMesh GoalieMesh;
		public ObjMesh BallMesh;
		public Simulation Simulation;
		public Ship Ship;
		public Image Nebula;
		public Image BallSpawn;

		float aspect;
		float rotate;
		float zoom;
		float maxZoom;
		float minZoom;
		public float xPos;
		public float yPos;
		bool isOrtho;
		bool showNebula;
		MapCompiled map;

		static Vector4 light_ambient = new Vector4(.8f, .8f, .8f, 1f);
		static Vector4 light_diffuse = new Vector4(.8f, .8f, .8f, 1f);
		static Vector4 light_specular = new Vector4(.8f, .8f, .8f, 1f);
		static Vector4 light_position = new Vector4(100f, 100f, 100f, 1f);
		
		public abstract bool IsLeftTurning { set; }
		public abstract bool IsRightTurning { set; }
		public abstract bool IsBraking { set; }
		public abstract bool IsThrusting { set; }
		public abstract bool IsBoosting { set; }
		public abstract bool IsReversing { set; }
		public abstract bool IsShooting { set; }
		public abstract Microsoft.Xna.Framework.Vector2 ShotVector { set; }		

		public MainWindow (MapCompiled map)
		{
			this.map = map;
			GameWindow = new GameWindow(800, 600);
			GameWindow.Title = "Rimbazlo";
			//var graphicsMode = new GraphicsMode(new ColorFormat(8, 8, 8, 8));			
			//GameWindow = new GameWindow(640, 480, graphicsMode, "Rimbalzo", GameWindowFlags.Default, DisplayDevice.Default, 3, 1, GraphicsContextFlags.Default);
			
			//GameWindow = new GameWindow(640, 480);
			//GameWindow.TargetRenderPeriod = 0f;

			zoom = 80f;
			rotate = 0f;
			minZoom = 2f;
			maxZoom = 1000f;
			xPos = 0f;
			yPos = 0f;
			isOrtho = false;
			showNebula = false;
			
			GameWindow.Resize += HandleWindowResize;			
			GameWindow.Keyboard.KeyDown += HandleWindowKeyboardKeyDown;
			GameWindow.Keyboard.KeyUp += HandleGameWindowKeyboardKeyUp;
			GameWindow.Mouse.WheelChanged += HandleWindowMouseWheelChanged;
			GameWindow.Mouse.ButtonDown += HandleGameWindowMouseButtonDown;
			GameWindow.Mouse.ButtonUp += HandleGameWindowMouseButtonUp;
			GameWindow.Load += HandleLoad;
			GameWindow.RenderFrame += HandleRenderFrame;
			GameWindow.UpdateFrame += HandleUpdateFrame;;
		}

		void HandleGameWindowMouseButtonUp (object sender, MouseButtonEventArgs e)
		{
			switch (e.Button) {
			case MouseButton.Left:
				IsShooting = false;
				break;
			case MouseButton.Right:
				IsBoosting = false;
				break;
			}
		}

		void HandleGameWindowMouseButtonDown (object sender, MouseButtonEventArgs e)
		{
			switch (e.Button) {
			case MouseButton.Left:
				IsShooting = true;
				break;
			case MouseButton.Right:
				IsBoosting = true;
				break;
			}
		}		

		void HandleUpdateFrame (object sender, FrameEventArgs e)
		{
			if (GameWindow.Mouse[MouseButton.Left]) {
				Microsoft.Xna.Framework.Vector2 v;
				v.X = GameWindow.Mouse.X - GameWindow.Width/2f;
				v.Y = GameWindow.Height/2f - GameWindow.Mouse.Y;
				v.Normalize();
				ShotVector = v;
			}
			
			var elapsed = (float) e.Time;
			Simulation.Step(elapsed);			
		}

		void HandleLoad (object sender, EventArgs e)
		{
			Programs.Load();
			Fps = new Fps();
			Starfield = new Starfield();
			MapMesh = new MapMesh(map);
			GoalMesh = new SensorMesh<GoalCompiled>(map.Goals, new Texture("Images/goal.png"));
			SpeedupMesh = new SensorMesh<SpeedupCompiled>(map.Speedups, new Texture("Images/speedup.png"));
			ShipMesh = new ObjMesh("Models/ship.obj");
			BallMesh = new ObjMesh("Models/ball.obj");
			GoalieMesh = new ObjMesh("Models/goalie.obj");
			Simulation = new Simulation(map);
			Ship = Simulation.Ships[0];
			Nebula = new Image("Images/nebula.png", GameWindow.Width, GameWindow.Height);
			BallSpawn = new Image("Images/ballspawn.png", -1f, 1f, -1f, 1f);
			
			GL.ClearColor(Color.Black);
			GL.ClearDepth(1.0f);

			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Lequal);
			
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			
			GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			GL.Hint(HintTarget.GenerateMipmapHint, HintMode.Nicest);
			GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
			GL.Hint(HintTarget.PointSmoothHint, HintMode.Nicest);
			GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
			
			Programs.Material.Use();
			Programs.Material.LightDiffuse = light_diffuse;
			Programs.Material.LightAmbient = light_ambient;
			Programs.Material.LightSpecular = light_specular;
			Programs.Material.LightPosition = light_position;
		}

		void HandleWindowMouseWheelChanged (object sender, MouseWheelEventArgs e)
		{
			if (GameWindow.Keyboard[Key.ShiftLeft]) { zoom -= e.DeltaPrecise*8f;	}
			else { zoom -= e.DeltaPrecise*2f; }
			ClampZoom();
		}

		void HandleWindowKeyboardKeyDown (object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key) {
			case Key.Escape:
				GameWindow.Exit();
				break;
			case Key.F:
				if (GameWindow.WindowState == WindowState.Fullscreen) { GameWindow.WindowState = WindowState.Normal; }
				else { GameWindow.WindowState = WindowState.Fullscreen; }
				break;
			case Key.W:
				IsThrusting = true;
				break;
			case Key.A:
				IsLeftTurning = true;
				break;
			case Key.S:
				IsReversing = true;
				break;
			case Key.D:
				IsRightTurning = true;
				break;
			case Key.ShiftLeft:
				IsBraking = true;
				break;

			case Key.PageUp:
				rotate += FMath.PiOver2;
				rotate %= FMath.TwoPi;
				break;
			case Key.PageDown:
				rotate -= FMath.PiOver2;
				rotate %= FMath.TwoPi;
				break;
			case Key.Home:
				if (isOrtho) isOrtho = false;
				else isOrtho = true;
				break;
			case Key.End:
				if (showNebula) showNebula = false;
				else showNebula = true;
				break;
			}
		}
		
		void HandleGameWindowKeyboardKeyUp (object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key) {
			case Key.W:
				IsThrusting = false;
				break;
			case Key.A:
				IsLeftTurning = false;
				break;
			case Key.S:
				IsReversing = false;
				break;
			case Key.D:
				IsRightTurning = false;				
				break;
			case Key.ShiftLeft:
				IsBraking = false;
				break;
			case Key.F1:
				var bytes = Simulation.ToBytes();
				File.WriteAllBytes("test.save", bytes);
				break;
			case Key.F2:
				var bytes2 = File.ReadAllBytes("test.save");
				Simulation.FromBytes(bytes2);
				break;

			}			
		}
		
		void ClampZoom()
		{
			if (zoom < minZoom) { zoom = minZoom; }
			else if (zoom > maxZoom) { zoom = maxZoom; }
		}

		void HandleRenderFrame (object sender, FrameEventArgs e)
		{
			var elapsed = (float) e.Time;
			Fps.Step(elapsed);
			Programs.Goal.Step(elapsed);
			
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			var p = Matrix4.Identity;
			p *= Matrix4.CreateTranslation(-Ship.Fixture.Body.Position.X, -Ship.Fixture.Body.Position.Y, -zoom);
			p *= Matrix4.CreateRotationZ(rotate);
			if (isOrtho) {
				var z = zoom * FMath.PiOver4;
				p *= OpenTK.Matrix4.CreateOrthographic(z*aspect, z, -1000f, 1000f);
			}
			else {
				p *= Matrix4.CreatePerspectiveFieldOfView(FMath.PiOver4, aspect, 1f, 2000f); 
			}

			var windowMatrix = Matrix4.CreateOrthographicOffCenter(0f, GameWindow.Width, 0f, GameWindow.Height, -1f, 1f);			

			Programs.Raster.Use();
			
			if (showNebula) {								
				Programs.Raster.ProjectionMatrix = windowMatrix;
				Programs.Raster.ModelviewMatrix = Matrix4.Identity;
				Nebula.Render();
			}

			Programs.Raster.ProjectionMatrix = p;
			foreach (var ball in Simulation.Balls) {
				if (ball.State == BallState.Spawning) {
					var unit = ball.SpawnClock / Ball.SpawnTime;
					var rot = unit * FMath.TwoPi;
					var m = Matrix4.Identity;
					m *= Matrix4.Scale(unit, unit, 0f);
					m *= Matrix4.CreateRotationZ(rot);					
					m *= Matrix4.CreateTranslation(ball.SpawnPos.X, ball.SpawnPos.Y, 0f);
					Programs.Raster.ModelviewMatrix = m;
					BallSpawn.Render();
				}
			}
			
			Programs.Trace.Use();
			Programs.Trace.ProjectionMatrix = windowMatrix;
			Programs.Trace.ModelviewMatrix = Matrix4.Identity;
			Starfield.Render();

			Programs.Goal.Use();			
			Programs.Goal.ProjectionMatrix = p;
			Programs.Goal.ModelviewMatrix = Matrix4.Identity;
			GoalMesh.Render();
			SpeedupMesh.Render();
			
			Programs.Material.Use();
			Programs.Material.ModelviewMatrix = Matrix4.Identity;
			Programs.Material.ProjectionMatrix = p;
			Programs.Material.UseTexture = true;
			MapMesh.Render();
			
			Programs.Material.UseTexture = false;
			foreach (var ship in Simulation.Ships) {
				var m = Matrix4.Identity;
				m *= Matrix4.CreateRotationZ(ship.Fixture.Body.Rotation);
				m *= Matrix4.CreateTranslation(ship.Fixture.Body.Position.X, ship.Fixture.Body.Position.Y, 0f);
				Programs.Material.ModelviewMatrix = m;
				Programs.Material.TeamColor = ship.Color;
				ShipMesh.Render();
				if (ship.Ball != null) BallMesh.Render();
			}			

			foreach (var goalie in Simulation.Goalies) {
				var m = Matrix4.Identity;
				m *= Matrix4.CreateRotationZ(goalie.Fixture.Body.Rotation);
				m *= Matrix4.CreateTranslation(goalie.Fixture.Body.Position.X, goalie.Fixture.Body.Position.Y, 0f);
				Programs.Material.ModelviewMatrix = m;
				GoalieMesh.Render();				
			}			
			
			foreach (var ball in Simulation.Balls) {
				if (ball.State == BallState.Free) {
					var m = Matrix4.Identity;
					m *= Matrix4.CreateTranslation(ball.Fixture.Body.Position.X, ball.Fixture.Body.Position.Y, 0f);
					Programs.Material.ModelviewMatrix = m;
					BallMesh.Render();
				}				
			}
			
			Programs.Raster.Use();
			Programs.Raster.ProjectionMatrix = windowMatrix;
			Programs.Raster.ModelviewMatrix = Matrix4.Identity;
			Fps.Render();

			GameWindow.SwapBuffers();			
		}

		void HandleWindowResize (object sender, EventArgs e)
		{
			GL.Viewport(0, 0, GameWindow.Width, GameWindow.Height);
			aspect = (float) GameWindow.Width / (float) GameWindow.Height;
			Nebula.SetSize(GameWindow.Width, GameWindow.Height);
		}
		
		public void Run() {
			GameWindow.Run(60, 100);
		}
	}
}
