#!/usr/bin/env python
# coding: utf-8

# In[1]:


from pulp import *

# A new problem
prob = LpProblem("example", LpMaximize)

# Variables
x = LpVariable("w1", 0, None, LpInteger)
y = LpVariable("w2", 0, None, LpInteger)
z = LpVariable("w3", 0, None, LpInteger)

# Objective
prob += 12*x + 18*y + 12*z, "objective"

# Constraints
prob += 1.5*x + 3*y + 4*z <= 1500, "c1"
prob += 3*x + 2*y + z <= 1200, "c2"

# Solve the problem using the default solver
prob.solve()

# Print the status of the problem
print("Status:", LpStatus[prob.status])

# Print the value of the variables
for v in prob.variables():
	print(v.name, " = ", v.varValue)

# Print the value of the objective
print("objective = ", value(prob.objective))


# In[ ]:




