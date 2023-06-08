
$(document).ready(function () {
    google.load('visualization', '1.0', { 'packages': ['corechart'] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        $.ajax({
            url: '/User/GetAllApplications',

            traditional: true,
            success: function (result) {
                console.log(result);
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Mission Id');
                data.addColumn('number', 'Number of Applications');
                $.each(result, function (i, resultdata) {

                    
                    if ( resultdata.length > 0) {
                        data.addRows([
                            [resultdata[0].missionId.toString(), resultdata.length]
                        ]);
                    }
                })

                // Set chart options  
                var options = {
                    'title': 'Mission Applications',
                    'width': 1000,
                    'height': 800,
                    hAxis: {
                        title: 'Mission Id',
                       
                    },
                    vAxis: {
                        title: 'Total No. Applications'
                    }
                };
/*                var chart = new google.visualization.ColumnChart($("#chart_container")[0]);
*/
                var chart = new google.visualization.LineChart($("#chart_container")[0]);

/*                var chart = new google.visualization.AreaChart($("#chart_container")[0]);
*/
/*                var chart = new google.visualization.BarChart($("#chart_container")[0]);
*/
/*                var chart = new google.visualization.PieChart($("#chart_container")[0]);
*/                chart.draw(data, options);
            },
            error: function () {
                alert('An error occurred while saving the data.');
            }
        })


    }
});
