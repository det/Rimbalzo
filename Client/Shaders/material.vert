#version 150

uniform mat4 modelview_matrix;
uniform mat4 projection_matrix;

in vec3 vert_position;
in vec3 vert_normal;
in vec2 vert_texcoord;

out vec3 frag_normal;
out vec3 frag_vertex;
out vec2 frag_texcoord;

void main(void)  
{
   vec4 vertex = vec4(vert_position, 1.0);
   frag_vertex = vec3(modelview_matrix * vertex);
   frag_normal = normalize((modelview_matrix * vec4(vert_normal, 0)).xyz);

   gl_Position = projection_matrix * modelview_matrix * vertex;

   frag_texcoord = vert_texcoord;
}
