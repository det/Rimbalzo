#version 150

uniform mat4 modelview_matrix;
uniform mat4 projection_matrix;

in vec2 vert_position;
in vec2 vert_texcoord;

out vec2 frag_texcoord;

void main(void)  
{     
   gl_Position = projection_matrix * modelview_matrix * vec4(vert_position, 0.0, 1.0);
   frag_texcoord = vert_texcoord;
}