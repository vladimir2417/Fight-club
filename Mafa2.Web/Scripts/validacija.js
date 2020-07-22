const inputsRegister = document.querySelectorAll('.regInput');
const inputsCheckout = document.querySelectorAll('.checkInput');
const inputsZahtev = document.querySelectorAll('.zahtevInput');

const patterns = {
    //registracija patterni
    ImeKorisnika: /^[A-Z][a-z]{2,}(\s[A-Z][a-z]{2,})?$/,
    PrezimeKorisnika: /^[A-Z][a-z]{1,}(\s[A-Z][a-z]{1,})?$/,
    //DatumRodjenja:  /^\d{1,2}\/\d{1,2}\/\d{4}$/
    Tezina: /^\d{2,3}$/, //dva ili 3 broja
    Visina: /^\d{3}$/, //visina mora biti u centimetrima
    MestoStanovanja: /^[A-Z][a-z]{2,}$/, //grad -> prvi karakter je Veliko slovo pa onda mala slova!
    Adresa: /^[A-Z][a-z]{2,}(\s[A-Z][a-z]{2,}){0,}\s\d+([a-zA-Z]{1,2})?$/,
    //adresa -> prvi karakter je Veliko slovo pa onda minimum dva mala, pa onda space pa onda broj
    BrDobijenihBorbi: /^\d{1,}$/,
    BrIzgubljenihBorbi: /^\d{1,}$/,
    Godine: /^\d{2}$/,
    Email: /^([a-z\d\.-]+)@([a-z\d-]+)\.([a-z]{2,8})(\.[a-z]{2,8})?$/,
    BrTelefona: /^((0)|(\+381))6\d(([0-9]{7})|([0-9]{6}))$/,
    UsernameKorisnik: /^[A-Za-z]{1}[A-Za-z\d]{4,14}$/, //username mora biti alfanumericki i da sadrzi 5-15 karaktera, ne moze poceti brojem
    PasswordKorisnika1: /^[\w@-]{5,20}$/, //password moze biti \w (bilo koji karakter: a-z, A-Z, 0-9 i _) i znakovi @ i -
    //kraj registracije patterna


    //ckeckout patterni
    AdresaZaIsporuku: /^[A-Z][a-z]{2,}(\s[A-Z][a-z]{2,}){0,}\s\d+([a-zA-Z]+)?$/,
    Grad: /^[A-Z][a-z]{2,}$/,
    ZipCode: /^\d{5}$/, //srpski zip code mora sadrzati 5 brojeva
    //kraj checkout patterna

    //zahtev za borbu patterni
    ZahtevanoMesto: /^[A-Z][a-z]{2,}$/
    //kraj zahteva za borbu patterna

};

//validation function
function validate(field, regex) {
    if (regex.test(field.value)) {
        //prosla validacija
        field.classList.remove("invalid");
        field.classList.add("valid");
    } else {
        //nije prosla validacija
        field.classList.remove("valid");
        field.classList.add("invalid");
    }
    //console.log(regex.test(field.value));
}

//dodajemo event listener-e svakom od registracionih inputa!
inputsRegister.forEach(function(inputReg) {
    inputReg.addEventListener('keyup', (e) => {
        //console.log(e.target.attributes.name.value);
        validate(e.target, patterns[e.target.attributes.name.value]);
    });
});

//dodajemo event listener-e svakom od chackout inputa!
inputsCheckout.forEach(function (inputCheck) {
    inputCheck.addEventListener('keyup', (e) => {
        validate(e.target, patterns[e.target.attributes.name.value]);
    });
});

//dodajemo event listener-e svakom od zahtev inputa!
inputsZahtev.forEach(function (inputZah) {
    inputZah.addEventListener('keyup', (e) => {
        validate(e.target, patterns[e.target.attributes.name.value]);
    });
});




//validacija forme onsubmit, ne pusta da se forma submituje ukoliko bilo koje
//registraciono polje sadrzi invalid klasu!!!
function validateRegForm() {
    var validno = true;
    for (var i = 0; i < inputsRegister.length; i++) {
        if (inputsRegister[i].classList.contains("invalid")) {
            event.preventDefault();
            alert("Validacija unetih podataka nije prošla! Molimo Vas, ispravite greške.");
            validno = false;
            break;
        }
    }

    return validno;
}

//validacije checkout forme, ne pusta da se forma submituje ukoliko neko od polja nije validno
//odnosno ima klasu invalid
function validateCheckOutForm() {
    var validno = true;
    for (var i = 0; i < inputsCheckout.length; i++) {
        if (inputsCheckout[i].classList.contains("invalid")) {
            event.preventDefault();
            alert("Validacija unetih podataka nije prošla. Molimo Vas, ispravite greške.");
            validno = false;
            break;
        }
    }
    return validno;
}


//validacija forme za zahtev za borbu, ne pusta da se forma submituje ukoliko neko od polja nije validno
//odnosno ima klasu invalid
function validateZahtevForm() {
    var validno = true;
    for (var i = 0; i < inputsZahtev.length; i++) {
        if (inputsZahtev[i].classList.contains("invalid")) {
            event.preventDefault();
            alert("Validacija unetih podataka nije prošla. Molimo Vas, ispravite greške.");
            validno = false;
            break;
        }
    }
    return validno;
}



