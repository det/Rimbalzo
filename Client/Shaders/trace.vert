#version 150

uniform mat4 modelview_matrix;
uniform mat4 projection_matrix;

in vec2 vert_position;

void main(void)  
{     
   gl_Position = projection_matrix * modelview_matrix * vec4(vert_position, 0.0, 1.0);
}