'use strict';

reportsManagerModule.factory('ReportsFactory', ['$http', function ($http) {
    var reportsFactory = {
        getTeams: getTeams,
        getTemplates: getTemplates,
        getFields: getFields,
        getUserActivity: getUserActivity,       //  debug
        getTemplateFields: getTemplateFields   //  debug
    };

    function getTeams() {
        return $http.get("Teams/GetAllTeams");
    }

    function getTemplates() {
        return $http.get("Templates/GetAllTemplates");
    }

    function getFields(templateId) {
        //console.log ($http.get("Templates/GetTemplateFields?templateId=" + templateId) );   //  debug
        return $http.get("Templates/GetTemplateFields?templateId=" + templateId);
    }

    //  --- a method from TemplateFactory -----------------------------------------------------------   //  debug
    //  1
    //function getTemplateFields(templateId) {
    //    var promise1 = $http.get("Templates/GetTemplateFields?templateId=" + templateId);   //  debug
    //    console.log(promise1);   //  debug

    //    return $http.get("Templates/GetTemplateFields?templateId=" + templateId)
    //    .then(function (response) {
    //        if (typeof response.data === 'object') {
    //            return response.data;
    //        } else {
    //            return $q.reject(response.data);
    //        }
    //    });
    //}

    //  2
    function getTemplateFields(templateId) {
        var fields = $http.get("Templates/GetTemplateFields?templateId=" + templateId);
        return fields;
    }
    //  ---------------------------------------------------------------------------------------------------------------  //  debug

    //  BE params
    // GetUserActivity(string userName, string dateFrom, string dateTo)

    //  example
    //function getTeams() {
    //    return $http.get("Teams/GetAllTeams");
    //}

    // v.001
    //function getUserActivity(userName, dateFrom, dateTo) {
    //    return $http.get("Reports/GetUserActivity?userName=" + userName + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo)
    //    .then(function (response) {
    //        if (typeof response.data === 'object') {
    //            return response.data;
    //        } else {
    //            return $q.reject(response.data);
    //        }
    //    });
    //}

    function getUserActivity(userName, dateFrom, dateTo) {
        return $http.get("Reports/GetUserActivity?userName=" + userName + "&dateFrom= " + "01/01/2016" + "&dateTo=" + dateTo + "01/03/2016");
        //  + "&dateFrom='" + dateFrom + "'&dateTo='" + dateTo + "'"
    }

    return reportsFactory;
}]);

