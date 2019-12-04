using LpSolveDotNet;
using System.Diagnostics;

namespace LpSolveDotNet_Example
{

    class Program
    {
        public static void Main()
        {
            LpSolve.Init();

            SolveA();
            SolveB();
        }
        private static int SolveA()
        {
            int lKolumn = 2; // trzy zmienne w modelu

            using (LpSolve lp = LpSolve.make_lp(0, lKolumn))
            {
                if (lp == null)
                {
                    return 1; // jesli nie moglo zbudowac modelu...
                }

                //nazwanie zmiennych
                lp.set_col_name(1, "P1");
                lp.set_col_name(2, "P2");

                //przestrzen tablicowa do obliczen
                int[] colno = new int[lKolumn];
                double[] row = new double[lKolumn];

                lp.set_add_rowmode(true);

                int j = 0;

                // rownanie pierwsze
                j = 0;
                colno[j] = 1;
                row[j++] = 3;

                colno[j] = 2;
                row[j++] = 9;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 27) == false)
                {
                    return 3;
                }

                // rownanie drugie

                j = 0;
                colno[j] = 1;
                row[j++] = 8;

                colno[j] = 2;
                row[j++] = 4;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 32) == false)
                {
                    return 3;
                }

                // rownanie trzecie

                j = 0;
                colno[j] = 1;
                row[j++] = 12;

                colno[j] = 2;
                row[j++] = 3;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 36) == false)
                {
                    return 3;
                }

                lp.set_add_rowmode(false);

                //funkcja celu
                j = 0;
                colno[j] = 1;
                row[j++] = 6;

                colno[j] = 2;
                row[j++] = 9;


                if (lp.set_obj_fnex(j, row, colno) == false)
                {
                    return 4;
                }

                // szukanie minimum
                lp.set_minim();

                lp.write_lp("model.lp");

                lp.set_verbose(3);

                lpsolve_return s = lp.solve();
                if (s != lpsolve_return.OPTIMAL)
                {
                    return 5;
                }


                Debug.WriteLine("Objective value: " + lp.get_objective());

                lp.get_variables(row);
                for (j = 0; j < lKolumn; j++)
                {
                    Debug.WriteLine(lp.get_col_name(j + 1) + ": " + row[j]);
                }
            }
            return 0;
        }
        private static int SolveB()
        {
            // liczba zmiennych w modelu
            int lKolumn = 2;

            using (LpSolve lp = LpSolve.make_lp(0, lKolumn))
            {
                // jesli nie udalo sie zbudowac modelu
                if (lp == null)
                {
                    return 1;
                }

                // nazwanie zmiennych
                lp.set_col_name(1, "P1");
                lp.set_col_name(2, "P2");

                // przestrzen tablicowa do obliczen
                int[] colno = new int[lKolumn];
                double[] row = new double[lKolumn];

                lp.set_add_rowmode(true);

                int j = 0;

                // rownanie pierwsze
                j = 0;
                colno[j] = 1;
                row[j++] = 3;

                colno[j] = 2;
                row[j++] = 9;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 27) == false)
                {
                    return 3;
                }

                // rownanie drugie
                j = 0;
                colno[j] = 1;
                row[j++] = 8;

                colno[j] = 2;
                row[j++] = 4;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 32) == false)
                {
                    return 3;
                }

                // rownanie trzecie
                j = 0;
                colno[j] = 1;
                row[j++] = 12;

                colno[j] = 2;
                row[j++] = 3;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 36) == false)
                {
                    return 3;
                }

                // rownanie czwarte
                j = 0;
                colno[j] = 1;
                row[j++] = 1;

                colno[j] = 2;
                row[j++] = -1;

                // dodanie rzedu do lpsolve
                if (lp.add_constraintex(j, row, colno, lpsolve_constr_types.GE, 1) == false)
                {
                    return 3;
                }

                lp.set_add_rowmode(false);


                //funkcja celu
                j = 0;
                colno[j] = 1;
                row[j++] = 6;

                colno[j] = 2;
                row[j++] = 9;


                if (lp.set_obj_fnex(j, row, colno) == false)
                {
                    return 4;
                }

                // szukanie minimum
                lp.set_minim();


                lp.write_lp("model.lp");

                lp.set_verbose(3);

                lpsolve_return s = lp.solve();
                if (s != lpsolve_return.OPTIMAL)
                {
                    return 5;
                }


                Debug.WriteLine("Objective value: " + lp.get_objective());

                lp.get_variables(row);
                for (j = 0; j < lKolumn; j++)
                {
                    Debug.WriteLine(lp.get_col_name(j + 1) + ": " + row[j]);
                }
            }
            return 0;
        }
    }
}
