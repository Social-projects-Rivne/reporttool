'use strict';

templatesManagerModule.factory('TemplateFactory', ['$http', function ($http) {
    var templateFactory = {
        GetAllTemplates: GetAllTemplates,
        GetAllTemplateFields: GetAllTemplateFields
    };

    function GetAllTemplates() {
        var templates = $http.get("Templates/GetAllTemplates");
        return templates;
    };

    function GetAllTemplateFields(templateId) {
        // TODO: check why not working?
        //return $http({
        //    url: 'Templates/GetTemplateFields',
        //    method: 'POST',
        //    data: templateId,
        //    headers: { 'content-type': 'application/json' }
        //})
        return $http.get("Templates/GetTemplateFields?templateId=" + templateId)
        .then(function (response) {
            if (typeof response.data === 'object') {
                return response.data;
            } else {
                // invalid response
                return $q.reject(response.data);
            }
        }, function (response) {
            // something went wrong
            return $q.reject(response.data);
        });
    }
    return templateFactory;
}]);

templatesManagerModule.factory('FieldsFactory', ['$http', function ($http) {
    var fields = {
        getAllFields: getAllFields
    };

    function getAllFields() {
        return $http.get("Templates/GetAllFields");
    }

    return fields;
}]);