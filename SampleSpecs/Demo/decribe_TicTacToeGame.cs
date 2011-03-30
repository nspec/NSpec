using System;
using NSpec;
using SampleSpecs.Model;

class describe_TicTacToeGame : nspec
{
    public describe_TicTacToeGame()
    {
        players = new[] { "x", "o" };
    }

    void when_players_try_to_take_the_same_square()
    {
        before = () => game = new TicTacToGame();

        it["should throw exception"] = expect<InvalidOperationException>(() =>
            players.Do(player => game.Play(player, 0, 0))
        );
    }

    void describe_a_finished_game()
    {
        before = () => game = new TicTacToGame();

        var indexes = new[] { 0, 1, 2 };

        indexes.Do(index => players.Do(player =>
        {
            context["3 {0}'s in column {1}".With(player, index)] = () =>
            {
                before = () => indexes.Do(column => game.Play(player, index, column));

                specify = () => game.Done.should_be_true();

                it["winner should be {0}".With(player)] = () => game.Winner.should_be(player);
            };

            context["3 {0}'s in row {1}".With(player, index)] = () =>
            {
                before = () => indexes.Do(row => game.Play(player, row, index));

                specify = () => game.Done.should_be_true();

                it["winner should be {0}".With(player)] = () => game.Winner.should_be(player);
            };
        }));

        players.Do(player =>
        {
            context["3 {0}'s left to right".With(player)] = () =>
            {
                before = () =>
                {
                    game.Play(player, 0, 0);
                    game.Play(player, 1, 1);
                    game.Play(player, 2, 2);
                };

                specify = () => game.Done.should_be_true();

                it["winner should be {0}".With(player)] = () => game.Winner.should_be(player);
            };

            context["3 {0}'s right to left".With(player)] = () =>
            {
                before = () =>
                {
                    game.Play(player, 2, 0);
                    game.Play(player, 1, 1);
                    game.Play(player, 0, 2);
                };

                specify = () => game.Done.should_be_true();

                it["winner should be {0}".With(player)] = () => game.Winner.should_be(player);
            };
        });

        context["all squares taken with no 3 in a row"] = () =>
        {
            before = () =>
                indexes.Do(row =>
                    indexes.Do(column =>
                        game.Play(AlternateUser(), row, column)
                    )
                );

            specify = () => game.Done.should_be_true();
            specify = () => game.Draw.should_be_true();
        };

        //this context has problems. it repeats previous specs 
        //and doesn't add anything. it does highlight the fact
        //that the game should be over after the first three plays
        //we should throw this one away.
        context["all squares taken with 3 x's in a row"] = () =>
        {
            before = () =>
            {
                game.Play("x", 0, 0);
                game.Play("x", 0, 1);
                game.Play("x", 0, 2);

                game.Play("o", 1, 0);
                game.Play("x", 1, 1);
                game.Play("o", 1, 2);

                game.Play("o", 2, 0);
                game.Play("x", 2, 1);
                game.Play("o", 2, 2);
            };

            specify = () => game.Winner.should_be("x");
            specify = () => game.Draw.should_be_false();
        };
    }

    private string AlternateUser()
    {
        if (user == "") return "x";

        return user = (user == "x") ? "y" : "x";
    }

    TicTacToGame game;
    private string user;
    private string[] players;
}
