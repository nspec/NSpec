using System;
using NSpec;


public class Car
{
    public Car(int compression = 0)
        : this(0, 0, compression)
    {

    }

    public Car(double tankSize = 0, int mpg = 0, int compression = 0)
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
        if (IsRunning == false)
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