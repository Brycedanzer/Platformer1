using Godot;
using System.Threading.Tasks;

public partial class Main : Node2D
{
	private int level = 1;
	private int score = 0;

	private Node currentLevelRoot = null;
	private Label scoreLabel;
	private ColorRect fade;

	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("HUD/ScorePanel/ScoreLabel");
		fade = GetNode<ColorRect>("HUD/Fade");

		Color modulate = fade.Modulate;
		modulate.A = 1.0f;
		fade.Modulate = modulate;

		// Set up the level
		currentLevelRoot = GetNode("LevelRoot");
		LoadLevel(level, true, false);
	}

	// ---------- LEVEL MANAGEMENT ----------
	private async void LoadLevel(int levelNumber, bool firstLoad, bool resetScore)
	{
		// Fade out
		if (!firstLoad)
		{
			await Fade(1.0f);
		}

		if (resetScore)
		{
			score = 0;
			scoreLabel.Text = "SCORE: 0";
		}

		// Change level
		if (currentLevelRoot != null)
		{
			currentLevelRoot.QueueFree();
		}

		string levelPath = string.Format("res://scenes/levels/level{0}.tscn", levelNumber);
		currentLevelRoot = GD.Load<PackedScene>(levelPath).Instantiate();
		AddChild(currentLevelRoot);
		currentLevelRoot.Name = "LevelRoot";
		SetupLevel(currentLevelRoot);

		// Fade in
		await Fade(0.0f);
	}

	private void SetupLevel(Node levelRoot)
	{
		// Connect exit
		Node exit = levelRoot.GetNodeOrNull("Exit");
		if (exit != null)
		{
			exit.Connect("body_entered", Callable.From<Node2D>(OnExitBodyEntered));
		}

		// Connect apples
		Node apples = levelRoot.GetNodeOrNull("Apples");
		if (apples != null)
		{
			foreach (Node apple in apples.GetChildren())
			{
				apple.Connect("Collected", Callable.From(IncreaseScore));
			}
		}

		// Connect enemies
		Node enemies = levelRoot.GetNodeOrNull("Enemies");
		if (enemies != null)
		{
			foreach (Node enemy in enemies.GetChildren())
			{
				enemy.Connect("PlayerDied", Callable.From<Node2D>(OnPlayerDied));
			}
		}
	}

	// ---------- SIGNAL HANDLERS ----------
	private void OnPlayerDied(Node2D body)
	{
		((Player)body).Die();
		LoadLevel(level, false, true);
	}

	private void OnExitBodyEntered(Node2D body)
	{
		if (body.Name == "Player")
		{
			level += 1;
			((Player)body).CanMove = false;
			LoadLevel(level, false, false);
		}
	}

	// ---------- SCORE ----------
	private void IncreaseScore()
	{
		score += 1;
		scoreLabel.Text = $"SCORE: {score}";
	}

	// ---------- FADE ----------
	private async Task Fade(float toAlpha)
	{
		Tween tween = CreateTween();
		tween.TweenProperty(fade, "modulate:a", toAlpha, 1.5f);
		await ToSignal(tween, Tween.SignalName.Finished);
	}
}
