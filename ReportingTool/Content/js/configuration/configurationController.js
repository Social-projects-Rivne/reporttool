'use strict'

configurationModule.controller("configurationController", ['$scope', '$http', '$stateParams', '$state', function ($scope, $http, $stateParams, $state) {

    angular.element(document).ready(function () {
        $scope.CheckConfigurationFile();
    });

    //object stores information about configuration
    $scope.configurationData = {
        ServerUrl: '',
        ProjectName: '',
    };

    $scope.isConfigurationNotReady = false;
    $scope.isConfigurationFileError = false;
    $scope.fileSavingError = false;
    $scope.serverError = false;

    $scope.fileStatusMessage = '';
    $scope.fileNotSavedMessage = '';
    $scope.serverErrorMessage = '';

    $scope.CheckConfigurationFile = function () {

        var req = {
            url: '/Configuration/SetConfigurations',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        };

        $http(req).then(
            function (r) {
                if (r.data.Answer == 'NotValid') {
                    $scope.isConfigurationNotReady = true;
                    $scope.isConfigurationFileError = true;
                    $scope.fileStatusMessage = "Configuration file is not valid";
                }
                else
                    if (r.data.Answer == 'IsEmpty') {
                        $scope.isConfigurationNotReady = true;
                        $scope.isConfigurationFileError = true;
                        $scope.fileStatusMessage = "Configuration file is empty";
                    }
                    else
                        if (r.data.Answer == 'NotExists') {
                            $scope.isConfigurationNotReady = true;
                            $scope.isConfigurationFileError = true;
                            $scope.fileStatusMessage = "Configuration file does not exist";
                        }
                        else
                            if (r.data.Answer == 'Exists') {
                                //redirect on main login page
                                $state.transitionTo('loginView');

                            }
            },
            function (response) {
                $scope.isConfigurationNotReady = true;
                $scope.serverError = true;
                $scope.serverErrorMessage = "Server error";
            }
         );
    }

    $scope.SendConfigurationDataToServer = function () {

        var req = {
            url: '/Configuration/SetConfigurations',
            method: 'POST',
            data: JSON.stringify($scope.configurationData),
            headers: { 'content-type': 'application/json' }
        };

        $http(req).then(
            function (r) {
                if (r.data.Answer == 'Created') {
                    //redirect on main login page
                    $state.transitionTo('loginView');

                }
                else
                    if (r.data.Answer == 'NotCreated') {
                        $scope.fileSavingError = true;
                        $scope.fileNotSavedMessage = "Error has occured while saving configuration file";
                    }
            },
            function (response) {
                $scope.serverError = true;
                $scope.serverErrorMessage = "Server error";
            }
         );
    }
}]);