function BindReport(report) {
    debugger;
    // Get models. models contains enums that can be used.
    var models = window['powerbi-client'].models;
    var config = {
        type: 'report',
        accessToken: report.accessToken,
        embedUrl: report.embedUrl,
        id: report.reportId,
        permissions: models.Permissions.View,
        settings: {
            filterPaneEnabled: false,
            navContentPaneEnabled: false,
            visualSettings: {
                visualHeaders: [
                    {
                        settings: {
                            visible: false
                        }
                        // No selector - Hide visual header for all the visuals in the report
                    }
                ]
            }
        }
    };
    // Grab the reference to the div HTML element that will host the report.
    var reportContainer = document.getElementById('reportContainer');
    // Embed the report and display it within the div container.
    report = powerbi.embed(reportContainer, config);
}

function print() {
    // Get a reference to the embedded report HTML element
    var embedContainer = $('#reportContainer')[0];
    // Get a reference to the embedded report.
    report = powerbi.get(embedContainer);
    // Trigger the print dialog for your browser.
    report.print()
        .catch(function (errors) {
            Log.log(errors);
        });
}