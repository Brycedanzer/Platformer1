using Godot;

// The Snail walks left and right
public partial class Snail : Enemy
{
	private int direction = -1;

	private AnimatedSprite2D animatedSprite2D;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void Move(double delta)
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
}
