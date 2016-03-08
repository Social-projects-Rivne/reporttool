'use strict';

templatesManagerModule.factory('TemplateFactory', ['$http', function ($http) {
    var templateFactory = {
        GetAllTemplates: GetAllTemplates,
        GetAllTemplateFields: GetAllTemplateFields,
        AddNewTemplate: AddNewTemplate,
        EditTemplate: EditTemplate,
        deleteTemplate: deleteTemplate
    };

    function GetAllTemplates() {
        var templates = $http.get("Templates/GetAllTemplates");
        return templates;
    }

    function GetAllTemplateFields(templateId) {
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

    function AddNewTemplate(newTemplate) {
        return $http({
            url: 'Templates/AddNewTemplate',
            method: 'POST',
            data: newTemplate,
            headers: { 'content-type': 'application/json' }
        });
    }

    function EditTemplate(editedTemplate) {
        return $http.put("Templates/EditTemplate", editedTemplate);
    }

    //  deletetemplate
    function deleteTemplate(id) {
        return $http.delete("Templates/DeleteTemplate", { params: { id: id } });
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