using System;
using Google.OrTools.LinearSolver;

namespace OR_Tools_KBanach
{
    class Program
    {
        static void Main()
        {
            Solver solver = Solver.CreateSolver("MINWD_Bryla", "GLOP_LINEAR_PROGRAMMING");

            //Wartości zmiennych do pkt. 1 zadania
            //Variable x = solver.MakeNumVar(0.0, double.PositiveInfinity, "x");
            //Variable y = solver.MakeNumVar(0.0, double.PositiveInfinity, "y");

            //Wartości zmiennych do pkt. 2 zadania
            Variable x = solver.MakeNumVar(0.0, 5000.0, "x");
            Variable y = solver.MakeNumVar(0.0, 4000.0, "y");

            Console.WriteLine("Liczba zmiennych = " + solver.NumVariables());

            Constraint ct = solver.MakeConstraint(0.0, 12000.0, "ct");
            ct.SetCoefficient(x, 1);
            ct.SetCoefficient(y, 2);

            Constraint ct1 = solver.MakeConstraint(0.0, 56000.0, "ct1");
            ct1.SetCoefficient(x, 7);
            ct1.SetCoefficient(y, 4);

            Console.WriteLine("Liczba ograniczeń = " + solver.NumConstraints());

            Objective objective = solver.Objective();
            objective.SetCoefficient(x, 2);
            objective.SetCoefficient(y, 4);
            objective.SetMaximization();

            solver.Solve();

            Console.WriteLine("Rozwiązanie:");
            Console.WriteLine("Wartość funkcji celu = " + solver.Objective().Value());
            Console.WriteLine("Ilość wyrobu pierwszego = " + x.SolutionValue());
            Console.WriteLine("Ilość wyrobu drugiego = " + y.SolutionValue());
        }
    }
}
