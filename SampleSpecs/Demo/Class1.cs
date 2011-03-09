using NSpec.Extensions;
using NSpec.Interpreter.Indexer;
using NSpec;

namespace SampleSpecs.Demo
{
    public class after_player_one_takes_a_square_ : spec
    {
        private string places;

        public void player_two_cannot_take_it()
        {
            before.each = () =>
            {
                var player1 = "";
                var player2 = "";
                places = "A";

                player1 = places.Remove('A');
            };

            specify(() => places.should_be_empty());

        }
    }
}
