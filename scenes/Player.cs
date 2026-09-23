using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -850.0f;

	private AnimatedSprite2D animatedSprite2D;
	private AudioStreamPlayer2D jumpSound;
	private AudioStreamPlayer2D deathSound;

	public bool Alive = true;
	public bool CanMove = true;

	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		jumpSound = GetNode<AudioStreamPlayer2D>("JumpSound");
		deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Alive)
		{
			return;
		}

		Vector2 velocity = Velocity;

		// Add animation
		if (velocity.X > 1 || velocity.X < -1)
		{
			animatedSprite2D.Animation = "running";
		}
		else
		{
			animatedSprite2D.Animation = "idle";
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
			animatedSprite2D.Animation = "jumping";
		}

		float direction = 0;

		if (CanMove)
		{
			// Handle Jump.
			if (Input.IsActionJustPressed("jump") && IsOnFloor())
			{
				velocity.Y = JumpVelocity;
				jumpSound.Play();
			}

			// Get the input direction (A/D via the "left"/"right" actions)
			// and handle the movement/deceleration.
			direction = Input.GetAxis("left", "right");
			if (direction != 0)
			{
				velocity.X = direction * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}
		}

		Velocity = velocity;
		MoveAndSlide();

		if (direction == 1)
		{
			animatedSprite2D.FlipH = false;
		}
		else if (direction == -1)
		{
			animatedSprite2D.FlipH = true;
		}
	}

	public void Die()
	{
		deathSound.Play();
		animatedSprite2D.Animation = "dying";
		Alive = false;
	}
}
