let tablePage = 0;
let totalPages = Math.ceil((model.length / 10));



function sortTable(n) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("myTable");
    switching = true;
    //Set the sorting direction to ascending:
    dir = "asc";
    /*Make a loop that will continue until
    no switching has been done:*/
    while (switching) {
        //start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /*Loop through all table rows (except the
        first, which contains table headers):*/
        for (i = 1; i < (rows.length - 1); i++) {
            //start by saying there should be no switching:
            shouldSwitch = false;
            /*Get the two elements you want to compare,
            one from current row and one from the next:*/
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            /*check if the two rows should switch place,
               based on the direction, asc or desc:*/
            if (dir == "asc") {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    //if so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    //if so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            /*If a switch has been marked, make the switch
            and mark that a switch has been done:*/
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            //Each time a switch is done, increase this count by 1:
            switchcount++;
        } else {
            /*If no switching has been done AND the direction is "asc",
            set the direction to "desc" and run the while loop again.*/
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}

function changePage(x) {
    if (x == null) {
        if (tablePage < (totalPages - 1)) {
            tablePage = (tablePage + 1);
            nextTablePage("tableBody");
        }
    } else if (x == -1) {
        if (tablePage > 0) {
            tablePage = (tablePage - 1);
            nextTablePage("tableBody");
        }
    } else {
        tablePage = x;
        nextTablePage("tableBody");
    }
    document.getElementById(`pageNum`).innerHTML = (tablePage + 1);
}

function buildNewRow(count, name) {
    let Html = `<tr id="${name}${count}">\n`;
    Html += `<td id="${name}${count} row Cell id"></td>\n`;
    Html += `<td id="${name}${count} row Cell dateAdded"></td>\n`;
    Html += `<td id="${name}${count} row Cell originalCSV"></td>\n`;
    Html += `<td id="${name}${count} row Cell sortedCSV"></td>\n`;
    Html += `<td id="${name}${count} row Cell sortTime" align="center"></td>\n`;
    Html += `<td id="${name}${count} row Cell sortOrder" align="center"></td>\n`;
    Html += `<td id="${name}${count} row Cell links" align="right"></td>\n`;
    Html += `</tr>\n`;

    return Html;
}

function nextTablePage(tableName) {
    //get table body
    var tablebody = document.getElementById(tableName);
    //calc count of needed rows
    var neededrowCount;
    if (model.length <= 10) {
        neededrowCount = model.length;
    } else if (tablePage == (totalPages - 1)) {
        var mod = model.length % 10;
        if (mod == 0) {
            neededrowCount = 10;
        } else {
            neededrowCount = mod;
        }
    } else {
        neededrowCount = 10;
    }
    //cal firstItem Index and build items
    var firstItemIndex = (tablePage * 10);
    var Items = [];
    for (var i = 0; i < neededrowCount; i++) {
        var index = (firstItemIndex + i);
        Items.push(model[index]);
    }
    //Clear and build new table html
    tablebody.innerHTML = "";
    for (var i = 0; i < neededrowCount; i++) {
        tablebody.innerHTML += buildNewRow(i, tableName);
    }

    //write new entries
    for (i = 0; i < neededrowCount; i++) {

        document.getElementById(`${tableName}${i} row Cell id`).innerHTML = Items[i].id;
        document.getElementById(`${tableName}${i} row Cell dateAdded`).innerHTML = Items[i].dateAdded.substring(0, 19);
        document.getElementById(`${tableName}${i} row Cell originalCSV`).innerHTML = Items[i].originalCSV.substring(0, 10) + "...";
        document.getElementById(`${tableName}${i} row Cell sortedCSV`).innerHTML = Items[i].sortedCSV.substring(0, 10);
        document.getElementById(`${tableName}${i} row Cell sortTime`).innerHTML = Items[i].sortTime;
        document.getElementById(`${tableName}${i} row Cell sortOrder`).innerHTML = Items[i].sortOrder;
        let linkHtml = `<a href="SortEntries/Edit/${Items[i].id}">Edit</a> | `;
        linkHtml += `<a href="SortEntries/Details/${Items[i].id}">Details</a> | `;
        linkHtml += `<a href="SortEntries/Delete/${Items[i].id}">Delete</a> | `;
        linkHtml += `<a href="SortEntries/DownloadFile/${Items[i].id}">Download</a>`;
        document.getElementById(`${tableName}${i} row Cell links`).innerHTML = linkHtml;

    }

}


var ascendingData = [];
var ascendingDataModelIndex = [];
var descendingData = [];
var descendingDataModelIndex = [];

function addData(item, index, arr) {
    if (item.sortOrder == 1) {
        ascendingData.push({ x: item.sortTime, y: item.sortedCSV.split(",").length });
        ascendingDataModelIndex.push({ modelIndex: index });
    } else {
        descendingData.push({ x: item.sortTime, y: item.sortedCSV.split(",").length });
        descendingDataModelIndex.push({ modelIndex: index });
    }
}
model.forEach(addData)

function setSelected(activePoints) {
    var datasetid = activePoints[0].datasetIndex;
    var id = activePoints[0].index;
    var dmIndex;
    if (datasetid == 0) {
        dmIndex = ascendingDataModelIndex[id].modelIndex;
    } else {
        dmIndex = descendingDataModelIndex[id].modelIndex;
    }
    var data = model[dmIndex];

    //get table body
    var tablebody = document.getElementById("selectedTableBody");
    //Clear and build new table html
    tablebody.innerHTML = "";
    tablebody.innerHTML += buildNewRow(1, "selectedTable");

    //write new entries

    document.getElementById(`selectedTable1 row Cell id`).innerHTML = data.id;
    document.getElementById(`selectedTable1 row Cell dateAdded`).innerHTML = data.dateAdded.substring(0, 19);
    document.getElementById(`selectedTable1 row Cell originalCSV`).innerHTML = data.originalCSV.substring(0, 10) + "...";
    document.getElementById(`selectedTable1 row Cell sortedCSV`).innerHTML = data.sortedCSV.substring(0, 10);
    document.getElementById(`selectedTable1 row Cell sortTime`).innerHTML = data.sortTime;
    document.getElementById(`selectedTable1 row Cell sortOrder`).innerHTML = data.sortOrder;
    let linkHtml = `<a href="SortEntries/Edit/${data.id}">Edit</a> | `;
    linkHtml += `<a href="SortEntries/Details/${data.id}">Details</a> | `;
    linkHtml += `<a href="SortEntries/Delete/${data.id}">Delete</a> | `;
    linkHtml += `<a href="SortEntries/DownloadFile/${data.id}">Download</a>`;
    document.getElementById(`selectedTable1 row Cell links`).innerHTML = linkHtml;



    console.log(data);
}


const ctx = document.getElementById('myChart');
const myChart = new Chart(ctx, {
    type: 'scatter',
    data: {
        datasets: [{
            label: "Ascending",
            backgroundColor: 'rgba(255, 0, 0, 1)',
            data: ascendingData
        }, {
            label: "Descending",
            backgroundColor: 'rgba(0, 0, 255, 1)',
            data: descendingData
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false,
        onClick(e) {
            //	chartClickHandler(e);
            const activePoints = myChart.getElementsAtEventForMode(e, 'nearest', {
                intersect: true
            }, false)
            if (activePoints.length > 0) {
                setSelected(activePoints);
            }
        },
        scales: {
            x: {
                display: true,
                title: {
                    display: true,
                    text: 'Time Taken (MilliSeconds)'
                }
            },
            y: {
                display: true,
                title: {
                    display: true,
                    text: '# of ints sorted'
                }
            }
        },
        elements: {
            point: {
                radius: 5,
                display: true
            }
        }
    }
});

changePage(0);