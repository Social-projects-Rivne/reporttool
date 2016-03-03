'use strict';

templatesManagerModule.controller("templatesManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory',
        function ($scope, $stateParams, $state, TemplateFactory) {

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

            //  deletetemplate
            var DeleteTemplate = null;
            //  deletetemplate
            $scope.deleteTemplate = function (deletedTemplateID) {
                TemplateFactory
                    .deleteTemplate(deletedTemplateID)
                    .then(
                        deleteTemplateSuccess,
                        deleteTemplateFail);
            }
            //  deletetemplate
            function deleteTemplateSuccess(response) {
                if (response.data.Answer == 'Deleted') {
                    $state.go('mainView.templatesManager', {}, { reload: true });
                }
            }
            //  deletetemplate
            function deleteTemplateFail(response) {
                console.error("deleteTemplate failed!");
            }
            

        }]);

templatesManagerModule.controller("templatesFieldsManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory', function ($scope, $stateParams, $state, TemplateFactory) {

        $scope.templateData = {};
        $scope.templateId = $stateParams.templateId;
        $scope.fields = getFields();
        $scope.existData = false;
        //TODO: set a value from Session object
        $scope.userNameFromSession = 'oharitc';

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
        };

        function templateFieldsFail(error) {
            // promise rejected, could log the error with: console.log('error', error);
            alert("Error: " + error.code + " " + error.statusText);
        };

        $scope.isOwner = function () {
            if ($scope.templateData.Owner === $scope.userNameFromSession) {
                return true;
            } else {
                return false;
            }
        }

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