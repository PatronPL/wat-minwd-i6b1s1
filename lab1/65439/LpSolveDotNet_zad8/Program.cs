using LpSolveDotNet;
using System.Diagnostics;

namespace LpSolveDotNet_Example
{

    class Program
    {
        public static void Main()
        {
            LpSolve.Init();

            zadanie8();
        }

        private static int zadanie8()
        {
         
            int Ncol = 3; // trzy zmienne w modelu

            using (LpSolve lp = LpSolve.make_lp(0, Ncol))
            {
                if (lp == null)
                {
                    return 1; // jesli nie moglo zbudowac modelu...
                }

                //nazwanie zmiennych
                lp.set_col_name(1, "W1");
                lp.set_col_name(2, "W2");
                lp.set_col_name(3, "W3");

                //przestrzen tablicowa do obliczen
                int[] colno = new int[Ncol];
                double[] row = new double[Ncol];

              
                lp.set_add_rowmode(true);

                int j = 0;

                /////////////////////////////////////// rownanie pierwsze
                j = 0;
                colno[j] = 1;
                row[j++] = 1.5;

                colno[j] = 2;
                row[j++] = 3;

                colno[j] = 3;
                row[j++] = 4;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.LE, 1500) == false)
                {
                    return 3;
                }

                /////////////////////////////////////////////////////////////////////// rownanie drugei

                j = 0;
                colno[j] = 1;
                row[j++] = 3;

                colno[j] = 2;
                row[j++] = 2;

                colno[j] = 3;
                row[j++] = 1;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.LE, 1200) == false)
                {
                    return 3;
                }

                
                lp.set_add_rowmode(false);

                //funkcja celu
                j = 0;
                colno[j] = 1; 
                row[j++] = 12;

                colno[j] = 2; 
                row[j++] = 18;

                colno[j] = 3;
                row[j++] = 12;

                if (lp.set_obj_fnex(j, row, colno) == false)
                {
                    return 4;
                }

                // szukanie maksa
                lp.set_maxim();


                lp.write_lp("model.lp");

                lp.set_verbose(3);

                lpsolve_return s = lp.solve();
                if (s != lpsolve_return.OPTIMAL)
                {
                    return 5;
                }


                Debug.WriteLine("Objective value: " + lp.get_objective());

                lp.get_variables(row);
                for (j = 0; j < Ncol; j++)
                {
                    Debug.WriteLine(lp.get_col_name(j + 1) + ": " + row[j]);
                }
            }
            return 0;
        } 
    }
}
