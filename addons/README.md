# Godot UV Manipulation Node
This is a visual shader addon for Godot 4.2+. This adds the UVManipulation node to the visual shader editor.
![image](https://github.com/user-attachments/assets/0c0ec2cc-9635-4e8f-bf48-104f19f1467c)


# Method
This node allows you to change pivot point, scale, rotate and offset a UV.

The order of operation is: `Change Pivot` > `Scale` > `Rotate` > `Reset Pivot` > `Offset`

The rotate function is created first.

	vec2 rotateUV(vec2 uv_in, float angle) {
		  float angle_rad = radians(angle);
		  mat2 rotate = mat2(
			  vec2(cos(angle_rad), -sin(angle_rad)),
			  vec2(sin(angle_rad), cos(angle_rad))
		  );
	  	return rotate * uv_in;
	}

Then the UV manipulation.

	vec2 UVManip(vec2 uv, float scale_placeholder, float rotation_placeholder, vec2 pivot_placeholder, vec2 offset_placeholder) {
	  vec2 pivot_internal = clamp(pivot_placeholder, 0.0, 1.0);
	  uv -= pivot_internal;
	  uv *= scale_placeholder;
	  uv = rotateUV(uv, rotation_placeholder);
	  uv += pivot_internal;
	  uv += offset_placeholder;
	  return uv;
	}

# Installation

Extract the zip file and copy the folder to your project. You'll need to restart the editor for the node to appear in visual shader.
Alternatively, you can make a new gdscript file and copy the code.
