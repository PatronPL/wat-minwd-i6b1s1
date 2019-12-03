import pulp

my_lp_problem = pulp.LpProblem("My LP Problem", pulp.LpMaximize)

x1 = pulp.LpVariable("x1", 0, None, pulp.LpInteger)
x2 = pulp.LpVariable("x2", 0, None, pulp.LpInteger)
x3 = pulp.LpVariable("x3", 0, None, pulp.LpInteger)
x4 = pulp.LpVariable("x4", 0, None, pulp.LpInteger)

my_lp_problem += 6 * x1 + 3 * x2 + 5 * x3 + 2 * x4, "Z"

my_lp_problem += 15 * x1 + 10 * x2 + 20 * x3 + 19 * x4 <= 26000
my_lp_problem += 9 * x1 + 3 * x2 + 5 * x3 + 10 * x4 <= 100000

print(my_lp_problem)

my_lp_problem.solve()
print(pulp.LpStatus[my_lp_problem.status])

for variable in my_lp_problem.variables():
    print(variable.name, variable.varValue)

print(pulp.value(my_lp_problem.objective))