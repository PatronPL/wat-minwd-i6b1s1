import org.ojalgo.optimisation.Expression;
import org.ojalgo.optimisation.ExpressionsBasedModel;
import org.ojalgo.optimisation.Optimisation;
import org.ojalgo.optimisation.Variable;

public class Ojalgo {

    public static void main(final String[] args) {


        final Variable a = Variable.make("A").lower(0).weight(2);
        final Variable b = Variable.make("B").lower(0).weight(4);

        final ExpressionsBasedModel model = new ExpressionsBasedModel();
        model.addVariable(a);
        model.addVariable(b);

       // final Expression limit_1 = model.addExpression("Zuzycie na jednostke I").lower(0).upper(96000); //v1
       // limit_1.set(a, 8).set(b, 16);

         final Expression limit_1 = model.addExpression("Zuzycie na jednostke I").lower(0).upper(5000); // v2
            limit_1.set(a, 8).set(b, 16);

       //final Expression limit_2 = model.addExpression("Ograniczenie 2").lower(0).upper(56000); //v1
        //limit_2.set(a,7).set(b, 4);

        final Expression limit_2 = model.addExpression("Ograniczenie 2").lower(0).upper(4000);  // v2
         limit_2.set(a,7).set(b, 4);

        a.integer(true);
        b.integer(true);

        Optimisation.Result wynik = model.maximise();


        System.out.println(wynik.toString());
        System.out.println(model.toString());

    }

} 