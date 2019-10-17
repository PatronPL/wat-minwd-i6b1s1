using System;
using Google.OrTools;
using Google.OrTools;
using Google.OrTools.LinearSolver;

namespace MINDW_ZAD7_TOMASIK
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver solver = Solver.CreateSolver("MINDW_ZAD7_TOMASIK", "CBC_MIXED_INTEGER_PROGRAMMING");

            Variable lP1 = solver.MakeNumVar(0, double.PositiveInfinity, "lP1");
            Variable lP2 = solver.MakeNumVar(0, double.PositiveInfinity, "lP2");
            Variable lP3 = solver.MakeNumVar(0, double.PositiveInfinity, "lP3");
            Variable lP4 = solver.MakeNumVar(0, double.PositiveInfinity, "lP4");

            Console.WriteLine("Liczba zmiennych: " + solver.NumVariables());

            Constraint ct = solver.MakeConstraint(1200, double.PositiveInfinity, "A");
            ct.SetCoefficient(lP1, 0.8);
            ct.SetCoefficient(lP2, 2.4);
            ct.SetCoefficient(lP3, 0.9);
            ct.SetCoefficient(lP4, 0.4);

            Constraint ct1 = solver.MakeConstraint(600, double.PositiveInfinity, "B");
            ct1.SetCoefficient(lP1, 0.6);
            ct1.SetCoefficient(lP2, 0.6);
            ct1.SetCoefficient(lP3, 0.3);
            ct1.SetCoefficient(lP4, 0.3);

            Console.WriteLine("Liczba ograniczeń: " + solver.NumConstraints());

            Objective objective = solver.Objective();
            objective.SetCoefficient(lP1, 9.6);
            objective.SetCoefficient(lP2, 14.4);
            objective.SetCoefficient(lP3, 10.8);
            objective.SetCoefficient(lP4, 7.2);

            objective.SetMinimization();

            solver.Solve();

            Console.WriteLine("Rozwiązanie:");
            Console.WriteLine("Wartość funkcji celu = " + solver.Objective().Value());
            Console.WriteLine("Ilość paszy 1: " + lP1.SolutionValue());
            Console.WriteLine("Ilość paszy 2: " + lP2.SolutionValue());
            Console.WriteLine("Ilość paszy 3: " + lP3.SolutionValue());
            Console.WriteLine("Ilość paszy 4: " + lP4.SolutionValue());



        }
    }
}
