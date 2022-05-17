
//Set selected point on graph to selected table, onclick event for chart
function setSelected(activePoints) {
    //Get seleted point datasetIndex and index in that dataset
    var datasetid = activePoints[0].datasetIndex;
    var id = activePoints[0].index;
    var dmIndex;
    if (datasetid == 0) {
        dmIndex = ascendingDataModelIndex[id].modelIndex;
    } else {
        dmIndex = descendingDataModelIndex[id].modelIndex;
    }
 
    $('#selectedTableRow').html($('#myTable tbody tr')[dmIndex].innerHTML);

}

//Build chart
//Chart Data
var ascendingData = [];
var ascendingDataModelIndex = [];
var descendingData = [];
var descendingDataModelIndex = [];
//Populate chart data
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

// jQueary pageification
let currentpage = 0;
$(document).ready(function () {  
    $('#pageNum').html(currentpage + 1);
    var rowsShown = 10;
    var rowsTotal = $('#myTable tbody tr').length;
    var numPages = Math.ceil(rowsTotal / rowsShown);
    $('#myTable tbody tr').hide();
    $('#myTable tbody tr').slice(0, rowsShown).show();
    $('#pageUp').bind('click', function () {
        if (currentpage < (numPages - 1)) {
            currentpage = (currentpage + 1);
            let startItem = (currentpage * rowsShown);
            let endItem = (startItem + rowsShown);
            $('#myTable tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).css('display', 'table-row').animate({ opacity: 1 }, 300);
            $('#pageNum').html(currentpage + 1);
        }
    });
    $('#pageDown').bind('click', function () {
        if (currentpage > 0) {
            currentpage = (currentpage - 1);
            let startItem = (currentpage * rowsShown);
            let endItem = (startItem + rowsShown);
            $('#myTable tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).css('display', 'table-row').animate({ opacity: 1 }, 300);
            $('#pageNum').html(currentpage + 1);
        }
    });
    $('#pageLast').bind('click', function () {
        currentpage = numPages - 1;
        let startItem = (currentpage * rowsShown);
        let endItem = (startItem + rowsShown);
        $('#myTable tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).css('display', 'table-row').animate({ opacity: 1 }, 300);
        $('#pageNum').html(currentpage + 1);
    });
    $('#pageFirst').bind('click', function () {
        currentpage = 0;
        let startItem = (currentpage * rowsShown);
        let endItem = (startItem + rowsShown);
        $('#myTable tbody tr').css('opacity', '0.0').hide().slice(startItem, endItem).css('display', 'table-row').animate({ opacity: 1 }, 300);
        $('#pageNum').html(currentpage + 1);
    });
});
