using Godot;
using System;

public partial class ButtonMethods : CheckButton
{
	public void ToggleOnTop(bool status)
	{
		DisplayServer.WindowSetFlag(DisplayServer.WindowFlags.AlwaysOnTop, status);
	}
}
