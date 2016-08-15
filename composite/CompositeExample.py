import sys

class Component:
    def __init__(self, atom):
        pass
    def __str__(self):
        pass
    def value(self):
        pass

class Atom(Component):
    def __init__(self, atom):
        self.atom = atom
        
    def __str__(self):
        return self.atom
    
    def value(self):
        try: 
            return float(self.atom)
        except: 
            pass
        
        if self.atom in Symbols: 
            return Symbols[self.atom]
        
        print "Error: " + self.atom
        sys.exit(-1)
    
class Composite(Component):
    def __init__(self, oper, leftExpression, rightExpression):
        self.oper = oper
        self.leftExpression = leftExpression
        self.rightExpression = rightExpression
        
    def __str__(self):
        return "({0} {1} {2})".format(self.leftExpression.__str__(), self.oper, self.rightExpression.__str__())
    
    def value(self):
        pass
        
class CompositeAdd(Composite):
    def value(self):
        return self.leftExpression.value() + self.rightExpression.value()
class CompositeSub(Composite):
    def value(self):
        return self.leftExpression.value() - self.rightExpression.value()
class CompositeMul(Composite):
    def value(self):
        return self.leftExpression.value() * self.rightExpression.value()
class CompositeDiv(Composite):
    def value(self):
        return self.leftExpression.value() / self.rightExpression.value()

class Operator:
    def getKompozit(self):
        pass
        
class Add(Operator):
    def getKompozit(self):
        return CompositeAdd
    def __str__(self):
        return '+'
class Sub(Operator):
    def getKompozit(self):
        return CompositeSub
    def __str__(self):
        return '-'
class Mul(Operator):
    def getKompozit(self):
        return CompositeMul
    def __str__(self):
        return '*'
class Div(Operator):
    def getKompozit(self):
        return CompositeDiv
    def __str__(self):
        return '/'

Operators = [Add(), Sub(), Mul(), Div()]

def CompositeFactory(operator, leftExpression, rightExpression):
    for oper in Operators:
        if oper.__str__() == operator:
            return oper.getKompozit()(operator, leftExpression, rightExpression)
    

def parse(strInput):
    for operator in ["+-", "*/"]:
        depth = 0
        for p in range(len(strInput) - 1, -1, -1):
            if strInput[p] == ')': depth += 1
            elif strInput[p] == '(': depth -= 1
            elif depth==0 and strInput[p] in operator:
                # strInput is a compound expression
                return CompositeFactory(strInput[p], parse(strInput[:p]), parse(strInput[p+1:]))
    strInput = strInput.strip()
    if strInput[0] == '(':
        # strInput is a parenthesized expression?
        return parse(strInput[1:-1])
    # strInput is an atom!
    return Atom(strInput)

Symbols = {}