#version 150

uniform sampler2D texture;
uniform bool use_texture;
uniform vec3 team_color;

uniform vec4 light_diffuse;
uniform vec4 light_ambient;
uniform vec4 light_specular;
uniform vec4 light_position;

uniform vec4 material_diffuse;
uniform vec4 material_ambient;
uniform vec4 material_specular;
uniform float material_shininess;

in vec3 frag_normal;
in vec3 frag_vertex;
in vec2 frag_texcoord;

void main (void)  
{
   vec4 material_diffuse2;
   if (material_diffuse == vec4(0.0, 0.0, 0.0, 1.0)) material_diffuse2 = vec4(team_color, 1.0);
   else material_diffuse2 = material_diffuse;
   
   vec3 L = normalize(light_position.xyz - frag_vertex);
   vec3 E = normalize(-frag_vertex);
   vec3 R = normalize(-reflect(L, frag_normal));  
   
   vec4 ambient_color = material_ambient * light_ambient;    
   vec4 diffuse_color = material_diffuse2 * light_diffuse * max(dot(frag_normal, L), 0.0);
   diffuse_color = clamp(diffuse_color, 0.0, 1.0);     
   vec4 specular_color = material_specular * light_specular * pow(max(dot(R, E), 0.0), 0.3 * material_shininess);
   specular_color = clamp(specular_color, 0.0, 1.0); 
   vec4 lighting_color = ambient_color + diffuse_color + specular_color;
      
   if (use_texture) {
      vec4 tex_color = texture2D(texture, frag_texcoord);
      vec3 color1 = tex_color.rgb;
      color1 *= tex_color.a;
      vec3 color2 = team_color;
      color2 *= 1 - tex_color.a;   
      vec4 color = vec4(color1.r + color2.r, color1.g + color2.g, color1.b + color2.b, 1.0);      
      gl_FragColor = lighting_color * color;
   }
   else {
      gl_FragColor = lighting_color;
   }
}
