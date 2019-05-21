//*********Main Pages ********/

//load the tester table interface
function loadUrlTesterToTable(url) {
    loadPerson(document.getElementById("testers"), document.getElementById("out"), url);
    var header = document.getElementById("tableHeared");
    header.style.display = "block";
    header.innerHTML = "Testers";
    document.getElementById("out").style.display = "block";
    document.getElementById("about").style.display = "none";
    document.getElementById("testers").style.display = "table";

}

//load the trainee table interface
function loadUrlTraineeToTable(url) {
    loadPerson(document.getElementById("testers"), document.getElementById("out"), url);
    var header = document.getElementById("tableHeared");
    header.style.display = "block";
    header.innerHTML = "Trainees";
    document.getElementById("out").style.display = "block";
    document.getElementById("about").style.display = "none";
    document.getElementById("testers").style.display = "table";

}

//load the tests table interface
function loadUrlTestToTable(url) {
    loadTest(document.getElementById("testers"), document.getElementById("out"), url);
    var header = document.getElementById("tableHeared");
    header.style.display = "block";
    header.innerHTML = "Tests";
    document.getElementById("out").style.display = "block";
    document.getElementById("about").style.display = "none";
    document.getElementById("testers").style.display = "table";

}

//Show about 
function showAbout() {
    document.getElementById("about").style.display = "block";
    document.getElementById("tableHeared").style.display = "block";
    document.getElementById("tableHeared").innerHTML = "About";
    document.getElementById("testers").style.display = "none";
    document.getElementById("out").style.display = "none";
}



//********Get from server func */

//load Persons from server
function loadPerson(table, label, url) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            //Parse the data
            var data = JSON.parse(this.responseText);
            var length = data.length;
            //Create table content
            var row = "<tr style=\"border:1px solid gray\"><th>Id</th><th>Name</th><th>Birth Date</th><th>Gender</th><th>Phone Number</th><th>Email Address</th><th>Licence</th></tr>";
            for (var i = 0; i < length; i++) {
                var date = new Date(data[i].birthDate);
                row += "<tr><td><b>" + data[i].id + "</b></td><td>" + data[i].firstName + " "
                    + data[i].lastName + "</td><td>" + getDateS(date) + "</td><td>"
                    + getGender(data[i].gender) + "</td><td>" + getValue(data[i].phoneNumber) + "</td><td><a target=\"_blank\" href=\"mailto:"
                    + data[i].emailAddress + "\">" + data[i].emailAddress + "</td><td>" + getLiceneFromList(data[i]) + "</td></tr>";
            }
            table.innerHTML = row;
            label.innerHTML = "Count: " + data.length;
            //if there is an error
        } else if (this.readyState == 4 && (this.status == 200 || this.status == 403 || this.status == 404 || this.status == 0)) {
            table.innerHTML = "<tr><th>Sorry Can't Load The Data. Please Try Again Later. Status: " + this.status + "</th></tr>";
        }
    };
    xhttp.open("GET", url, true);
    xhttp.send();
}

//load tests from server
function loadTest(table, label, url) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        //parse data
        if (this.readyState == 4 && this.status == 200) {
            var data = JSON.parse(this.responseText);
            //Create table content
            var length = data.length;
            var row = "<tr style=\"border:1px solid gray\"><th>Id</th><th>Tester Id</th><th>Trainee Id</th><th>Test Address</th><th>Actual Date</th><th>Test Date</th><th>License</th><th>Status</th><th>Test Route</th></tr>";;
            for (var i = 0; i < length; i++) {
                row += "<tr><td><b>" + data[i].id + "</b></td><td>" + data[i].testerId + "</td><td>" + data[i].traineeId + "</td><td>"
                    + getAddress(data[i].addressOfBeginningTest) + "</td><td>" + getDateS(new Date(data[i].actualTestTime)) + "</td><td>"
                    + getDateS(new Date(data[i].testTime)) + "</td><td>" + getLicense(data[i].licenseType) + "</td><td>" + getPassed(data[i].passed) +
                    "</td><td>" + getLink(data[i].routeUrl, "Show Route") + "</td></tr>";
            }
            table.innerHTML = row;
            label.innerHTML = "Count: " + data.length;
            //if there is an error
        } else if (this.readyState == 4 && (this.status == 200 || this.status == 403 || this.status == 404 || this.status == 0)) {
            table.innerHTML = "<tr><th>Sorry Can't Load The Data. Please Try Again Later. Status: " + this.status + "</th></tr>";
        }
    };
    xhttp.open("GET", url, true);
    xhttp.send();
}

//*********Convert data to string */

//Get gender enum in text
function getGender(num) {
    if (num == 0) {
        return "Male";
    } else if (num == 1) {
        return "Female";
    } else {
        return "Other";
    }
}

//Check if value is empty
function getValue(val) {
    if (val == null && val == undefined) {
        return "";
    }
    return val;
}

//Get the lisence list in text
function getLiceneFromList(data) {
    var list;
    //if it is a tester
    if (data.licenseTypeTeaching != undefined) {
        list = data.licenseTypeTeaching;
        var str = "";
        for (var i = 0; i < list.length; i++) {
            str += getLicense(list[i]);
            if (i != list.length - 1) {
                str += ", ";
            }
        }
        return str;
        //if it is a trainee
    } else if (list = data.licenseTypeLearning != undefined) {
        list = data.licenseTypeLearning;
        var str = "";
        for (var i = 0; i < list.length; i++) {
            str += getLicense(list[i].license);
            if (i != list.length - 1) {
                str += ", ";
            }
        }
        return str;
    }
}

//Get license enum as text
function getLicense(data) {
    if (data == 0) {
        return "B";
    } else if (data == 1) {
        return "A2";
    } else if (data == 2) {
        return "A1";
    } else if (data == 3) {
        return "A";
    } else if (data == 4) {
        return "C1";
    } else if (data == 5) {
        return "C";
    } else if (data == 6) {
        return "D"
    } else if (data == 7) {
        return "D1";
    } else if (data == 8) {
        return "D2";
    } else if (data == 9) {
        return "D3";
    } else if (data == 10) {
        return "E";
    } else if (data == 11) {
        return "1";
    }
}

//get address as string
function getAddress(add) {
    return add.city + " " + add.street + " " + add.building + " " + add.entrance;
}
function getPassed(pss) {
    if (pss == null) {
        return "No Result";
    } else if (!pss) {
        return "Failed";
    }
    return "Passed";
}

//get date as dd/mm/yyyy
function getDateS(d) {
    if (d.getUTCFullYear() == 0) {
        return "";
    }
    return d.getDate() + "/" + d.getMonth() + "/" + d.getUTCFullYear();
}

//get link
function getLink(url, text) {
    if (url == null || url == undefined) {
        return "Not Set";
    }
    return "<a href=\"" + url + "\" target=\"_blank\">" + text + "</a>";
}



