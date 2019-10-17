package pl.edu.wat.student.bandurski.konrad;

import com.joptimizer.exception.JOptimizerException;
import com.joptimizer.functions.ConvexMultivariateRealFunction;
import com.joptimizer.functions.LinearMultivariateRealFunction;
import com.joptimizer.optimizers.JOptimizer;
import com.joptimizer.optimizers.OptimizationRequest;


public class Podpunkt2
{
    public static void main(String[] args) throws JOptimizerException {
        // Initialize objective function
        LinearMultivariateRealFunction objectiveFunction =
                new LinearMultivariateRealFunction(new double[] {-50.0, -10.0}, 0); //50x1 + 10x2 -> max

        // Initialize inequalities constraints
        ConvexMultivariateRealFunction[] constraints = new ConvexMultivariateRealFunction[5];
        // x >= 0
        constraints[0] = new LinearMultivariateRealFunction(new double[] {-1.0, 0.0}, 0.0);
        // y >= 0
        constraints[1] = new LinearMultivariateRealFunction(new double[] {0.0, -1.0}, 0.0);
        // 12x1 + 4x2 <= 480
        constraints[2] = new LinearMultivariateRealFunction(new double[] {12.0, 4.0}, -480.0);
        // 8x1 + 8x2 <= 640
        constraints[3] = new LinearMultivariateRealFunction(new double[] {8.0, 8.0}, -640.0);
        //x1 <= x2
        constraints[4] = new LinearMultivariateRealFunction(new double[] {1.0, -1.0}, 0);

        // Initialize optimization request
        OptimizationRequest request = new OptimizationRequest();
        request.setF0(objectiveFunction);
        request.setFi(constraints);

        // Set tolerance
        request.setToleranceFeas(1.E-9);
        request.setTolerance(1.E-9);

        // Initialize JOptimizer and get optimization result
        JOptimizer optimizer = new JOptimizer();
        optimizer.setOptimizationRequest(request);
        optimizer.optimize();

        double[] solution = optimizer.getOptimizationResponse().getSolution();

        // Print result
        System.out.println("Length: " + solution.length);
        for (int i = 0; i < solution.length / 2; i++) {
            System.out.println("X" + (i + 1) + ": " + Math.round(solution[i]) + "\tY" + (i + 1) + ": " + Math.round(solution[i + 1]));
        }
    }
}