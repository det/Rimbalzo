using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
	public class Fps
	{
		int framesSinceUpdate;
		float clock;
		Label label;

		public Fps ()
		{
			label = new Label(new Texture("Images/font.png"), "Waiting...");
			clock = 0f;
			framesSinceUpdate = 0;
		}

		public void Step(float elapsed)
		{
			clock += elapsed;
			
			if (clock>= 1f) {
				var fps = (float) framesSinceUpdate / clock + 0.5f;
				label.Text = String.Format("FPS: {0}", (int) fps);
				clock = 0f;
				framesSinceUpdate = 0;
			}			
		}
		
		public void Render()
		{
			framesSinceUpdate++;
			label.Render();
		}
	}
}

