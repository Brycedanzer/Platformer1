using Godot;

// The Bouncer moves up and down
public partial class Bouncer : Enemy
{
	private int direction = -1;

	public override void Move(double delta)
	{
		Vector2 position = Position;
		position.Y += direction * speed * (float)delta;
		Position = position;
	}

	private void OnTimerTimeout()
	{
		direction *= -1;
	}
}
