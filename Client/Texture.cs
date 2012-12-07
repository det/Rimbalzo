
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
	public class Texture
	{
		int texId;
		public Size Size;
		
		public Texture (string path) : this(new Bitmap(path)) {}

		public Texture (Bitmap bitmap)
		{			
			texId = GL.GenTexture();
			Size = new System.Drawing.Size(bitmap.Width, bitmap.Height);

			var bitmapData = bitmap.LockBits(
				new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb
			);
			
			Bind();
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.LinearMipmapLinear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMinFilter.Linear);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Four, bitmap.Width, bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
		    bitmap.UnlockBits(bitmapData);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}
		
		public void Bind()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, texId);
		}
	}
}
