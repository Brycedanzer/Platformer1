using Godot;

// Enemy is the PARENT class.
// Snail, Bouncer and Spinner are its CHILDREN. They all get everything in here.
// "abstract" means you can't make a plain Enemy, only one of its children.
public abstract partial class Enemy : Area2D
{
	[Signal]
	public delegate void PlayerDiedEventHandler(Node2D body);

	// Every enemy has a speed
	protected float speed = 100;

	// Every child has to write its own Move method
	public abstract void Move(double delta);

	public override void _Process(double delta)
	{
		Move(delta);
	}

	// Every enemy kills the player when it touches him
	private void OnBodyEntered(Node2D body)
	{
		if (body is Player player && player.Alive)
		{
			EmitSignal(SignalName.PlayerDied, body);
		}
	}
}
