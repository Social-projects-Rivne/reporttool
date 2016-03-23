reportsManagerModule.factory('ReportsFactory', ['$http', function ($http) {
    var reportsFactory = {
        getTeams: getTeams,
        getTemplates: getTemplates,
        getFields: getFields,
        getJiraUsers: getJiraUsers
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

    function getJiraUsers(param) {
        return $http.get("JiraUsers/GetAllUsers", { params: { "searchValue": param } });
    }

    return reportsFactory;
}]);