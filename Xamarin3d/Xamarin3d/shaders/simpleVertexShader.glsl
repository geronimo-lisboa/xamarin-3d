
uniform mat4 modelMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
//A cor do vertice, serve pra testar se realmente está funcionando direito a introspecção
attribute vec4 vColor;
//A posição espacial do vertice atual.
attribute vec3 vPosition;

varying vec4 vertexColor;
void main()
{
	mat4 mvp = projectionMatrix * viewMatrix * modelMatrix;

	gl_Position = mvp * vec4(vPosition, 1.0);
	vertexColor = vColor;
}