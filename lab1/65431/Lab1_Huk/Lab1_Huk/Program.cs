using Google.OrTools.LinearSolver;
using System;

namespace Lab1_Huk
{
    class Program
    {
        static void Main(string[] args)
        {

            Solver solver = Solver.CreateSolver("SimpleMipProgram", "CBC_MIXED_INTEGER_PROGRAMMING");

            Variable p1 = solver.MakeNumVar(0.0, double.PositiveInfinity, "p1");
            Variable p2 = solver.MakeNumVar(0.0, double.PositiveInfinity, "p2");

            Console.WriteLine("Number of variables = " + solver.NumVariables());

            Constraint ct = solver.MakeConstraint(120.0, double.PositiveInfinity, "ct");
            ct.SetCoefficient(p1, 6);
            ct.SetCoefficient(p2, 3);

            //ZADANIE NR 2
            //Constraint ct = solver.MakeConstraint(120.0, 240.0, "ct");
            //ct.SetCoefficient(x, 6);
            //ct.SetCoefficient(y, 3);

            Constraint ct2 = solver.MakeConstraint(60.0, double.PositiveInfinity, "ct2");
            ct2.SetCoefficient(p1, 1);
            ct2.SetCoefficient(p2, 3);

            Constraint ct3 = solver.MakeConstraint(36.0, double.PositiveInfinity, "ct3");
            ct3.SetCoefficient(p1, 9);
            ct3.SetCoefficient(p2, 1);

            Constraint ct4 = solver.MakeConstraint(180.0, double.PositiveInfinity, "ct4");
            ct4.SetCoefficient(p1, 6);
            ct4.SetCoefficient(p2, 6);

            Console.WriteLine("Liczba zmiennych = " + solver.NumConstraints());

            Objective objective = solver.Objective();
            objective.SetCoefficient(p1, 1.2);
            objective.SetCoefficient(p2, 1.8);
            objective.SetMinimization();

            solver.Solve();

            Console.WriteLine("Rozwiązanie:");
            Console.WriteLine("Wynik = " + solver.Objective().Value());
            Console.WriteLine("p1 = " + p1.SolutionValue());
            Console.WriteLine("p2 = " + p2.SolutionValue());
        }
    }
}
