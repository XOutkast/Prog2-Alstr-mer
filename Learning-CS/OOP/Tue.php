<?php
// variables
$age = 42;
$name = "Johan H";
$height = 1.54;

// print the variable
echo "$age years old, $name is $height\n";
$flag = true;

// math & combining with text
$a = 5;
$b = 3;

$_sum = $a + $b;
echo $_sum . "\n";

// ask for user input
echo "Qor da'daada: ";
$Qor = intval(trim(fgets(STDIN)));

if ($Qor >= 18) {
    echo "Waad codeyn kartaa!\n";
} else {
    echo "Ma codeyn kartid!\n";
}

// ask for x and y
echo "Qor x: ";
$x = intval(trim(fgets(STDIN)));

echo "Qor y: ";
$y = intval(trim(fgets(STDIN)));

// function to add numbers
function add($x, $y) {
    return $x + $y;
}

$result = add($x, $y);
echo "Naatijada waa $result\n";
echo "2-Naatijada waa " . $result . "\n";
?>