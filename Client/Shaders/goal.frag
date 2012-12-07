#version 150

uniform sampler2D texture0;
uniform vec2 rotation;
uniform vec3 team_color;

in vec2 frag_position;

void main (void)  
{
	// Why must I negate rotation.y?
	vec2 frag_texcoord;
	frag_texcoord.x = frag_position.x * rotation.x - frag_position.y * -rotation.y;
	frag_texcoord.y = frag_position.x * -rotation.y + frag_position.y * rotation.x;
	vec4 tex_color = texture2D(texture0, frag_texcoord);

	float red = team_color.r * tex_color.r;
	float green = team_color.g * tex_color.g;
	float blue = team_color.b * tex_color.b;

    gl_FragColor = vec4(red, green, blue, tex_color.a);      
}
