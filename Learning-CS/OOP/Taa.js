// variables
let age = 42;
let name = "Johan H";
let height = 1.54;

// print the variable
console.log(`${age} years old, ${name} is ${height}`);
let flag = true;

// math & combining with text
let a = 5;
let b = 3;

let _sum = a + b;
console.log(_sum);

// prompt user input (Node.js environment)
const readline = require('readline').createInterface({
    input: process.stdin,
    output: process.stdout
});

// ask for age
readline.question("Qor da'daada: ", (QorInput) => {
    let Qor = parseInt(QorInput);

    if (Qor >= 18) {
        console.log("Waad codeyn kartaa!");
    } else {
        console.log("Ma codeyn kartid!");
    }

    // ask for x
    readline.question("Qor x: ", (xInput) => {
        let x = parseInt(xInput);

        // ask for y
        readline.question("Qor y: ", (yInput) => {
            let y = parseInt(yInput);

            // function to add numbers
            function add(x, y) {
                return x + y;
            }

            let result = add(x, y);
            console.log(`Naatijada waa ${result}`);
            console.log("2-Naatijada waa " + result);

            readline.close();
        });
    });
});