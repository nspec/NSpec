using System;
using NSpec;
using SampleSpecs.Model;

class describe_TicTacToeGame : nspec
{
    void before_each()
    {
        game = new TicTacToGame();

        players = new[] { "x", "o" };
    }

    void when_players_try_to_take_the_same_square()
    {
        it["should throw exception"] = expect<InvalidOperationException>(() =>
            players.Do(player => game.Play(player, 0, 0))
        );
    }

    protected TicTacToGame game;
    protected string[] players;
}
