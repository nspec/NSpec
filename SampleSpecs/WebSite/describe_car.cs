using System;
using NSpec;
using NSpec.Assertions;
using NSpec.Extensions;
using System.Collections.Generic;

namespace SampleSpecs.WebSite
{
    public class Car
    {
        public Car(double tankSize = 0, int compression = 0, int mpg = 0)
        {
            TankSize = tankSize;
            Mpg = mpg;

            switch (compression)
            {
                case 7:
                    OctaneRequirement = 87;
                    BrakeThermalEffeciency = .28;
                    break;
                case 8:
                    OctaneRequirement = 92;
                    BrakeThermalEffeciency = .3;
                    break;
                case 9:
                    OctaneRequirement = 96;
                    BrakeThermalEffeciency = .32;
                    break;
            }
        }

        public double TankSize { get; private set; }
        public bool IsLowOnFuel { get; private set; }
        public bool IsOnEmpty { get; private set; }
        public bool IsRunning { get; private set; }
        public int OctaneRequirement { get; private set; }
        public double BrakeThermalEffeciency { get; private set; }
        public double GasInTank { get; private set; }
        public double Odometer { get; private set; }
        public int Mpg { get; private set; }

        public void TurnOn()
        {
            UpdateState();
        }

        public void Drive(double miles)
        {
            if(IsRunning == false)
            {
                throw new InvalidOperationException("Car is not running.");
            }

            double milesPossible = Mpg * GasInTank;

            if (milesPossible > miles)
            {
                Odometer = miles;
                GasInTank = GasInTank - (GasInTank * (miles / milesPossible));
            }
            else
            {
                Odometer = milesPossible;
                GasInTank = 0;
            }

            UpdateState();
        }

        public void FillTank(double gallons)
        {
            GasInTank = gallons;

            UpdateState();
        }

        public void UpdateState()
        {
            if ((TankSize * .1) >= GasInTank) IsLowOnFuel = true;

            if (GasInTank == 0) IsOnEmpty = true;

            IsRunning = GasInTank != 0;
        }
    }

    class describe_car : spec
    {
        public void describe_fuel_requirements()
        {
            Car car = null;

            context["car has 10 gallon tank"] = () =>
            {
                before = () => car = new Car(tankSize: 10);

                context["tank is empty"] = () =>
                {
                    before = () => car.FillTank(gallons: 0);

                    it += () => car.IsOnEmpty.should_be_true();
                };

                context["turning car on"] = () =>
                {
                    act = () => car.TurnOn();

                    context["car does not have gas"] = () =>
                    {
                        before = () => car.FillTank(gallons: 0);

                        it += () => car.IsRunning.should_be_false();
                    };

                    context["car has gas"] = () =>
                    {
                        before = () => car.FillTank(gallons: 10);

                        it += () => car.IsRunning.should_be_true();
                    };
                };

                context["10% of gas is left"] = () =>
                {
                    before = () => car.FillTank(gallons: 10 * .1);

                    it += () => car.IsLowOnFuel.should_be_true();
                };

                context["less than 10% of gas is left"] = () =>
                {
                    before = () => car.FillTank(gallons: 10 * .01);

                    it += () => car.IsLowOnFuel.should_be_true();
                };

                context["more than 10% of gas is left"] = () =>
                {
                    before = () => car.FillTank(gallons: 9);

                    it += () => car.IsLowOnFuel.should_be_false();
                };
            };
        }

        public void describe_compression_ratio()
        {
            Car car = null;

            new Tuples<int, int, double> 
            { 
                { 7, 87, .28 }, 
                { 8, 92, .30 }, 
                { 9, 96, .32 }
            }.Do((compression, octane, effeciency) =>
            {
                context["car has compression ratio of {0} to 1".With(compression)] = () =>
                {
                    before = () => car = new Car(compression: compression);

                    it["should have octane requirement of {0}".With(octane)] = () =>
                    {
                        car.OctaneRequirement.should_be(octane);
                    };

                    it["should have brake thermal efficiency of {0}%".With(effeciency * 100)] = () =>
                    {
                        car.BrakeThermalEffeciency.should_be(effeciency);
                    };
                };
            });
        }

        public void when_driving_car()
        {
            new List<dynamic>()
            {
                new { gasInTank = 10, mpg = 1,  miles = 10.0, expectedDistance = 10.0, gasLeft = 0.0, running = false, lowfuel = true, onEmpty = true },
                new { gasInTank = 10, mpg = 2,  miles = 5.0,  expectedDistance = 5.0,  gasLeft = 7.5, running = true, lowfuel = false, onEmpty = false },
                new { gasInTank = 10, mpg = 10, miles = 10.0, expectedDistance = 10.0, gasLeft = 9.0, running = true, lowfuel = false, onEmpty = false }
            }.Do(
            (d) =>
            {
                int gasInTank = d.gasInTank;
                int mpg = d.mpg;
                double miles = d.miles;
                double expectedDistance = d.expectedDistance;
                double gasLeft = d.gasLeft;
                bool isRunning = d.running;
                bool isLowFuel = d.lowfuel;
                bool onEmpty = d.onEmpty;

                context["a car with {0} gallon(s) of gas, that gets {1} mile(s) per gallon, attempting to drive {2} miles".With(gasInTank, mpg, miles)] = () =>
                {
                    Car car = null;

                    before = () =>
                    {
                        car = new Car(tankSize: gasInTank, mpg: mpg);
                        car.FillTank(gasInTank);
                    };

                    act = () => car.Drive(miles);

                    it["should have made it {0} miles".With(expectedDistance)] = () =>
                    {
                        car.Odometer.should_be(expectedDistance);
                    };

                    it["should have {0} gallons left in tank".With(gasLeft)] = () =>
                    {
                        car.GasInTank.should_be(gasLeft);
                    };

                    it["should {0} running".With(isRunning ? "be" : "not be")] = () =>
                    {
                        car.IsRunning.should_be(isRunning);
                    };

                    it["should {0} low fuel".With(isLowFuel ? "have" : "not have")] = () => 
                    {
                    	car.IsLowOnFuel.should_be(isLowFuel);
                    };
                };
            });
        }
    }
}
