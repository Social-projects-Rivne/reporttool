'use strict';

templatesManagerModule.factory('TemplateFactory', ['$http', '$uibModal', function ($http, $uibModal) {
    var templateFactory = {
        GetAllTemplates: GetAllTemplates,
        GetAllTemplateFields: GetAllTemplateFields,
        AddNewTemplate: AddNewTemplate,
        EditTemplate: EditTemplate,
        deleteTemplate: deleteTemplate,
        serverResponse: serverResponse,
        isValid: isValid
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

    var openModal = function (message) {
        $uibModal.open({
            animation: true,
            templateUrl: 'Modal.html',
            controller: 'ModalController',
            resolve: {
                message: function () {
                    return message;
                }
            }
        });
    }

    function isValid(template) {
        var valid = true;
        if (template.templateName == null || template.templateName == '' || template.templateName == ' ') {
            openModal('Template name is incorrect');
            valid = false;
        }
        else if (template.fields == null || template.fields == '' || template.fields == ' ') {
            openModal('No fields in template');
            valid = false;
        } else if (template.fields.every(field => field.isSelected == null || field.isSelected == '' || field.isSelected == ' ')) {
            openModal('No fields in template');
            valid = false;
        }
        return valid;
    }

    function serverResponse(response) {
        if (response.data.Answer == 'WrongTemplate') {
            openModal('Template is empty');
        }
        if (response.data.Answer == 'WrongName') {
            openModal('Template name is incorrect');
        }
        if (response.data.Answer == 'WrongOwnerName') {
            openModal('Template owner is incorrect');
        }
        if (response.data.Answer == 'FieldsIsEmpty') {
            openModal('No fields in template');
        }
        if (response.data.Answer == 'FieldIsNotCorrect') {
            openModal('Field of template is not correct');
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