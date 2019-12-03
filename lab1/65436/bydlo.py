import pulp

model = pulp.LpProblem("Minimize costs", pulp.LpMinimize)

LP1 = pulp.LpVariable("LP1", lowBound=0, cat='Integer')
LP2 = pulp.LpVariable("LP2", lowBound=0, cat='Integer')
LP3 = pulp.LpVariable("LP3", lowBound=0, cat='Integer')
LP4 = pulp.LpVariable("LP4", lowBound=0, cat='Integer')

model += 9.6 * LP1 + 14.4 * LP2 + 10.8 * LP3 + 7.2 * LP4, "Cost"

model += LP1 * 0.8 + LP2 * 2.4 + LP3 * 0.9 + LP4 * 0.4 >= 1200
model += LP1 * 0.6 + LP2 * 0.6 + LP3 * 0.3 + LP4 * 0.3 >= 600

model.solve()
pulp.LpStatus[model.status]

print "Ilosc paszy P1 = {}".format(LP1.varValue)
print "Ilosc paszy P2 = {}".format(LP2.varValue)
print "Ilosc paszy P3 = {}".format(LP3.varValue)
print "Ilosc paszy P4 = {}".format(LP4.varValue)
