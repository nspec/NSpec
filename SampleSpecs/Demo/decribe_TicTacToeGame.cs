using System;
using NSpec;
using SampleSpecs.Model;

class describe_TicTacToeGame : nspec
{
    TicTacToGame ticTacToGame = null;

    void when_players_try_to_take_the_same_square()
    {
        before = () => ticTacToGame = new TicTacToGame();

        it["should throw exception"] = expect<InvalidOperationException>(() =>
        {
            ticTacToGame.Place("x", 0, 0);
            ticTacToGame.Place("o", 0, 0);
        });
    }

    void describe_a_finished_game()
    {
        before = () => ticTacToGame = new TicTacToGame();

        new[] { 0, 1, 2 }.Do((rowColumn) =>
        {
            new[] { "x", "o" }.Do((player) =>
            {
                context["3 {0}'s in column {1}".With(player, rowColumn)] = () =>
                {
                    before = () =>
                    {
                        ticTacToGame.Place(player, rowColumn, 0);
                        ticTacToGame.Place(player, rowColumn, 1);
                        ticTacToGame.Place(player, rowColumn, 2);
                    };

                    specify = () => ticTacToGame.Done.should_be_true();

                    it["winner should be {0}".With(player)] = () => ticTacToGame.Winner.should_be(player);
                };

                context["3 {0}'s in row {1}".With(player, rowColumn)] = () =>
                {
                    before = () =>
                    {
                        ticTacToGame.Place(player, 0, rowColumn);
                        ticTacToGame.Place(player, 1, rowColumn);
                        ticTacToGame.Place(player, 2, rowColumn);
                    };

                    specify = () => ticTacToGame.Done.should_be_true();

                    it["winner should be {0}".With(player)] = () => ticTacToGame.Winner.should_be(player);
                };
            });
        });


        new[] { "x", "o" }.Do((player) =>
        {
            context["3 {0}'s left to right".With(player)] = () =>
            {
                before = () =>
                {
                    ticTacToGame.Place(player, 0, 0);
                    ticTacToGame.Place(player, 1, 1);
                    ticTacToGame.Place(player, 2, 2);
                };

                specify = () => ticTacToGame.Done.should_be_true();

                it["winner should be {0}".With(player)] = () => ticTacToGame.Winner.should_be(player);
            };

            context["3 {0}'s right to left".With(player)] = () =>
            {
                before = () =>
                {
                    ticTacToGame.Place(player, 2, 0);
                    ticTacToGame.Place(player, 1, 1);
                    ticTacToGame.Place(player, 0, 2);
                };

                specify = () => ticTacToGame.Done.should_be_true();

                it["winner should be {0}".With(player)] = () => ticTacToGame.Winner.should_be(player);
            };
        });

        context["all squares taken with no 3 in a row"] = () =>
        {
            before = () =>
            {
                ticTacToGame.Place("x", 0, 0);
                ticTacToGame.Place("o", 0, 1);
                ticTacToGame.Place("x", 0, 2);

                ticTacToGame.Place("o", 1, 0);
                ticTacToGame.Place("x", 1, 1);
                ticTacToGame.Place("o", 1, 2);

                ticTacToGame.Place("o", 2, 0);
                ticTacToGame.Place("x", 2, 1);
                ticTacToGame.Place("o", 2, 2);
            };

            specify = () => ticTacToGame.Done.should_be_true();
            specify = () => ticTacToGame.Draw.should_be_true();
        };

        context["all squares taken with 3 x's in a row"] = () =>
        {
            before = () =>
            {
                ticTacToGame.Place("x", 0, 0);
                ticTacToGame.Place("x", 0, 1);
                ticTacToGame.Place("x", 0, 2);

                ticTacToGame.Place("o", 1, 0);
                ticTacToGame.Place("x", 1, 1);
                ticTacToGame.Place("o", 1, 2);

                ticTacToGame.Place("o", 2, 0);
                ticTacToGame.Place("x", 2, 1);
                ticTacToGame.Place("o", 2, 2);
            };

            specify = () => ticTacToGame.Winner.should_be("x");
            specify = () => ticTacToGame.Draw.should_be_false();
        };
    }
}
