
using Google.OrTools.LinearSolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie_optymalizacyjne
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the linear solver with the GLOP backend.
            Solver solver = Solver.CreateSolver("SimpleLpProgram", "GLOP_LINEAR_PROGRAMMING");

            // Create the variables x and y.
            Variable x = solver.MakeNumVar(0.0, Double.MaxValue, "x");
            Variable y = solver.MakeNumVar(0.0, Double.MaxValue, "y");

            Console.WriteLine("Number of variables = " + solver.NumVariables());

            // Create a linear constraint, 0 <= x + y <= 2.
            Constraint ct = solver.MakeConstraint(0.0, 120.0, "ct");
            ct.SetCoefficient(x, 6);
            ct.SetCoefficient(y, 3);

            Constraint ct2 = solver.MakeConstraint(0.0, 60.0, "ct2");
            ct2.SetCoefficient(x, 1);
            ct2.SetCoefficient(y, 3);

            Constraint ct3 = solver.MakeConstraint(0.0, 36.0, "ct3");
            ct3.SetCoefficient(x, 9);
            ct3.SetCoefficient(y, 1);

            Constraint ct4 = solver.MakeConstraint(0.0, 180.0, "ct4");
            ct4.SetCoefficient(x, 6);
            ct4.SetCoefficient(y, 6);

            Console.WriteLine("Number of constraints = " + solver.NumConstraints());

            // Create the objective function, 3 * x + y.
            Objective objective = solver.Objective();
            objective.SetCoefficient(x, 1.2);
            objective.SetCoefficient(y, 1.8);
            objective.SetMinimization();

            solver.Solve();

            Console.WriteLine("Solution:");
            Console.WriteLine("Objective value = " + solver.Objective().Value());
            Console.WriteLine("x = " + x.SolutionValue());
            Console.WriteLine("y = " + y.SolutionValue());

        }
    }
}
