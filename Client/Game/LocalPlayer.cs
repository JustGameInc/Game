using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Client.Game;

public class LocalPlayer : Player
{
	public Texture2D Sprite { get; set;}
	public SpriteFont Font { get; set; }
	public Vector2 Size { get; set;}
	public Vector2 Velocity { get; set; } = Vector2.Zero;
	
	public Vector2 Speed { get; set; } = new Vector2(10f, 10f);
	public Vector2 MaxSpeed { get; set; } = new Vector2(3000f, 3000f);
	public Vector2 MaxWalkSpeed { get; set; } = new Vector2(50f, 50f);	

	public LocalPlayer(Microsoft.Xna.Framework.Game game) : base(game)
	{
		DrawOrder = 1;
		Size = new Vector2(40, 60);
	}

	public override void Update(GameTime gameTime)
	{
		var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.S))
			Velocity += Velocity.Y <= MaxWalkSpeed.Y ? Vector2.UnitY * Speed.Y * delta : Vector2.Zero;
		if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.W))
			Velocity -= Velocity.Y <= MaxWalkSpeed.Y ? Vector2.UnitY * Speed.Y * delta : Vector2.Zero;
		if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.D))
			Velocity += Velocity.X <= MaxWalkSpeed.X ? Vector2.UnitX * Speed.X * delta : Vector2.Zero;
		if (GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A))
			Velocity -= Velocity.X <= MaxWalkSpeed.X ? Vector2.UnitX * Speed.X * delta : Vector2.Zero;
		

		Velocity -= Velocity * 2f * delta;

		var vel = Velocity;
		vel.X = MathHelper.Clamp(vel.X, -MaxSpeed.X, MaxSpeed.X);
		vel.Y = MathHelper.Clamp(vel.Y, -MaxSpeed.Y, MaxSpeed.Y);
		Velocity = vel;

		Position += Velocity;

		base.Update(gameTime);
	}

	protected override void LoadContent()
	{
		var content = Game.Content;
		Sprite = content.Load<Texture2D>("Textures/Player");
		Font = content.Load<SpriteFont>("Fonts/Font");
		base.LoadContent();
	}

	public override void Draw(GameTime gameTime)
	{
		spriteBatch.Begin(transformMatrix: Game1.GetCamera(Game).Transform);
		spriteBatch.Draw(Sprite, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
		spriteBatch.DrawString(Font, "Vazgen", new Vector2(
			(int)Position.X + Size.X / 2 - Font.MeasureString("Vazgen").X * 0.5f, 
			(int)Position.Y - Font.MeasureString("Vazgen").Y), Color.White
		);
		spriteBatch.End();
		
		spriteBatch.Begin();
		spriteBatch.DrawString(Font, Vector2.Floor(Position).ToString(), new Vector2(0, 0), Color.White);
		spriteBatch.End();


		base.Draw(gameTime);
	}
}