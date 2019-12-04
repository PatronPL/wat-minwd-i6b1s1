#!/usr/bin/env python
# coding: utf-8
# In[3]:

import pulp
# In[4]:
model = pulp.LpProblem("Profit minimizing problem", pulp.LpMinimize)
# In[5]:
P1 = pulp.LpVariable('P1', lowBound=0, cat='Integer')
P2 = pulp.LpVariable('P2', lowBound=0, cat='Integer')
# In[6]:
# In[7]:
model += 1.2 * P1 + 1.8 * P2, "Profit"

model += 6 * P1 + 3 * P2 >= 120
model += 1 * P1 + 3 * P2 >= 60
model += 9 * P1 + 1 * P2 >= 36
model += 6 * P1 + 6 * P2 >= 180
# In[8]:
model.solve()

print(pulp.LpStatus[model.status])

# In[10]:
print( "Production of Product 1 = {}".format(P1.varValue))
print( "Production of Product 2 = {}".format(P2.varValue))
# In[11]:
# In[12]:
print (pulp.value(model.objective))
# In[13]:
model += 6 * P1 + 3 * P2 <= 240
# In[14]:
print( "Production of Product 1 = {}".format(P1.varValue))
print( "Production of Product 2 = {}".format(P2.varValue))
# In[15]:
print (pulp.value(model.objective))



