reportsManagerModule.factory('ReportsFactory', ['$http', function ($http) {
    var reportsFactory = {
        getTeams: getTeams,
        getTemplates: getTemplates,
        getFields: getFields
    };

    function getTeams() {
        return $http.get("Teams/GetAllTeams");
    }

    function getTemplates() {
        return $http.get("Templates/GetAllTemplates");
    }

    function getFields(templateId) {
        return $http.get("Templates/GetTemplateFields?templateId=" + templateId);
    }
    return reportsFactory;
}]);