#version 150

uniform sampler2D texture0;

in vec2 frag_texcoord;

void main (void)  
{  
   gl_FragColor = texture2D(texture0, frag_texcoord);
}
