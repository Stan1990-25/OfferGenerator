// Adding contact person
function AddContactP()
{

    if (document.getElementById("cpName").value == "")
    {
        (document.getElementById("cpName")).style.borderColor = '#ff0000';
        return;
    }

    var table = document.getElementById("cpTable").getElementsByTagName("tbody")[0];

    var newRow = table.insertRow();

    newRow.insertCell(0).appendChild(document.createTextNode(document.getElementById("cpName").value));

    newRow.insertCell(1).appendChild(document.createTextNode(document.getElementById("cpPhone").value));

    newRow.insertCell(2).appendChild(document.createTextNode(document.getElementById("cpEmail").value));

    var delButton = document.createElement("div");
    delButton.innerHTML = "<a class='btn btn-danger' id='cprow" + newRow.rowIndex + "' onclick='DeleteRow()'>  -  </a>";

    newRow.insertCell(3).appendChild(delButton);

    (document.getElementById("cpName")).style.borderColor = '#ced4da';
}

// Adding delivery address
function AddDelAdd()
{

    if (document.getElementById("daAddress").value == "")
    {
        (document.getElementById("daAddress")).style.borderColor = '#ff0000';
        return;
    }

    var table = document.getElementById("daTable").getElementsByTagName("tbody")[0];

    var newRow = table.insertRow();

    newRow.insertCell(0).appendChild(document.createTextNode(document.getElementById("daAddress").value));

    var delButton = document.createElement("div");
    delButton.innerHTML = "<a class='btn btn-danger' id='darow" + newRow.rowIndex + "' onclick='DeleteRow()'>  -  </a>";

    newRow.insertCell(1).appendChild(delButton);

    (document.getElementById("daAddress")).style.borderColor = '#ced4da';
}

// Adding Courier
function AddCourier()
{

    if (document.getElementById("crName").value == "")
    {
        (document.getElementById("crName")).style.borderColor = '#ff0000';
        return;
    }

    var table = document.getElementById("crTable").getElementsByTagName("tbody")[0];

    var newRow = table.insertRow();

    newRow.insertCell(0).appendChild(document.createTextNode(document.getElementById("crName").value));

    var delButton = document.createElement("div");
    delButton.innerHTML = "<a class='btn btn-danger' id='crrow" + newRow.rowIndex + "' onclick='DeleteRow()'>  -  </a>";

    newRow.insertCell(1).appendChild(delButton);

    (document.getElementById("crName")).style.borderColor = '#ced4da';
}

// Add event listener to Add Contact table
function DeleteRow()
{
    document.addEventListener("click", DeleteRowClick);
}

// Deleting row from the contact person table
function DeleteRowClick(evHandler)
{
    var rowCall = evHandler.target.id;

    var indexOfCP = rowCall.indexOf("cp");
    var indexOfDA = rowCall.indexOf("da");
    var indexOfCr = rowCall.indexOf("cr");

    if (indexOfCP > -1)
    {
        var rowCP = document.getElementById(rowCall);

        var rowRealCP = rowCP.parentNode.parentNode.parentNode;

        var indexCP = rowRealCP.rowIndex;

        document.getElementById("cpTable").deleteRow(indexCP);
        document.removeEventListener("click", DeleteRowClick);
    }

    if (indexOfDA > -1)
    {
        var rowDA = document.getElementById(rowCall);

        var rowRealDA = rowDA.parentNode.parentNode.parentNode;

        var indexDA = rowRealDA.rowIndex;

        document.getElementById("daTable").deleteRow(indexDA);
        document.removeEventListener("click", DeleteRowClick);
    }

    if (indexOfCr > -1)
    {
        var rowCr = document.getElementById(rowCall);

        var rowRealCr = rowCr.parentNode.parentNode.parentNode;

        var indexCr = rowRealCr.rowIndex;

        document.getElementById("crTable").deleteRow(indexCr);
        document.removeEventListener("click", DeleteRowClick);
    }

}

// Construction JSON string for the controller
function ConstrutJSON(id, url)
{
    // Checking whether the Name textbox has empty string
    var nameInput = true;
    if (document.getElementById("clName").value == "") {
        (document.getElementById("clName")).style.borderColor = '#FF0000'; // ced4da
        nameInput = false;
    }
    // Checking whether the City textbox has empty string
    var cityInput = true;
    if (document.getElementById("clCity").value == "") {
        (document.getElementById("clCity")).style.borderColor = '#FF0000'; // ced4da
        cityInput = false;
    }
    // Checking whether the Discount textbox is a number
    if (isNaN(document.getElementById("clDiscount").value) || !nameInput || !cityInput) {
        (document.getElementById("clDiscount")).style.borderColor = '#FF0000'; // ced4da
        return;
    }


    var jsonForController =
    {
        "Client": { "Id": 0, "Name": "", "Discount": 20, "City": "" },
        "Contacts": [{ "Id": 0, "Name": "", "PhoneNumber": "", "EmailAddress": "" }],
        "DeliveryAddresses": [{ "Id": 0, "DelAddress": "" }],
        "Couriers": [{ "Id": 0, "CourierName": "" }]
    }

    jsonForController.Client.Id = id;
    jsonForController.Client.Name = document.getElementById("clName").value;
    jsonForController.Client.Discount = document.getElementById("clDiscount").value;
    jsonForController.Client.City = document.getElementById("clCity").value;

    // Contact persons
    var tableCP = document.getElementById("cpTable");
    jsonForController.Contacts = []; // Initializing of the Contacts array
    for (var i = 0; i < tableCP.lastElementChild.children.length; i++)
    {

        jsonForController.Contacts.push({
            Id: id,
            Name: tableCP.lastElementChild.children[i].children[0].innerText,
            PhoneNumber: tableCP.lastElementChild.children[i].children[1].innerText,
            EmailAddress: tableCP.lastElementChild.children[i].children[2].innerText
        });
    }

    // Delivery addresses
    var tableDA = document.getElementById("daTable");
    jsonForController.DeliveryAddresses = []; // Initializing of the DeliveryAddress array
    for (var i = 0; i < tableDA.lastElementChild.children.length; i++)
    {
        jsonForController.DeliveryAddresses.push({
            Id: id,
            DelAddress: tableDA.lastElementChild.children[i].children[0].innerText,
        });
    }

    // Couriers
    var tableCr = document.getElementById("crTable");
    jsonForController.Couriers = []; // Initializing of the Couriers array
    for (var i = 0; i < tableCr.lastElementChild.children.length; i++)
    {
        jsonForController.Couriers.push({
            Id: id,
            CourierName: tableCr.lastElementChild.children[i].children[0].innerText,
        });
    }

    $.ajax({
        url: url,
        type: "POST",
        dataType: "json",
        data: jsonForController,
        success: RedirectTo
    });
}

// Redirect to another page
function RedirectTo(res)
{
    window.location.href = res.newUrl;
}

//TestFunc
function Test(model)
{
    var str = JSON.stringify(model);
}


