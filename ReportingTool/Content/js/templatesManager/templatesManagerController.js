'use strict';

templatesManagerModule.controller("templatesManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory',
        function ($scope, $stateParams, $state, TemplateFactory) {

            $scope.validationIsInProgress = true;
            $scope.idSelectedTemplate = null;
            $scope.setSelected = function (idSelectedTemplate) {
                $scope.idSelectedTemplate = idSelectedTemplate;
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
    ['$scope', '$stateParams', '$state', 'TemplateFactory', 'TempObjectFactory', '$uibModal',
        function ($scope, $stateParams, $state, TemplateFactory, TempObjectFactory, $uibModal) {

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


            $scope.animationsEnabled = true;
            $scope.open = function () {

                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalContent.html',
                    controller: 'ModalInstanceCtrl'
                });

                modalInstance.result.then(function () {
                    //if user select "YES"
                    $scope.deleteTemplate($scope.templateId);
                }, function () {
                    //if user select "NO"
                    $log.info('Modal dismissed at: ' + new Date());
                });
            };

            function getFields() {
                TemplateFactory.GetAllTemplateFields($scope.templateId)
                .then(function (data) {
                    templateFieldsSuccess(data);
                }, function (error) {
                    templateFieldsFail(error);
                });
            }

            function templateFieldsSuccess(data) {
                // promise fulfilled
                $scope.templateData = data;
                $scope.fields = data.fields;
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
                if (field.fieldDefaultValue)
                    return field.fieldDefaultValue;
                else {
                    if (field.fieldName.toLowerCase() === fieldEnum.RisksAndIssues.toLowerCase() ||
                        field.fieldName.toLowerCase() === fieldEnum.PlannedActivities.toLowerCase() ||
                        field.fieldName.toLowerCase() === fieldEnum.UserActivities.toLowerCase()) {
                        return $scope.fieldValueGeneratedAutomatically;
                    } else {
                        return $scope.fieldValueSetManually;
                    }
                }
            }

            $scope.edit = function () {
                var editedTemplate = {
                    templateId: $scope.templateId,
                    templateName: $scope.templateData.templateName,
                    fields: $scope.templateData.fields
                }

                TempObjectFactory.set(editedTemplate);
                $state.go('mainView.templatesManager.editTemplate');
            }

            ////TODO: For Marusiak A. Please, delete unnecessary commented lines, if you don't need them

            $scope.deleteTemplate = function (deletedTemplateID) {

                console.log("deletedTemplateID = " + deletedTemplateID);  //  debug
                TemplateFactory
                    .deleteTemplate(deletedTemplateID)
                    .then(
                        deleteTemplateSuccess,
                        deleteTemplateFail);
            };
            //  deletetemplate
            function deleteTemplateSuccess(response) {
                if (response.data.Answer == 'Deleted') {
                    $state.go('mainView.templatesManager.templatesList', {}, { reload: true });
                }
            }
            //  deletetemplate
            function deleteTemplateFail(response) {
                console.error("deleteTemplate failed!");
            }

            $scope.saveToTempAndGoToReportManager = function () {

                var saveTempTemplate = {
                    templateId: $scope.templateId,
                    templateName: $scope.templateData.templateName,
                    fields: $scope.templateData.fields
                }
                TempObjectFactory.set(saveTempTemplate);
                $state.go('mainView.reportsManager.reportsConditions');
            };

        }]);

// Please note that $uibModalInstance represents a modal window (instance) dependency.
// It is not the same as the $uibModal service used above.

templatesManagerModule.controller('ModalInstanceCtrl', function ($scope, $uibModalInstance) {


    $scope.ok = function () {
        $uibModalInstance.close();
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});

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

                TemplateFactory.AddNewTemplate($scope.newTemplate).then(addSuccess, addFail);
            };

            function addSuccess(response) {
                if (response.data.Answer == 'Added') {
                    $state.go('mainView.templatesManager.templatesList', {}, { reload: true });
                }
            }

            function addFail(response) {
                console.error("error during saving new template");
            }

            $scope.cancel = function () {
                $state.go('mainView.templatesManager.templatesList');
            }

        }]);

templatesManagerModule.controller('EditTemplateController',
    ['$scope', '$state', 'FieldsFactory', '$http', 'UserFactory', 'TemplateFactory', 'TempObjectFactory',
        function ($scope, $state, FieldsFactory, $http, UserFactory, TemplateFactory, TempObjectFactory) {

            $scope.tempTemplate = {};

            $scope.editedTemplate = {
                templateId: 0,
                templateName: '',
                fields: []
            };

            FieldsFactory.getAllFields().then(getFieldsSuccess, getFieldsFail);

            function getFieldsSuccess(responce) {
                $scope.tempTemplate = TempObjectFactory.get();
                var emptyFields = responce.data;

                for (var i in $scope.tempTemplate.fields) {
                    for (var j in responce.data) {
                        if ($scope.tempTemplate.fields[i].fieldID === responce.data[j].fieldID) {
                            emptyFields.splice(j, 1);
                        }
                    }
                }

                $scope.tempTemplate.fields = $scope.tempTemplate.fields.concat(emptyFields);
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
                $scope.editedTemplate.templateId = $scope.tempTemplate.templateId;
                $scope.editedTemplate.templateName = $scope.tempTemplate.templateName;
                for (var i in $scope.tempTemplate.fields) {

                    if ($scope.tempTemplate.fields[i].isSelected) {
                        $scope.editedTemplate.fields.push({
                            fieldID: $scope.tempTemplate.fields[i].fieldID,
                            defaultValue: $scope.tempTemplate.fields[i].fieldDefaultValue
                        });
                    }
                }
                TempObjectFactory.set({});
                TemplateFactory.EditTemplate($scope.editedTemplate).then(editSuccess, editFail);
            };

            function editSuccess(response) {
                if (response.data.Answer == 'Edited') {
                    $state.go('mainView.templatesManager.templatesList', {}, { reload: true });
                }
            }

            function editFail(response) {
                console.error("error during saving edited template");
            }

            $scope.cancel = function () {
                TempObjectFactory.set({});
                $state.go('mainView.templatesManager.templatesList');
            }

        }]);