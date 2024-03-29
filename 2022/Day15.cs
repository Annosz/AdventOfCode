﻿using System.Globalization;

namespace _2022;

public static class Day15
{
    private const int LineNumber = 2000000;

    private static readonly HashSet<Coordinate> FilledCoordinates = new();
    private static readonly Dictionary<Coordinate, char> SymbolMap = new();

    public static string Solve()
    {
        List<(Coordinate, int)> sensors = new();
        HashSet<Coordinate> beacons = new();

        foreach (var line in File.ReadLines(@".\Input\Day15.txt"))
        {
            string lineWorkingCopy = line.Remove(0, "Sensor at x=".Length);
            string[] lineSplit = lineWorkingCopy.Split(',', 2);
            int sensorX = int.Parse(lineSplit[0]);
            lineWorkingCopy = lineSplit[1];

            lineWorkingCopy = lineWorkingCopy.Remove(0, " y=".Length);
            lineSplit = lineWorkingCopy.Split(':', 2);
            int sensorY = int.Parse(lineSplit[0]);
            lineWorkingCopy = lineSplit[1];


            lineWorkingCopy = lineWorkingCopy.Remove(0, " closest beacon is at x=".Length);
            lineSplit = lineWorkingCopy.Split(',', 2);
            int beaconX = int.Parse(lineSplit[0]);
            lineWorkingCopy = lineSplit[1];

            lineWorkingCopy = lineWorkingCopy.Remove(0, " y=".Length);
            int beaconY = int.Parse(lineWorkingCopy);

            int manhattanDistance = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);
            sensors.Add((new Coordinate(sensorX, sensorY), manhattanDistance));
            beacons.Add(new Coordinate(beaconX, beaconY));
        }

        // Part 1
        foreach (var coordinate in beacons)
        {
            FillOnMap(coordinate, 'B');
        }

        foreach (var (coordinate, _) in sensors)
        {
            FillOnMap(coordinate, 'S');
        }

        foreach ((var coordinate, int manhattan) in sensors)
        {
            for (int i = coordinate.X - manhattan; i <= coordinate.X + manhattan; i++)
            {
                if (Math.Abs(coordinate.X - i) + Math.Abs(coordinate.Y - LineNumber) <= manhattan)
                {
                    FillOnMap(new Coordinate(i, LineNumber), '#');
                }
            }
        }

        var part1Solution = FilledCoordinates.Count(c => c.Y == 2000000 && SymbolMap.ContainsKey(c) && SymbolMap[c] == '#').ToString();

        // Part 2
        List<Coordinate> pointsToCheck = new();
        foreach ((var coordinate, int manhattan) in sensors)
        {
            for (int i = 0; i <= manhattan; i++)
            {
                pointsToCheck.Add(new Coordinate(coordinate.X + (i + 1), coordinate.Y + (manhattan - i)));
                pointsToCheck.Add(new Coordinate(coordinate.X - (i + 1), coordinate.Y + (manhattan - i)));
                pointsToCheck.Add(new Coordinate(coordinate.X + (i + 1), coordinate.Y - (manhattan - i)));
                pointsToCheck.Add(new Coordinate(coordinate.X - (i + 1), coordinate.Y - (manhattan - i)));
            }

            pointsToCheck.Add(new Coordinate(coordinate.X, coordinate.Y - manhattan - 1));
            pointsToCheck.Add(new Coordinate(coordinate.X, coordinate.Y + manhattan + 1));
        }

        foreach (var pointCoordinate in pointsToCheck)
        {
            if (pointCoordinate.X < 0 || pointCoordinate.Y < 0 || pointCoordinate.X > 4000000 || pointCoordinate.Y > 4000000)
                continue;

            bool solution = true;
            foreach ((var sensorCoordinate, int manhattan) in sensors)
            {
                if (Math.Abs(sensorCoordinate.X - pointCoordinate.X) + Math.Abs(sensorCoordinate.Y - pointCoordinate.Y) <= manhattan)
                {
                    solution = false;
                }
            }

            if (solution)
                return (pointCoordinate.X * (decimal)4000000 + pointCoordinate.Y).ToString(CultureInfo.InvariantCulture);
        }

        return "?";
    }

    private static void FillOnMap(Coordinate coordinate, char symbol)
    {
        if (SymbolMap.ContainsKey(coordinate))
            return;

        FilledCoordinates.Add(coordinate);
        SymbolMap.Add(coordinate, symbol);
    }

    private record Coordinate(int X, int Y);
}