﻿'use strict'

loginModule.controller("loginController", ['$scope', '$http', '$stateParams', '$state', function ($scope, $http, $stateParams, $state) {

    $scope.credentials = {
        userName: '',
        password: '',
    };
    $scope.showErrors = {
       showAuthentificationError : false,
       showConnectionError : false
    };
    $scope.showElements = {
        showLogout: true
    };
    $scope.showErrors.showAuthentificationError = false;
    $scope.showErrors.showConnectionError = false;
    $scope.errorText = "";
    $scope.validationIsInProgress = false;

    $scope.HideErrors = function () {
        $scope.errorText = "";
        $scope.showErrors.showAuthentificationError = false;
        $scope.showErrors.showConnectionError = false;
    }

    $scope.SendData = function () {

        var req = {
            url: 'Login/CheckCredentials',
            method: 'POST',
            data: $scope.credentials,
            headers: { 'content-type': 'application/json' }
        };

        $scope.validationIsInProgress = true;
        $http(req).then(
            function (r) {
                $scope.validationIsInProgress = false;
                if (r.data.Status == "connectionError") {
                    $scope.errorText = "Can not connect to Jira host";
                    $scope.showErrors.showConnectionError = true;
                }
                else
                    if (r.data.Status == "validCredentials") {
                        $scope.showElements.showLogout = true;
                        //redirect on main page
                        $state.transitionTo('mainView');
                    }
                    else
                        if (r.data.Status == "invalidCredentials") {
                            $scope.errorText = "Wrong user name or password";
                            $scope.showErrors.showAuthentificationError = true;
                        }

            },
            function (response) {
                $scope.validationIsInProgress = false;
                $scope.errorText = "Server error";
                $scope.showErrors.showConnectionError = true;
            }
         );
    }

    $scope.Logout = function () {

        var req = {
            url: 'Login/Logout',
            method: 'GET',
            headers: { 'content-type': 'application/json' }
        };

        $http(req).then(
            function (r) {
                if (r.data.Status == "loggedOut") {
                    $state.transitionTo('loginView');
                }
            },
            function (response) {
                alert("Server error");
            }
         );
    }

}]);