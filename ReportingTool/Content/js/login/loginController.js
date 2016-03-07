'use strict'

loginModule.controller("loginController",
    ['$scope', '$state', '$http', '$stateParams', 'LoginService',
    function ($scope, $state, $http, $stateParams, LoginService) {

        angular.element(document).ready(function () {
            $scope.CheckSession();
        });

        $scope.credentials = {
            userName: '',
            password: '',
        };
        $scope.showErrors = {
            showAuthentificationError: false,
            showConnectionError: false
        };

        $scope.showLogin = LoginService.GetShowLoginStatus();

        $scope.errorText = "";
        $scope.validationIsInProgress = false;

        $scope.HideErrors = function () {
            $scope.errorText = "";
            $scope.showErrors.showAuthentificationError = false;
            $scope.showErrors.showConnectionError = false;
        };

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
                    $state.transitionTo('mainView');
                }
                else {
                    LoginService.SetShowLogoutStatus(false);
                    //redirect on main page
                    LoginService.SetShowLoginStatus(true);
                    $state.transitionTo('loginView');
                }
            }, function errorCallback(response) {
                $scope.validationIsInProgress = false;
                $scope.errorText = "Server error";
                $scope.showErrors.showConnectionError = true;
            });
        };


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
                    //$scope.validationIsInProgress = false;
                    if (r.data.Status == "connectionError") {
                        $scope.validationIsInProgress = false;
                        $scope.errorText = "Can not connect to Jira host";
                        $scope.showErrors.showConnectionError = true;
                    }
                    else
                        if (r.data.Status == "validCredentials") {
                            //LoginService.SetShowLogoutStatus(true);
                            //redirect on main page
                            //$state.transitionTo('mainView');                
                            $http.get("JiraUsers/CreateBackendStorage").
                                then(
                                    function (res) {
                                        $scope.validationIsInProgress = false;
                                        LoginService.SetShowLogoutStatus(true);
                                        $state.transitionTo('mainView');
                                    },
                                    function (res) {
                                        $scope.validationIsInProgress = false;
                                        alert("Can not get correct data from Jira");                                     
                                        console.log(res.data);
                                        LoginService.SetShowLogoutStatus(true);
                                        LoginService.SetShowLoginStatus(false);
                                                                                $state.transitionTo('mainView');
                                    });
                        }
                        else
                            if (r.data.Status == "invalidCredentials") {
                                $scope.validationIsInProgress = false;
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

    }]);
