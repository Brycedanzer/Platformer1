using Godot;

public partial class StartMenu : Control
{
	public override void _Ready()
	{
		// Select the Play button so a controller can press it
		GetNode<Button>("PlayButton").GrabFocus();
	}

	// Runs when the Play button is pressed
	private void OnPlayButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main.tscn");
	}
}
