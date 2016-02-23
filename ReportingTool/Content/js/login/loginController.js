'use strict'

loginModule.controller("loginController", ['$scope', '$state', '$http', '$stateParams', 'LoginService', '$rootScope', function ($scope, $state, $http, $stateParams, LoginService, $rootScope) {

    angular.element(document).ready(function () {
        $scope.showLogout = LoginService.GetShowLogoutStatus();
        $scope.CheckSession();
    });

    $scope.credentials = {
        userName: '',
        password: '',
    };
    $scope.showErrors = {
       showAuthentificationError : false,
       showConnectionError : false
    };

    $rootScope.showLogout = false;
    $scope.showLogout = $rootScope.showLogout;

    $scope.showErrors.showAuthentificationError = false;
    $scope.showErrors.showConnectionError = false;
    $scope.errorText = "";
    $scope.validationIsInProgress = false;

    $scope.HideErrors = function () {
        $scope.errorText = "";
        $scope.showErrors.showAuthentificationError = false;
        $scope.showErrors.showConnectionError = false;
    }

    $scope.CheckSession = function () {

        $http({
            method: 'GET',
            url: 'Login/CheckSession',
            data: {},
            headers: { 'content-type': 'application/json' }

        }).then(function successCallback(response) {

            if (response.data.Status == "sessionExists") {
                //redirect on main page
                LoginService.SetShowLogoutStatus(true);
                $scope.showLogout = LoginService.GetShowLogoutStatus();
                $state.transitionTo('mainView');
            }
            else {
                LoginService.SetShowLogoutStatus(false);
                $scope.showLogout = LoginService.GetShowLogoutStatus();
                //redirect on main page
                $state.transitionTo('loginView');
            }
        }, function errorCallback(response) {
            $scope.validationIsInProgress = false;
            $scope.errorText = "Server error";
            $scope.showErrors.showConnectionError = true;
        });
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
                        LoginService.SetShowLogoutStatus(true);
                        $scope.showLogout = LoginService.GetShowLogoutStatus();
                        alert($scope.showLogout);
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
                    LoginService.SetShowLogoutStatus(false);
                    $scope.showLogout = LoginService.GetShowLogoutStatus();
                    alert($scope.showLogout);
                    $state.transitionTo('loginView');
                }
            },
            function (response) {
                alert("Server error");
            }
         );
    }
}]);