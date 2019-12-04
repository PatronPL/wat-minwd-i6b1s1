import scpsolver.constraints.LinearBiggerThanEqualsConstraint;
import scpsolver.lpsolver.LinearProgramSolver;
import scpsolver.lpsolver.SolverFactory;
import scpsolver.problems.LinearProgram;


public class Main {
    public static void main(String[] args) {
        LinearProgram lp = new LinearProgram(new double[]{6.0, 3.0, 5, 2});
        lp.addConstraint(new LinearBiggerThanEqualsConstraint(new double[]{15.0, 10.0, 20.0, 19.0}, 26000.0, "c1"));
        lp.addConstraint(new LinearBiggerThanEqualsConstraint(new double[]{9.0, 3.0, 5.0, 10.0}, 100000.0, "c2"));
        lp.setMinProblem(true);
        LinearProgramSolver solver = SolverFactory.newDefault();
        double[] sol = solver.solve(lp);
    }
}





