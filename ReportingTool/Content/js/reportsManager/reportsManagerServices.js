'use strict';

reportsManagerModule.factory('ReportsFactory', ['$http', function ($http) {
    var reportsFactory = {
        getTeams: getTeams,
        getTemplates: getTemplates,
        getFields: getFields,
        getTemplateFields: getTemplateFields,   //  debug
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


    return reportsFactory;
}]);

