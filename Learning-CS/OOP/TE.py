# variable
age = 42
name = "Johan H"
height = 1.54

# print the variable
print(f"{age} years old, {name} is {height}")
flag = True

# math & combining with text
a = 5
b = 3

_sum = a + b
print(_sum)

Qor = int(input("Qor da'daada: "))

if Qor >= 18:
    print("Waad codeyn kartaa!")
else:
    print("Ma codeyn kartid!")

x = int(input("Qor x: "))
y = int(input("Qor y: "))

def add(x, y):
    return x + y

result = add(x, y)
print(f"Naatijada waa {result}")
print("2-Naatijada waa " + str(result))