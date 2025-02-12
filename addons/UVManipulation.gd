# This node was created by Foyezes
# x.com/Foyezes
# bsky.app/profile/foyezes.bsky.social

@tool
extends VisualShaderNodeCustom
class_name VisualShaderNodeUVManip

func _get_name():
	return "UVManipulation"

func _get_category():
	return "UV"

func _get_description():
	return "Rotate, Scale and Offset UV"

func _get_output_port_count():
	return 1
	
func _get_input_port_count():
	return 5

func _get_return_icon_type():
	return VisualShaderNode.PORT_TYPE_VECTOR_2D

func _get_output_port_name(port):
	match port:
		0:
			return "uv"
			
func _get_output_port_type(port):
	return VisualShaderNode.PORT_TYPE_VECTOR_2D

func _get_input_port_name(port):
	match port:
		0:
			return "UV"
		1:
			return "Scale"
		2:
			return "Rotation"
		3:
			return "Pivot"
		4:
			return "Offset"
			
func _get_input_port_type(port):
	match port:
		0:
			return VisualShaderNode.PORT_TYPE_VECTOR_2D
		1:
			return VisualShaderNode.PORT_TYPE_SCALAR
		2:
			return VisualShaderNode.PORT_TYPE_SCALAR
		3:
			return VisualShaderNode.PORT_TYPE_VECTOR_2D
		4:
			return VisualShaderNode.PORT_TYPE_VECTOR_2D
			
func _get_input_port_default_value(port):
	match port:
		1:
			return 1.0;
		2:
			return 0.0;
		3:
			return Vector2(0.0, 0.0);
		4:
			return Vector2(0.0, 0.0);

func _get_global_code(mode):
	return """
	
	vec2 rotateUV(vec2 uv_in, float angle) {
		float angle_rad = radians(angle);
		mat2 rotate = mat2(
			vec2(cos(angle_rad), -sin(angle_rad)),
			vec2(sin(angle_rad), cos(angle_rad))
		);
		return rotate * uv_in;
	}

	vec2 UVManip(vec2 uv, float scale_placeholder, float rotation_placeholder, vec2 pivot_placeholder, vec2 offset_placeholder) {
		vec2 pivot_internal = clamp(pivot_placeholder, 0.0, 1.0);
		uv -= pivot_internal;
		uv *= scale_placeholder;
		uv = rotateUV(uv, rotation_placeholder);
		uv += pivot_internal;
		uv += offset_placeholder;
		return uv;
	}
	
	"""

func _get_code(input_vars, output_vars, mode, type):
	return output_vars[0] + "=UVManip(%s, %s, %s, %s, %s);" % [input_vars[0], input_vars[1], input_vars[2], input_vars[3], input_vars[4]]
