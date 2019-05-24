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
    document.getElementById("testers").style.display = "table";
    document.getElementById("AddTester").style.display = "none";


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
    document.getElementById("AddTester").style.display = "none";

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
    document.getElementById("AddTester").style.display = "none";

}

//Show about 
function showAbout() {
    document.getElementById("about").style.display = "block";
    document.getElementById("tableHeared").style.display = "block";
    document.getElementById("tableHeared").innerHTML = "About";
    document.getElementById("testers").style.display = "none";
    document.getElementById("out").style.display = "none";
    document.getElementById("AddTester").style.display = "none";

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
                row += "<tr style=\"cursor:pointer;\" onclick=\"clickTableRow(this)\" id=\"" + data[i].id +"\"><td><b>" + data[i].id + "</b></td><td>" + data[i].firstName + " "
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

//send new tester
function sendNewTester(url) {
    tester = testerAdd;
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            //Parse the data
            var data = this.responseText;
            alert(data);
        }
    };
        xhttp.open("POST", url, true);
    xhttp.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    xhttp.send(JSON.stringify(tester));
}

function addTester() {
    document.getElementById("about").style.display = "none";
    document.getElementById("tableHeared").style.display = "block";
    document.getElementById("tableHeared").innerHTML = "Add New Tester";
    document.getElementById("testers").style.display = "none";
    document.getElementById("out").style.display = "none";
    document.getElementById("AddTester").style.display = "block";
    if (testerAdd == null || testerAdd == undefined) {
        testerAdd = {
            "Experience": 0,
            "MaxWeekExams": 0,
            "LicenseTypeTeaching": [],
            "MaxDistance": 0,
            "Schedule": null,
            "Id": 0,
            "EmailAddress": "youremail@gmail.com",
            "FirstName": "First Name",
            "LastName": "Last Name",
            "BirthDate": "1980-01-01",
            "Gender": 0,
            "PhoneNumber": "0500000000",
            "Address": "Israel",
            "LicenseType": []
        };
    }
    createForm(testerAdd, document.getElementById("AddTester"),"sendNewTester(\'https://localhost:44308/api/tester/add\')","Send");

}

function createForm(object,div,buttonevent,buttontext){  
    var str='<table class="none">';
    for (var key in object) {
        if (key == 'Gender' || key == 'gender') {
            var selctedM = "", selctedF = "", selctedO = "";
            if (object[key] == 0) {
                selctedM = "selected";
            } else if (object[key] == 1) {
                selctedF = "selected";
            } else {
                selctedO = "selected";
            }
            var text = key.replace(/([A-Z])/g, ' $1').trim();
            str += '<tr class="none"><td><span>' + text + '</span></td><td><select id="' + key + '" type="text" onchange="onchangeevent(this)"><option value="Male"' + selctedM + '>Male</option><option value="Female"' + selctedF + '>Female</option><option value="Other"' + selctedO +'>Other</option></br></td></tr>';
        } else if (key == 'BirthDate' || key == 'birthDate') {
            var text = key.replace(/([A-Z])/g, ' $1').trim();
            str += '<tr class="none"><td><span>' + text + '</span></td><td><input id="' + key + '" type="date" onchange="onchangeevent(this)" value="' + object[key] + '" ></br></td></tr>';
        } else if (key == 'LicenseTypeTeaching' || key == 'licenseTypeTeaching') {
            var text = key.replace(/([A-Z])/g, ' $1').trim();
            str += '<tr class="none"><td><span>' + text + '</span></td><td><select multiple size="4" id="' + key + '" type="text" onclick="onchangeevent(this)" value="' + object[key] + '" >';
            for (var i = 0; i < licenses.length; i++) {
                if (testerAdd.LicenseTypeTeaching != undefined && testerAdd.LicenseTypeTeaching.includes(i) || testerAdd.licenseTypeTeaching != undefined && testerAdd.licenseTypeTeaching.includes(i)) {
                    str += '<option value="' + licenses[i] + '" selected>' + licenses[i] + '</option>';
                } else {
                    str += '<option value="' + licenses[i] + '">' + licenses[i] + '</option>';
                }
            }
            str += '</br></td></tr>';
        } else if (((typeof object[key]) == "number" || (typeof object[key]) == "string")) {
            var text = key.replace(/([A-Z])/g, ' $1').trim();
            str += '<tr class="none"><td><span>' + text + '</span></td><td><input id="' + key + '" type="text" onchange="onchangeevent(this)" value="' + object[key] + '" ></br></td></tr>';
        } 
    }
    str += '<tr class="none"><td><button onclick="' + buttonevent + '">' + buttontext + '</button></td></tr></table>';
    div.innerHTML = str;
}

function onchangeevent(elem) {
    if (elem.id == 'Gender' || elem.id == 'gender') {
        if (elem.value == 'Male') {
            testerAdd[elem.id] = 0;
        } else if (elem.value == 'Female') {
            testerAdd[elem.id] = 1;
        } else {
            testerAdd[elem.id] = 2;
        }
    } if (elem.id == 'LicenseTypeTeaching' || elem.id == 'licenseTypeTeaching') {
        var sel = elem;
        var opts = [], opt;
        // loop through options in select list
        for (var i = 0, len = sel.options.length; i < len; i++) {
            opt = sel.options[i];

            // check if selected
            if (opt.selected) {
                // add to array of option elements to return from this function
                opts.push(i);
            }
        }
        testerAdd[elem.id] = opts;
    }else {
        testerAdd[elem.id] = elem.value;
    }
}

var licenses = ['B', 'A2', 'A1', 'A', 'C1', 'C', 'D', 'D1', 'D2', 'D3', 'E', '1']
var testerAdd = null;


function clickTableRow(elem) {
    showTester(elem.id);
}

function showTester(id) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            var data = JSON.parse(this.responseText);
            if (data != null && data != undefined) {
                testerAdd = data;
                document.getElementById("about").style.display = "none";
                document.getElementById("tableHeared").style.display = "block";
                document.getElementById("tableHeared").innerHTML = "Tester ID: " + testerAdd.id;
                document.getElementById("testers").style.display = "none";
                document.getElementById("out").style.display = "none";
                document.getElementById("AddTester").style.display = "block";
                testerAdd.birthDate = testerAdd.birthDate.substring(0, 10);
                createForm(testerAdd, document.getElementById("AddTester"),"","Update");

            }
        }
    };
    xhttp.open("GET", "https://localhost:44308/api/tester/"+id, true);
    xhttp.send();
}