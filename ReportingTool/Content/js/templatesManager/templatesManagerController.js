'use strict';

templatesManagerModule.controller("templatesManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory', function ($scope, $stateParams, $state, TemplateFactory) {

        $scope.templates = {};

        $scope.idSelectedTemplate = null;
        $scope.setSelected = function (idSelectedTemplate) {
            $scope.idSelectedTemplate = idSelectedTemplate;
            console.log(idSelectedTemplate);
        }

        TemplateFactory.GetAllTemplates().then(templatesSuccess, templatesFail);

        function templatesSuccess(response) {
            $scope.templates = response.data;
        };

        function templatesFail(response) {
            alert("Error: " + response.code + " " + response.statusText);
        };

    }]);

templatesManagerModule.controller("templatesFieldsManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory', function ($scope, $stateParams, $state, TemplateFactory) {

        $scope.templateData = {};
        $scope.templateId = $stateParams.templateId;
        $scope.fields = getFields();
        $scope.existData = false;
        $scope.isOwner = false;

        var fieldEnum = {
            Reporter: "Reporter",
            Receiver: "Receiver",
            UsualTasks: "UsualTasks",
            RisksAndIssues: "RisksAndIssues",
            PlannedActivities: "PlannedActivities",
            PlannedVacations: "PlannedVacations",
            UserActivities: "UserActivities",
            Reasons: "Reasons"
        }

        $scope.filedEnum = fieldEnum;
        $scope.fieldValueGeneratedAutomatically = "Field data will be generated automatically";
        $scope.fieldValueSetManually = "Field data must be set manually";

        function getFields() {
            TemplateFactory.GetAllTemplateFields($scope.templateId)
            .then(function (data) {
                // promise fulfilled
                if (data.length !== 0) {
                    templateFieldsSuccess(data);
                } else {
                    //TODO:Thinking about inserting here some check. Not nesseccary
                }
            }, function (error) {
                // promise rejected, could log the error with: console.log('error', error);
                templateFieldsFail(error);
            });
        }

        function templateFieldsSuccess(data) {
            // promise fulfilled
            $scope.templateData = data;
            $scope.fields = data.Fields;
            $scope.existData = true;
            $scope.isOwner = data.IsOwner;
        };

        function templateFieldsFail(error) {
            // promise rejected, could log the error with: console.log('error', error);
            alert("Error: " + error.code + " " + error.statusText);
        };

        $scope.getFieldValue = function (field) {
            if (field.DefaultValue)
                return field.DefaultValue;
            else {
                if (field.FieldName.toLowerCase() === fieldEnum.RisksAndIssues.toLowerCase() ||
                    field.FieldName.toLowerCase() === fieldEnum.PlannedActivities.toLowerCase() ||
                    field.FieldName.toLowerCase() === fieldEnum.UserActivities.toLowerCase()) {
                    return $scope.fieldValueGeneratedAutomatically;
                } else {
                    return $scope.fieldValueSetManually;
                }
            }
        }
    }]);