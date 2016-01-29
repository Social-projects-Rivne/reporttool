'use strict'

loginModule.controller("loginController", ['$scope', '$http', '$stateParams', '$state', function ($scope, $http, $stateParams, $state) {

    $scope.credentials = {
        userName: '',
        password: '',
    };

    $scope.showAuthentificationError = false;
    $scope.showConnectionError = false;
    $scope.errorText = "";
    $scope.validationIsInProgress = false;

    $scope.HideErrors = function () {
        $scope.errorText = "";
        $scope.showAuthentificationError = false;
        $scope.showConnectionError = false;
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
                    $scope.showConnectionError = true;
                }
                else
                    if (r.data.Status == "validCredentials") {
                        //redirect on main page
                        $state.transitionTo('mainView');
                    }
                    else
                        if (r.data.Status == "invalidCredentials") {
                            $scope.errorText = "Wrong user name or password";
                            $scope.showAuthentificationError = true;
                        }

            },
            function (response) {
                $scope.validationIsInProgress = false;
                $scope.errorText = "Server error";
                $scope.showConnectionError = true;
            }
         );
    }

}]);