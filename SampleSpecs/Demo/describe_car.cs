using System;
using NSpec;

/*
public class Car
{
    car implementation would be here
{
*/

class describe_car : nspec
{
    void describe_fuel_requirements()
    {
        Car car = null;

        context["car has 10 gallon tank"] = () =>
        {
            before = () => car = new Car(tankSize: 10);

            context["tank is empty"] = () =>
            {
                before = () => car.FillTank(gallons: 0);

                specify = () => car.IsOnEmpty.should_be_true();
            };

            context["turning car on"] = () =>
            {
                act = () => car.TurnOn();

                context["car does not have gas"] = () =>
                {
                    specify = () => car.IsRunning.should_be_false();
                };

                context["car has gas"] = () =>
                {
                    before = () => car.FillTank(gallons: 10);

                    specify = () => car.IsRunning.should_be_true();
                };
            };

            context["10% of gas is left"] = () =>
            {
                before = () => car.FillTank(gallons: 10 * .1);

                specify = () => car.IsLowOnFuel.should_be_true();
            };

            context["less than 10% of gas is left"] = () =>
            {
                before = () => car.FillTank(gallons: 10 * .01);

                specify = () => car.IsLowOnFuel.should_be_true();
            };

            context["more than 10% of gas is left"] = () =>
            {
                before = () => car.FillTank(gallons: 9);

                specify = () => car.IsLowOnFuel.should_be_false();
            };
        };
    }

    void describe_compression_ratio()
    {
        Car car = null;

        //new Tuples<int, int, double> 
        //{ 
        //    { 7, 87, .28 }, 
        //    { 8, 92, .30 }, 
        //    { 9, 96, .32 }
        //}.Do(
        //(compressionRatio, octane, effeciency) =>
        //{
        //    context["car has compression ratio of {0} to 1".With(compressionRatio)] = () =>
        //    {
        //        before = () => car = new Car(compressionRatio);

        //        it["should have octane requirement of {0}".With(octane)] = () =>
        //        {
        //            car.OctaneRequirement.should_be(octane);
        //        };

        //        it["should have brake thermal efficiency of {0}%".With(effeciency * 100)] = () =>
        //        {
        //            car.BrakeThermalEffeciency.should_be(effeciency);
        //        };
        //    };
        //});
    }

    void when_driving_car()
    {
        it["should throw error if car isn't started"] = 
            expect<InvalidOperationException>(() => 
            {
                Car car = new Car(tankSize: 10, mpg: 1);
                car.Drive(10);
            });

        new[]
        {
            new { gasInTank = 10, mpg = 1,  miles = 10.0, expectedDistance = 10.0, 
                  gasLeft = 0.0, running = false, lowfuel = true, onEmpty = true },

            new { gasInTank = 10, mpg = 2,  miles = 5.0,  expectedDistance = 5.0,  
                  gasLeft = 7.5, running = true, lowfuel = false, onEmpty = false },

            new { gasInTank = 10, mpg = 10, miles = 10.0, expectedDistance = 10.0, 
                  gasLeft = 9.0, running = true, lowfuel = false, onEmpty = false }
        }.Do(example =>
        {
            context["with {0} gallon(s) of gas, mpg: {1}, driving: {2} miles"
                .With(example.gasInTank, example.mpg, example.miles)] = () =>
            {
                Car car = null;

                before = () =>
                {
                    car = new Car(example.gasInTank, example.mpg);
                    car.FillTank(example.gasInTank);
                };

                act = () => car.Drive(example.miles);

                it["should have made it {0} miles".With(example.expectedDistance)] = () =>
                {
                    car.Odometer.should_be(example.expectedDistance);
                };

                it["should have {0} gallons left in tank".With(example.gasLeft)] = () =>
                {
                    car.GasInTank.should_be(example.gasLeft);
                };

                it["should {0} running".With(example.running ? "be" : "not be")] = () =>
                {
                    car.IsRunning.should_be(example.running);
                };

                it["should {0} low fuel".With(example.lowfuel ? "have" : "not have")] = () =>
                {
                    car.IsLowOnFuel.should_be(example.lowfuel);
                };
            };
        });
    }
}
