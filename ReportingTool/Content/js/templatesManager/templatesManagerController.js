'use strict';

templatesManagerModule.controller("templatesManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory',
        function ($scope, $stateParams, $state, TemplateFactory) {

            $scope.validationIsInProgress = true;
            $scope.idSelectedTemplate = null;
            $scope.setSelected = function (idSelectedTemplate) {
                $scope.idSelectedTemplate = idSelectedTemplate;
                console.log(idSelectedTemplate);
            }

            TemplateFactory.GetAllTemplates().then(templatesSuccess, templatesFail);

            function templatesSuccess(response) {
                $scope.templates = response.data;
                $scope.validationIsInProgress = false;
            };

            function templatesFail(response) {
                $scope.validationIsInProgress = false;
                alert("Error: " + response.code + " " + response.statusText);
            };



        }]);

templatesManagerModule.controller("templatesFieldsManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory', 'TempObjectFactory',
        function ($scope, $stateParams, $state, TemplateFactory, TempObjectFactory) {

            $scope.templateData = {};
            $scope.templateId = $stateParams.templateId;
            $scope.fields = getFields();
            $scope.existData = false;
            $scope.isOwner = false;
            $scope.validationIsInProgress = true;

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
                $scope.validationIsInProgress = false;
            };

            function templateFieldsFail(error) {
                // promise rejected, could log the error with: console.log('error', error);
                $scope.existData = true;    //  fix
                $scope.validationIsInProgress = false;
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

            $scope.edit = function (editTemplate) {
                TempObjectFactory.set(editTemplate);
                $scope.activeTeam = {};
                $state.go('mainView.teamsManager.editTeam');
            }

            //  deletetemplate
            var DeleteTemplate = null;
            //  deletetemplate
            $scope.deleteTemplate = function (deletedTemplateID) {

                var bAnswer = false;
                //  console.log($scope.templates);  //  debug
                console.log("deletedTemplateID = " + deletedTemplateID);  //  debug
                var bAnswer =
                    confirm("Are you sure you want to delete this template ?");
                //  + $scope.templates[deletedTemplateID - 1].templateName + 

                if (bAnswer == true) {
                    TemplateFactory
                        .deleteTemplate(deletedTemplateID)
                        .then(
                            deleteTemplateSuccess,
                            deleteTemplateFail);
                }
                else {
                    // $state.go('mainView.templatesManager.templatesList', {}, { reload: true });
                }
            };
            //  deletetemplate
            function deleteTemplateSuccess(response) {
                if (response.data.Answer == 'Deleted') {
                    //  $state.go('mainView.templatesManager', {}, { reload: true });   //  fix
                    $state.go('mainView.templatesManager.templatesList', {}, { reload: true });
                }
            }
            //  deletetemplate
            function deleteTemplateFail(response) {
                console.error("deleteTemplate failed!");
            }

        }]);

templatesManagerModule.controller('AddTemplateController',
    ['$scope', '$state', 'FieldsFactory', '$http', 'UserFactory', 'TemplateFactory',
        function ($scope, $state, FieldsFactory, $http, UserFactory, TemplateFactory) {
            $scope.tempTemplate = {
                templateName: '',
                fields: []
            };

            $scope.newTemplate = {
                templateName: '',
                fields: []
            };

            FieldsFactory.getAllFields().then(getFieldsSuccess, getFieldsFail);

            function getFieldsSuccess(responce) {
                $scope.tempTemplate.fields = responce.data;
            }

            function getFieldsFail(responce) {
                console.log('FAIL: ' + responce.message);
            }

            $scope.getJiraUsers = function (searchValue) {
                return UserFactory.getJiraUsers(searchValue).then(function (response) {
                    return response.data
                });
            };

            $scope.save = function () {
                $scope.newTemplate.templateName = $scope.tempTemplate.templateName;
                for (var i in $scope.tempTemplate.fields) {

                    if ($scope.tempTemplate.fields[i].isSelected) {
                        $scope.newTemplate.fields.push({
                            fieldID: $scope.tempTemplate.fields[i].fieldID,
                            defaultValue: $scope.tempTemplate.fields[i].fieldDefaultValue
                        });
                    }
                }

                TemplateFactory.AddNewTemplate($scope.newTemplate);
            };

        }]);

templatesManagerModule.controller('EditTemplateController',
    ['$scope', '$state', 'FieldsFactory', '$http', 'UserFactory', 'TemplateFactory', 'TempObjectFactory',
        function ($scope, $state, FieldsFactory, $http, UserFactory, TemplateFactory, TempObjectFactory) {
            $scope.tempTemplate = {
                templateName: '',
                fields: []
            };

            $scope.editedTemplate = {
                templateName: '',
                fields: []
            };

            FieldsFactory.getAllFields().then(getFieldsSuccess, getFieldsFail);

            function getFieldsSuccess(responce) {
                $scope.tempTemplate.fields = responce.data;
            }

            function getFieldsFail(responce) {
                console.log('FAIL: ' + responce.message);
            }

            $scope.getJiraUsers = function (searchValue) {
                return UserFactory.getJiraUsers(searchValue).then(function (response) {
                    return response.data
                });
            };

            $scope.save = function () {
                $scope.editedTemplate.templateName = $scope.tempTemplate.templateName;
                for (var i in $scope.tempTemplate.fields) {

                    if ($scope.tempTemplate.fields[i].isSelected) {
                        $scope.editedTemplate.fields.push({
                            fieldID: $scope.tempTemplate.fields[i].fieldID,
                            defaultValue: $scope.tempTemplate.fields[i].fieldDefaultValue
                        });
                    }
                }

                TemplateFactory.EditTemplate($scope.editedTemplate);
            };

        }]);