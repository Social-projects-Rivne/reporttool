'use strict';

templatesManagerModule.factory('TemplateFactory', ['$http', function ($http) {
    var templateFactory = {
        GetAllTemplates: GetAllTemplates,
        GetAllTemplateFields: GetAllTemplateFields,
        AddNewTemplate: AddNewTemplate,
        EditTemplate: EditTemplate,
        deleteTemplate: deleteTemplate,
        serverResponse: serverResponse,
        isValidForAdd: isValidForAdd,
        isValidForEdit: isValidForEdit
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
                return $q.reject(response.data);
            }
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

    function isValidForAdd(template) {
        var valid = true;
        
        return valid;
    }

    function isValidForEdit(template) {
        var valid = true;

        return valid;
    }

    function serverResponse(response) {
        if (response.data.Answer == 'WrongTemplate') {
            window.alert('Template is empty');
        }
        if (response.data.Answer == 'WrongName') {
            window.alert('Template name is incorrect');
        }
        if (response.data.Answer == 'FieldsIsEmpty') {
            window.alert('No fields in template');
        }
        if (response.data.Answer == 'FieldIsNotCorrect') {
            window.alert('Field of template is not correct');
        }
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