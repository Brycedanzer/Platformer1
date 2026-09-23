using Godot;

public partial class Snail : Area2D
{
	[Signal]
	public delegate void PlayerDiedEventHandler(Node2D body);

	private float speed = 100;
	private int direction = -1;

	private AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _Process(double delta)
	{
		Vector2 position = Position;
		position.X += direction * speed * (float)delta;
		Position = position;
	}

	private void OnTimerTimeout()
	{
		direction *= -1;
		animatedSprite2D.FlipH = !animatedSprite2D.FlipH;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body.Name == "Player" && ((Player)body).Alive)
		{
			EmitSignal(SignalName.PlayerDied, body);
		}
	}
}
